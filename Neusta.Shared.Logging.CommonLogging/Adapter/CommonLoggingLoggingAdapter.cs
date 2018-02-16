namespace Neusta.Shared.Logging.CommonLogging.Adapter
{
	using Neusta.Shared.Logging.Base;

	internal sealed class CommonLoggingLoggingAdapter : BaseLoggingAdapter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommonLoggingLoggingAdapter"/> class.
		/// </summary>
		public CommonLoggingLoggingAdapter(ILoggingConfiguration configuration)
			: base(configuration)
		{
		}

		#region Overrides of BaseLoggerAdapter

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override ILogger CreateLogger(string name)
		{
			return new CommonLoggingLogger(this, new SimpleLogger(name), name);
		}

		#endregion
	}
}