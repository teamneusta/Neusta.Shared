namespace Neusta.Shared.Logging.Adapter.Null
{
	using Neusta.Shared.Logging.Base;

	internal sealed class NullLoggingAdapterBuilder : BaseLoggingAdapterBuilder
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NullLoggingAdapterBuilder"/> class.
		/// </summary>
		public NullLoggingAdapterBuilder()
		{
		}

		#region Overrides of BaseLoggingAdapterBuilder

		/// <summary>
		/// Builds the logger adapter.
		/// </summary>
		public override ILoggingAdapter Build(ILoggingConfiguration configuration)
		{
			return new NullLoggingAdapter(configuration);
		}

		#endregion
	}
}