namespace Neusta.Shared.AspNetCore.Extensions.Logging.Adapter
{
	using JetBrains.Annotations;
	using Neusta.Shared.Logging;
	using Neusta.Shared.Logging.Base;

	[UsedImplicitly]
	internal sealed class RedirectLoggingAdapterBuilder : BaseLoggingAdapterBuilder
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RedirectLoggingAdapterBuilder"/> class.
		/// </summary>
		public RedirectLoggingAdapterBuilder()
		{
		}

		#region Overrides of BaseLoggerAdapterBuilder

		/// <summary>
		/// Builds the logger adapter.
		/// </summary>
		public override ILoggingAdapter Build(ILoggingConfiguration configuration)
		{
			if (RedirectLoggingAdapter.Instance != null)
			{
				return RedirectLoggingAdapter.Instance;
			}
			return new RedirectLoggingAdapter(configuration);
		}

		#endregion
	}
}