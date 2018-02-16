// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider;
	using Neusta.Shared.ObjectProvider.Configuration.Helper;
	using Neusta.Shared.ObjectProvider.Internal;
	using Neusta.Shared.ObjectProvider.NetStandard.Utils;

	// ReSharper disable once InconsistentNaming
	public static class IServiceCollectionExtensions
	{
		/// <summary>
		/// Adds registrations to the <paramref name="services"/> collection using
		/// conventions specified using the <paramref name="action"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceCollection Scan(this IServiceCollection services, Action<ITypeSourceSelector> action)
		{
			Guard.ArgumentNotNull(services, nameof(services));
			Guard.ArgumentNotNull(action, nameof(action));

			var selector = new TypeSourceSelector();
			action(selector);
			services.Populate(selector, RegistrationStrategy.Append);

			return services;
		}

		#region Private Methods

		private static void Populate(this IServiceCollection services, ISelector selector, RegistrationStrategy strategy)
		{
			var applier = new RegistrationStrategyApplier(services);
			selector.Populate(applier, strategy);
		}

		#endregion
	}
}