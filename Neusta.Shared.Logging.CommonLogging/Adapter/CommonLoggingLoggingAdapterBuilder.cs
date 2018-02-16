namespace Neusta.Shared.Logging.CommonLogging.Adapter
{
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Base;

	[UsedImplicitly]
	internal sealed class CommonLoggingLoggingAdapterBuilder : BaseLoggingAdapterBuilder
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommonLoggingLoggingAdapterBuilder"/> class.
		/// </summary>
		public CommonLoggingLoggingAdapterBuilder()
		{
		}

		#region Overrides of BaseLoggerAdapterBuilder

		/// <summary>
		/// Builds the logger adapter.
		/// </summary>
		public override ILoggingAdapter Build(ILoggingConfiguration configuration)
		{
			return new CommonLoggingLoggingAdapter(configuration);
		}

		#endregion
	}
}