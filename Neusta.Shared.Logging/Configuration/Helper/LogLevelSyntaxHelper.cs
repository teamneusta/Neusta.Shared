namespace Neusta.Shared.Logging.Configuration.Helper
{
	using System;

	internal sealed class LogLevelSyntaxHelper<TResult> : BaseSyntaxHelper<TResult>, ILogLevelSyntax<TResult>
		where TResult : IFluentSyntax
	{
		private readonly Action<ILoggingConfiguration, LogLevel> setAction;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogLevelSyntaxHelper{TResult}" /> class.
		/// </summary>
		public LogLevelSyntaxHelper(ILoggingConfiguration configuration, TResult result, Action<ILoggingConfiguration, LogLevel> setAction)
			: base(configuration, result)
		{
			this.setAction = setAction;
		}

		#region Implementation of ILogLevelSyntax<out ILoggingBuilderSyntax>

		/// <summary>
		/// A log level describing that everything should be logged (ALL).
		/// </summary>
		public TResult All()
		{
			this.setAction(this.Configuration, LogLevel.All);
			return this.Result;
		}

		/// <summary>
		/// The TRACE log level.
		/// </summary>
		public TResult Trace()
		{
			this.setAction(this.Configuration, LogLevel.Trace);
			return this.Result;
		}

		/// <summary>
		/// The DEBUG log level.
		/// </summary>
		public TResult Debug()
		{
			this.setAction(this.Configuration, LogLevel.Debug);
			return this.Result;
		}

		/// <summary>
		/// The INFO log level.
		/// </summary>
		public TResult Info()
		{
			this.setAction(this.Configuration, LogLevel.Info);
			return this.Result;
		}

		/// <summary>
		/// The WARN log level.
		/// </summary>
		public TResult Warn()
		{
			this.setAction(this.Configuration, LogLevel.Warn);
			return this.Result;
		}

		/// <summary>
		/// The ERROR log level.
		/// </summary>
		public TResult Error()
		{
			this.setAction(this.Configuration, LogLevel.Error);
			return this.Result;
		}

		/// <summary>
		/// The FATAL log level.
		/// </summary>
		public TResult Fatal()
		{
			this.setAction(this.Configuration, LogLevel.Fatal);
			return this.Result;
		}

		/// <summary>
		/// A log level describing that no logging should happen (OFF).
		/// </summary>
		public TResult Off()
		{
			this.setAction(this.Configuration, LogLevel.Off);
			return this.Result;
		}

		/// <summary>
		/// The given log level.
		/// </summary>
		public TResult Set(LogLevel level)
		{
			this.setAction(this.Configuration, level);
			return this.Result;
		}

		#endregion
	}
}