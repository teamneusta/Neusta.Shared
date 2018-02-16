namespace Neusta.Shared.Logging.Base
{
	public abstract class BaseLoggingAdapterBuilder : ILoggingAdapterBuilder
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseLoggingAdapterBuilder"/> class.
		/// </summary>
		protected BaseLoggingAdapterBuilder()
		{
		}

		#region Implementation of ILoggerAdapterBuilder

		/// <summary>
		/// Builds the logger adapter.
		/// </summary>
		public abstract ILoggingAdapter Build(ILoggingConfiguration configuration);

		/// <summary>
		/// Registers the given adapter as default logger.
		/// </summary>
		public virtual void RegisterAsDefault(ILoggingConfiguration configuration, ILoggingAdapter adapter)
		{
			LoggingAdapter.Register(adapter);
		}

		#endregion
	}
}