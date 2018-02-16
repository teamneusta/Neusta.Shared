// ReSharper disable once CheckNamespace
namespace Neusta.Shared.Logging
{
	using System;
	using JetBrains.Annotations;

	internal static class LogLevelExtensions
	{
		[PublicAPI]
		public static LogLevel Translate(this global::NLog.LogLevel level)
		{
			if (level == null)
			{
				return LogLevel.MinLevel;
			}
			if (level <= global::NLog.LogLevel.Trace)
			{
				return LogLevel.Trace;
			}
			if (level <= global::NLog.LogLevel.Debug)
			{
				return LogLevel.Debug;
			}
			if (level <= global::NLog.LogLevel.Info)
			{
				return LogLevel.Info;
			}
			if (level <= global::NLog.LogLevel.Warn)
			{
				return LogLevel.Warn;
			}
			if (level <= global::NLog.LogLevel.Error)
			{
				return LogLevel.Error;
			}
			if (level <= global::NLog.LogLevel.Fatal)
			{
				return LogLevel.Fatal;
			}
			return LogLevel.MaxLevel;
		}

		[PublicAPI]
		public static global::NLog.LogLevel Translate(this LogLevel level)
		{
			if ((level == null) || (level <= LogLevel.Trace))
			{
				return global::NLog.LogLevel.Trace;
			}
			if (level <= LogLevel.Debug)
			{
				return global::NLog.LogLevel.Debug;
			}
			if (level <= LogLevel.Info)
			{
				return global::NLog.LogLevel.Info;
			}
			if (level <= LogLevel.Warn)
			{
				return global::NLog.LogLevel.Warn;
			}
			if (level <= LogLevel.Error)
			{
				return global::NLog.LogLevel.Error;
			}
			if (level <= LogLevel.Fatal)
			{
				return global::NLog.LogLevel.Fatal;
			}
			return global::NLog.LogLevel.Off;
		}
	}
}
