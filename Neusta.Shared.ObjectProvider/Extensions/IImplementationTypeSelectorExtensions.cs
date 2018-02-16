// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	// ReSharper disable once InconsistentNaming
	public static class IImplementationTypeSelectorExtensions
	{
		/// <summary>
		/// Adds all public, non-abstract classes from the selected assemblies to the <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceTypeSelector AddClasses(this IImplementationTypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalAddClasses(null, true);
		}

		/// <summary>
		/// Adds all non-abstract classes from the selected assemblies to the <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceTypeSelector AddClasses(this IImplementationTypeSelector selector,bool publicOnly)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalAddClasses(null, publicOnly);
		}


		/// <summary>
		/// Adds all public, non-abstract classes from the selected assemblies that
		/// matches the requirements specified in the <paramref name="action"/>
		/// to the <see cref="IServiceDescriptorCollection"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceTypeSelector AddClasses(this IImplementationTypeSelector selector, Action<IImplementationTypeFilter> action)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(action, nameof(action));

			return selector.InternalAddClasses(action, false);
		}

		/// <summary>
		/// Adds all non-abstract classes from the selected assemblies that
		/// matches the requirements specified in the <paramref name="action" />
		/// to the <see cref="T:Neusta.Shared.ObjectProvider.IServiceDescriptorCollection" />.
		/// </summary>
		public static IServiceTypeSelector AddClasses(this IImplementationTypeSelector selector, Action<IImplementationTypeFilter> action, bool publicOnly)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(action, nameof(action));

			return selector.InternalAddClasses(action, publicOnly);
		}

	}
}