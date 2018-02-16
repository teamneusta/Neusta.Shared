namespace Neusta.Shared.ObjectProvider.Base
{
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging;

	public abstract class BaseContainerAdapter<T> : IContainerAdapter
		where T : BaseContainerAdapter<T>
	{
		private static readonly ILogger logger = LogManager.GetLogger<T>();

		private readonly IContainerConfiguration configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseContainerAdapter{T}" /> class.
		/// </summary>
		protected BaseContainerAdapter(IContainerConfiguration configuration)
		{
			this.configuration = configuration;
		}

		/// <summary>
		/// Gets the logger.
		/// </summary>
		[UsedImplicitly]
		protected static ILogger Logger
		{
			[DebuggerStepThrough]
			get => logger;
		}

		#region Implementation of IObjectProviderProvider

		/// <summary>
		/// Gets the <see cref="IObjectProvider"/>.
		/// </summary>
		public abstract IObjectProvider ObjectProvider { get; }

		#endregion

		#region Implementation of IContainerAdapter

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		public virtual IContainerConfiguration Configuration
		{
			[DebuggerStepThrough]
			get { return this.configuration; }
		}

		#endregion
	}
}