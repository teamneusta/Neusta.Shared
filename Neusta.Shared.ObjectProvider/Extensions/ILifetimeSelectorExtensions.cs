// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	// ReSharper disable once InconsistentNaming
	public static class ILifetimeSelectorExtensions
	{
		/// <summary>
		/// Registers each matching concrete type with <see cref="ServiceLifetime.Singleton"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector WithSingletonLifetime(this ILifetimeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalWithLifetime(ServiceLifetime.Singleton);
		}

		/// <summary>
		/// Registers each matching concrete type with <see cref="ServiceLifetime.Scoped"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector WithScopedLifetime(this ILifetimeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalWithLifetime(ServiceLifetime.Scoped);
		}

		/// <summary>
		/// Registers each matching concrete type with <see cref="ServiceLifetime.PerResolutionRequest"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector WithPerResolutionRequestLifetime(this ILifetimeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalWithLifetime(ServiceLifetime.PerResolutionRequest);
		}

		/// <summary>
		/// Registers each matching concrete type with <see cref="ServiceLifetime.Transient"/> lifetime.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector WithTransientLifetime(this ILifetimeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalWithLifetime(ServiceLifetime.Transient);
		}

		/// <summary>
		/// Registers each matching concrete type with the specified <paramref name="lifetime"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector WithLifetime(this ILifetimeSelector selector, ServiceLifetime lifetime)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalWithLifetime(lifetime);
		}
	}
}