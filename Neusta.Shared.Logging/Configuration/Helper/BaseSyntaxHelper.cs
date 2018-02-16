namespace Neusta.Shared.Logging.Configuration.Helper
{
	using System.Diagnostics;

	internal abstract class BaseSyntaxHelper<TResult> : IFluentSyntax
		where TResult : IFluentSyntax
	{
		private readonly ILoggingConfiguration configuration;
		private readonly TResult result;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSyntaxHelper" /> class.
		/// </summary>
		protected BaseSyntaxHelper(ILoggingConfiguration configuration, TResult result)
		{
			this.configuration = configuration;
			this.result = result;
		}

		/// <summary>
		/// Gets the <see cref="ILoggingConfiguration" />.
		/// </summary>
		public ILoggingConfiguration Configuration
		{
			[DebuggerStepThrough]
			get { return this.configuration; }
		}

		/// <summary>
		/// Gets the logging configuration syntax root.
		/// </summary>
		public TResult Result
		{
			[DebuggerStepThrough]
			get { return this.result; }
		}
	}
}