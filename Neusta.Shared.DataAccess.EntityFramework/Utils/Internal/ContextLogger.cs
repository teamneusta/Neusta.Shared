namespace Neusta.Shared.DataAccess.EntityFramework.Utils.Internal
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Text.RegularExpressions;
	using Neusta.Shared.Logging;

	internal static class ContextLogger
	{
		private static readonly ILogger logger = LogManager.GetLogger(typeof(ContextLogger));

		private static readonly string[] prefixToIgnore = { "-- Executing", "Started transaction", "Committed transaction", "Opened connection", "Closed connection", "INSERT [dbo].[AuditLog" };
		private static readonly string[] messageWithoutStartingNewLine = { "-- ", "Opened", "Closed" };
		private static readonly string[] namespacesToIgnoreForStacktrace = { "System.", "Neusta.Shared." };
		private static readonly Type[] classesToIgnoreForStacktrace = { typeof(ContextLogger) };

		public static bool IsLoggingEnabled
		{
			[DebuggerStepThrough]
			get { return logger.IsTraceEnabled; }
		}

		public static void LogMessage(string message)
		{
			if (logger.IsTraceEnabled && !string.IsNullOrWhiteSpace(message))
			{
				if (ShouldMessageBeIgnored(message))
				{
					if (IsMessageToBeReplacedByStackTrace(message))
					{
						LogStackTrace();
					}
				}
				else
				{
					message = FormatSql(message);
					WriteLogMessage(message);
				}
			}
		}

		#region Private Methods

		private static void WriteLogMessage(string s)
		{
			if (s.StartsWith("-- Failed", StringComparison.OrdinalIgnoreCase))
			{
				logger.Error(s);
			}
			else
			{
				logger.Trace(s);
			}
		}


		private static bool ShouldMessageBeIgnored(string message)
		{
			return prefixToIgnore.Any(message.StartsWith);
		}

		private static bool IsMessageToBeReplacedByStackTrace(string message)
		{
			return message.StartsWith(prefixToIgnore[0]);
		}

		private static void LogStackTrace()
		{
			var stackTrace = new StackTrace(true);
			StackFrame[] stackFrames = stackTrace.GetFrames();
			if (stackFrames != null)
			{
				foreach (StackFrame stackFrame in stackFrames.Skip(2))
				{
					MethodBase methodBase = stackFrame.GetMethod();
					int lineNumber = stackFrame.GetFileLineNumber();
					if (methodBase != null && lineNumber > 0)
					{
						Type declaringType = methodBase.DeclaringType;
						if (declaringType != null && !classesToIgnoreForStacktrace.Contains(declaringType))
						{
							string declaringTypeNamespace = (declaringType.Namespace ?? string.Empty) + ".";
							if (!namespacesToIgnoreForStacktrace.Any(x => declaringTypeNamespace.StartsWith(x, StringComparison.OrdinalIgnoreCase)))
							{
								WriteLogMessage($"-- {declaringType.FullName}.{methodBase.Name}: {lineNumber}");
								break;
							}
						}
					}
				}
			}
		}

		private static string FormatSql(string message)
		{
			if (!messageWithoutStartingNewLine.Any(message.StartsWith))
			{
				message = Environment.NewLine + message;
			}
			else
			{
				message = message.TrimEnd(Environment.NewLine.ToCharArray());
			}

			return Regex.Replace(message, @"((?<=SELECT )\r?\n *|(?<=DISTINCT )\r?\n *|(?<=, )\r?\n *|\r?\n *\))",
				string.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
		}

		#endregion
	}
}