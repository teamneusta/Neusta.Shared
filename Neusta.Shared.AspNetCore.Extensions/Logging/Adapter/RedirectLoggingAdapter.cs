namespace Neusta.Shared.AspNetCore.Extensions.Logging.Adapter
{
	using System.Diagnostics;
	using Neusta.Shared.Logging;
	using Neusta.Shared.Logging.Base;

	internal sealed class RedirectLoggingAdapter : BaseLoggingAdapter
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RedirectLoggingAdapter instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="RedirectLoggingAdapter"/> class.
		/// </summary>
		public RedirectLoggingAdapter(ILoggingConfiguration configuration)
			: base(configuration)
		{
			instance = this;
		}

		#region Internal Methods

		/// <summary>
		/// Gets the instance.
		/// </summary>
		internal static RedirectLoggingAdapter Instance
		{
			[DebuggerStepThrough]
			get { return instance; }
		}

		/// <summary>
		/// Updates the redirect targets.
		/// </summary>
		internal void UpdateRedirectTargets()
		{
			foreach (var kvp in this.GetKnownLoggers())
			{
				ILogger logger = kvp.Value;
				while (logger is IForwardingLogger forwardingLogger)
				{
					logger = forwardingLogger.TargetLogger;
				}
				if (logger is RedirectLogger redirectLogger)
				{
					redirectLogger.TargetLogger = RedirectLoggerFactory.Instance?.CreateLogger(logger.Name);
				}
			}
		}

		#endregion

		#region Overrides of BaseLoggerAdapter

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override ILogger CreateLogger(string name)
		{
			var logger = RedirectLoggerFactory.Instance?.CreateLogger(name);
			return new RedirectLogger(logger, name);
		}

		#endregion
	}
}