namespace Neusta.Shared.AspNetCore.Extensions.Logging
{
	using System;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using Microsoft.Extensions.Logging;

	internal static class LogLevelTranslator
	{
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Neusta.Shared.Logging.LogLevel Translate(Nullable<LogLevel> logLevel)
		{
			if (logLevel.HasValue)
			{
				return Translate(logLevel.Value);
			}
			return Shared.Logging.LogLevel.All;
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Neusta.Shared.Logging.LogLevel Translate(LogLevel logLevel)
		{
			switch (logLevel)
			{
				case Microsoft.Extensions.Logging.LogLevel.Trace:
					return Neusta.Shared.Logging.LogLevel.Trace;
				case Microsoft.Extensions.Logging.LogLevel.Debug:
					return Neusta.Shared.Logging.LogLevel.Debug;
				case Microsoft.Extensions.Logging.LogLevel.Information:
					return Neusta.Shared.Logging.LogLevel.Info;
				case Microsoft.Extensions.Logging.LogLevel.Warning:
					return Neusta.Shared.Logging.LogLevel.Warn;
				case Microsoft.Extensions.Logging.LogLevel.Error:
					return Neusta.Shared.Logging.LogLevel.Error;
				case Microsoft.Extensions.Logging.LogLevel.Critical:
					return Neusta.Shared.Logging.LogLevel.Fatal;
				case Microsoft.Extensions.Logging.LogLevel.None:
					return Neusta.Shared.Logging.LogLevel.Off;
				default:
					return Neusta.Shared.Logging.LogLevel.Debug;
			}
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static LogLevel Translate(Neusta.Shared.Logging.LogLevel logLevel)
		{
			if (logLevel <= Neusta.Shared.Logging.LogLevel.Trace)
			{
				return Microsoft.Extensions.Logging.LogLevel.Trace;
			}
			if (logLevel <= Neusta.Shared.Logging.LogLevel.Debug)
			{
				return Microsoft.Extensions.Logging.LogLevel.Debug;
			}
			if (logLevel <= Neusta.Shared.Logging.LogLevel.Info)
			{
				return Microsoft.Extensions.Logging.LogLevel.Information;
			}
			if (logLevel <= Neusta.Shared.Logging.LogLevel.Warn)
			{
				return Microsoft.Extensions.Logging.LogLevel.Warning;
			}
			if (logLevel <= Neusta.Shared.Logging.LogLevel.Error)
			{
				return Microsoft.Extensions.Logging.LogLevel.Error;
			}
			if (logLevel <= Neusta.Shared.Logging.LogLevel.Fatal)
			{
				return Microsoft.Extensions.Logging.LogLevel.Critical;
			}
			return Microsoft.Extensions.Logging.LogLevel.None;
		}
	}
}