namespace Neusta.Shared.Logging.CommonLogging.Adapter
{
	using System;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.Logging.Base;

	internal class CommonLoggingLogger : BaseSimplifiedAdapterLogger<CommonLoggingLoggingAdapter>
	{
		private readonly SimpleLogger targetLogger;

		/// <summary>
		/// Initializes a new instance of the <see cref="CommonLoggingLogger"/> class.
		/// </summary>
		public CommonLoggingLogger(CommonLoggingLoggingAdapter adapter, SimpleLogger targetLogger, string name)
			: base(adapter, name)
		{
			this.targetLogger = targetLogger;
		}

		#region Overrides of BaseSimplifiedLogger

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool IsEnabled(global::Neusta.Shared.Logging.LogLevel level)
		{
			if ((level == null) || (level <= Neusta.Shared.Logging.LogLevel.Trace))
			{
				return this.targetLogger.IsTraceEnabled;
			}
			if (level <= Neusta.Shared.Logging.LogLevel.Debug)
			{
				return this.targetLogger.IsDebugEnabled;
			}
			if (level <= Neusta.Shared.Logging.LogLevel.Info)
			{
				return this.targetLogger.IsInfoEnabled;
			}
			if (level <= Neusta.Shared.Logging.LogLevel.Warn)
			{
				return this.targetLogger.IsWarnEnabled;
			}
			if (level <= Neusta.Shared.Logging.LogLevel.Error)
			{
				return this.targetLogger.IsErrorEnabled;
			}
			if (level <= Neusta.Shared.Logging.LogLevel.Fatal)
			{
				return this.targetLogger.IsFatalEnabled;
			}
			return false;
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void InternalLog(global::Neusta.Shared.Logging.LogLevel level, string message, Exception exception)
		{
			if ((level == null) || (level <= Neusta.Shared.Logging.LogLevel.Trace))
			{
				this.targetLogger.Trace(message, exception);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Debug)
			{
				this.targetLogger.Debug(message, exception);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Info)
			{
				this.targetLogger.Info(message, exception);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Warn)
			{
				this.targetLogger.Warn(message, exception);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Error)
			{
				this.targetLogger.Error(message, exception);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Fatal)
			{
				this.targetLogger.Fatal(message, exception);
			}
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void InternalLog(global::Neusta.Shared.Logging.LogLevel level, string message)
		{
			if ((level == null) || (level <= Neusta.Shared.Logging.LogLevel.Trace))
			{
				this.targetLogger.Trace(message);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Debug)
			{
				this.targetLogger.Debug(message);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Info)
			{
				this.targetLogger.Info(message);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Warn)
			{
				this.targetLogger.Warn(message);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Error)
			{
				this.targetLogger.Error(message);
			}
			else if (level <= Neusta.Shared.Logging.LogLevel.Fatal)
			{
				this.targetLogger.Fatal(message);
			}
		}

		#endregion
	}
}