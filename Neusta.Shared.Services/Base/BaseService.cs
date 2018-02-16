namespace Neusta.Shared.Services.Base
{
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging;

	[UsedImplicitly]
	public abstract class BaseService<TService>
		where TService : BaseService<TService>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static readonly ILogger<TService> staticLogger = LogManager.GetLogger<TService>();
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static readonly ILogger<TService> nullLogger = LogManager.GetNullLogger<TService>();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly ILogger<TService> logger;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseService{TService}"/> class.
		/// </summary>
		protected BaseService()
		{
			this.logger = staticLogger;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseService{TService}"/> class.
		/// </summary>
		protected BaseService(ILogger<TService> logger)
		{
			this.logger = logger ?? nullLogger;
		}

		#region Protected Properties

		/// <summary>
		/// Gets the logger.
		/// </summary>
		[PublicAPI]
		protected ILogger Logger
		{
			[DebuggerStepThrough]
			get { return this.logger; }
		}

		#endregion
	}
}