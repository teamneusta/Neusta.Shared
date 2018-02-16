using System;

namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.Logging;
	using Neusta.Shared.ObjectProvider.Internal;

	internal sealed class RootSyntaxHelper : IContainerConfigurationSyntax
	{
		private static readonly ILogger logger = LogManager.GetLogger<ContainerBuilder>();

		private readonly IContainerConfiguration configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="RootSyntaxHelper" /> class.
		/// </summary>
		public RootSyntaxHelper(IContainerConfiguration configuration)
		{
			this.configuration = configuration;
		}

		#region Implementation of IContainerConfigurationSyntax

		/// <summary>
		/// Adds registrations using conventions specified using the <paramref name="scan"/>.
		/// </summary>
		public IContainerConfigurationSyntax Scan(Action<ITypeSourceSelector> scan)
		{
			return this.Scan(scan, RegistrationStrategy.Append);
		}

		/// <summary>
		/// Adds registrations using conventions specified using the <paramref name="scan"/>.
		/// </summary>
		public IContainerConfigurationSyntax Scan(Action<ITypeSourceSelector> scan, RegistrationStrategy strategy)
		{
			Guard.ArgumentNotNull(scan, nameof(scan));

			var selector = new TypeSourceSelector(this.configuration?.ApplicationAssembly);
			scan(selector);
			this.Populate(selector, strategy);

			return this;
		}

		/// <summary>
		/// Unknown types are resolved automatically.
		/// </summary>
		public IContainerConfigurationSyntax AutoResolveUnknownTypes()
		{
			this.configuration.AutoResolveUnknownTypes = true;
			return this;
		}

		#endregion

		#region Private Methods

		private void Populate(ISelector selector, RegistrationStrategy strategy)
		{
			var descriptors = this.configuration.ServiceDescriptors;
			var applier = new RegistrationStrategyApplier(descriptors);
			if (logger.IsTraceEnabled)
			{
				int cnt = descriptors.Count;
				selector.Populate(applier, strategy);
				cnt = descriptors.Count - cnt;
				logger.Trace("Added {0} service descriptors from assembly scanning.", cnt);
			}
			else
			{
				selector.Populate(applier, strategy);
			}
		}

		#endregion
	}
}
