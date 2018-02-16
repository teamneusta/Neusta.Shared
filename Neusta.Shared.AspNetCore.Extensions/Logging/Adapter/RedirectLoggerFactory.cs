namespace Neusta.Shared.AspNetCore.Extensions.Logging.Adapter
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;

	public sealed class RedirectLoggerFactory : LoggerFactory
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static RedirectLoggerFactory instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="RedirectLoggerFactory"/> class.
		/// </summary>
		[UsedImplicitly]
		public RedirectLoggerFactory()
		{
			this.SetupInstance();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RedirectLoggerFactory"/> class.
		/// </summary>
		[UsedImplicitly]
		public RedirectLoggerFactory(IEnumerable<ILoggerProvider> providers)
			: base(providers)
		{
			this.SetupInstance();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RedirectLoggerFactory"/> class.
		/// </summary>
		[UsedImplicitly]
		public RedirectLoggerFactory(IEnumerable<ILoggerProvider> providers, LoggerFilterOptions filterOptions)
			: base(providers, filterOptions)
		{
			this.SetupInstance();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RedirectLoggerFactory"/> class.
		/// </summary>
		[UsedImplicitly]
		public RedirectLoggerFactory(IEnumerable<ILoggerProvider> providers, IOptionsMonitor<LoggerFilterOptions> filterOption)
			: base(providers, filterOption)
		{
			this.SetupInstance();
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		internal static RedirectLoggerFactory Instance
		{
			[DebuggerStepThrough]
			get { return instance; }
		}

		private void SetupInstance()
		{
			instance = this;
			RedirectLoggingAdapter.Instance?.UpdateRedirectTargets();
		}
	}
}