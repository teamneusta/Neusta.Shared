namespace Neusta.Shared.Logging.Adapter.Debug
{
	using Neusta.Shared.Logging.Base;

	internal sealed class DebugLoggingAdapterBuilder : BaseLoggingAdapterBuilder
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DebugLoggingAdapterBuilder"/> class.
		/// </summary>
		public DebugLoggingAdapterBuilder()
		{
		}

		#region Overrides of BaseLoggingAdapterBuilder

		/// <summary>
		/// Builds the logger adapter.
		/// </summary>
		public override ILoggingAdapter Build(ILoggingConfiguration configuration)
		{
			return new DebugLoggingAdapter(configuration);
		}

		#endregion
	}
}