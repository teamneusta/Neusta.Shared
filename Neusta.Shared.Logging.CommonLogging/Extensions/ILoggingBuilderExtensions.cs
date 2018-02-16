// ReSharper disable once CheckNamespace
namespace Neusta.Shared.Logging
{
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.CommonLogging.Adapter;

	// ReSharper disable once InconsistentNaming
	public static class ILoggingBuilderExtensions
	{
		/// <summary>
		/// Use CommonLogging.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder UseCommonLogging(this ILoggingBuilder builder)
		{
			builder.RegisterAdapterBuilder<CommonLoggingLoggingAdapterBuilder>();
			return builder;
		}
	}
}