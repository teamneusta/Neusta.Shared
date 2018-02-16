namespace Neusta.Shared.AspNetCore.Extensions.Logging.Adapter
{
	using System;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using Microsoft.Extensions.Logging;
	using Neusta.Shared.Logging.Base;

	internal class RedirectLogger : BaseSimplifiedLogger
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ILogger targetLogger;

		/// <summary>
		/// Initializes a new instance of the <see cref="RedirectLogger"/> class.
		/// </summary>
		public RedirectLogger(ILogger targetLogger, string name)
			: base(name)
		{
			this.targetLogger = targetLogger;
		}

		/// <summary>
		/// Gets or sets the target logger.
		/// </summary>
		public ILogger TargetLogger
		{
			[DebuggerStepThrough]
			get { return this.targetLogger; }
			[DebuggerStepThrough]
			set { this.targetLogger = value; }
		}

		#region Overrides of BaseSimplifiedLogger

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool IsEnabled(global::Neusta.Shared.Logging.LogLevel level)
		{
			if (this.targetLogger == null)
			{
				return false;
			}
			return this.targetLogger.IsEnabled(LogLevelTranslator.Translate(level));
		}

		/// <summary>
		/// Writes the diagnostic message and exception at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void InternalLog(global::Neusta.Shared.Logging.LogLevel level, string message, Exception exception)
		{
			this.targetLogger?.Log(LogLevelTranslator.Translate(level), exception, message);
		}

		/// <summary>
		/// Writes the diagnostic message at the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void InternalLog(global::Neusta.Shared.Logging.LogLevel level, string message)
		{
			this.targetLogger?.Log(LogLevelTranslator.Translate(level), message);
		}

		#endregion
	}
}