namespace Neusta.Shared.Logging
{
	using System;
	using System.Diagnostics;
	using System.Reflection;
	using System.Text;
	using JetBrains.Annotations;

	/// <summary>
	/// The LogManager manages the logging subsystem.
	/// </summary>
	public static class LogManager
	{
		/// <summary>
		/// Gets or sets the global threshold.
		/// </summary>
		[PublicAPI]
		public static LogLevel GlobalThreshold
		{
			[DebuggerStepThrough]
			get { return LoggingAdapter.Default.GlobalThreshold; }
			[DebuggerStepThrough]
			set { LoggingAdapter.Default.GlobalThreshold = value; }
		}

		/// <summary>
		/// Updates the logger settings for all active loggers.
		/// </summary>
		[PublicAPI]
		public static void UpdateLoggerSettings()
		{
			LoggingAdapter.Default.UpdateLoggerSettings();
		}

		/// <summary>
		/// Decreases the log enable counter and if it reaches -1 the logs are disabled.
		/// </summary>
		[PublicAPI]
		public static void DisableLogging()
		{
			LoggingAdapter.Default.DisableLogging();
		}

		/// <summary>
		/// Increases the log enable counter and if it reaches 0 the logs are disabled.
		/// </summary>
		[PublicAPI]
		public static void EnableLogging()
		{
			LoggingAdapter.Default.EnableLogging();
		}

		/// <summary>
		/// Returns <see langword="true"/> if logging is currently enabled.
		/// </summary>
		[PublicAPI]
		public static bool IsLoggingEnabled()
		{
			if (LoggingAdapter.IsInitialized)
			{
				return LoggingAdapter.Default.IsLoggingEnabled();
			}
			return false;
		}

		/// <summary>
		/// Returns <see langword="true"/> if logging for the given <see cref="LogLevel"/> is enabled by the global threshold.
		/// </summary>
		[PublicAPI]
		public static bool IsLoggingEnabled(LogLevel logLevel)
		{
			if (LoggingAdapter.IsInitialized)
			{
				return LoggingAdapter.Default.IsLoggingEnabled(logLevel);
			}
			return false;
		}

		/// <summary>
		/// Gets a logger that discards all log messages.
		/// </summary>
		[PublicAPI]
		public static ILogger GetNullLogger()
		{
			return LoggingAdapter.Default.GetNullLogger();
		}

		/// <summary>
		/// Gets a logger that discards all log messages.
		/// </summary>
		[PublicAPI]
		public static ILogger<T> GetNullLogger<T>()
		{
			return LoggingAdapter.Default.GetNullLogger<T>();
		}

		/// <summary>
		/// Gets the logger for the specified instance.
		/// </summary>
		[PublicAPI]
		public static ILogger GetLogger(object instance)
		{
			if (instance == null)
			{
				return LoggingAdapter.Default.GetNullLogger();
			}
			Type type = instance.GetType();
			string name = GetLoggerTypeName(type);
			return LoggingAdapter.Default.GetLogger(name);
		}

		/// <summary>
		/// Gets the logger for the specified type.
		/// </summary>
		[PublicAPI]
		public static ILogger GetLogger(Type type)
		{
			if (type == null)
			{
				return LoggingAdapter.Default.GetNullLogger();
			}
			string name = GetLoggerTypeName(type);
			return LoggingAdapter.Default.GetLogger(name);
		}

		/// <summary>
		/// Gets the specified named logger.
		/// </summary>
		[PublicAPI]
		public static ILogger GetLogger(string name)
		{
			return LoggingAdapter.Default.GetLogger(name);
		}

		/// <summary>
		/// Gets the logger for the specified type.
		/// </summary>
		[PublicAPI]
		public static ILogger<T> GetLogger<T>()
		{
			return LoggingAdapter.Default.GetLogger<T>();
		}

		/// <summary>
		/// Gets the specified named logger for the specified type.
		/// </summary>
		[PublicAPI]
		public static ILogger<T> GetLogger<T>(string name)
		{
			return LoggingAdapter.Default.GetLogger<T>(name);
		}

		#region Internal Methods

		/// <summary>
		/// Gets the logger name for the given type.
		/// </summary>
		[PublicAPI]
		internal static string GetLoggerTypeName(Type type)
		{
			var typeInfo = type.GetTypeInfo();
			if (typeInfo.IsGenericType)
			{
				string fullName = type.FullName;
				var sb = new StringBuilder(fullName.Length);
				int idx = fullName.IndexOf('`');
				if (idx > 0)
				{
					fullName = fullName.Substring(0, idx);
				}
				sb.Append(fullName);
				Type[] subTypes = typeInfo.GenericTypeArguments;
				if (subTypes.Length > 0)
				{
					sb.Append('<');
					bool isFirst = true;
					foreach (Type subType in subTypes)
					{
						if (isFirst)
						{
							isFirst = false;
						}
						else
						{
							sb.Append(',');
						}
						if (!subType.IsGenericParameter)
						{
							sb.Append(GetLoggerTypeName(subType));
						}
					}
					sb.Append('>');
				}
				return sb.ToString();
			}
			if (type.IsGenericParameter)
			{
				return type.Name;
			}
			return type.FullName;
		}

		#endregion
	}
}