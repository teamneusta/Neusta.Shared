// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	// ReSharper disable once InconsistentNaming
	public static class IAssemblySelectorExtensions
	{
		/// <summary>
		/// Will scan for types in the application assembly.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromApplicationAssembly(this IAssemblySelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			var assembly = selector.InternalGetApplicationAssembly();
			if (assembly == null)
			{
				throw new InvalidOperationException("No application assembly defined.");
			}
			return selector.FromAssemblies(assembly);
		}

		/// <summary>
		/// Will scan for types in each <see cref="Assembly"/> in <paramref name="assemblies"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssemblies(this IAssemblySelector selector, params Assembly[] assemblies)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalFromAssemblies(assemblies);
		}

		/// <summary>
		/// Will scan for types in each <see cref="Assembly"/> in <paramref name="assemblies"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssemblies(this IAssemblySelector selector, IEnumerable<Assembly> assemblies)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(assemblies, nameof(assemblies));

			return selector.InternalFromAssemblies(assemblies);
		}

		/// <summary>
		/// Will scan for types from the assembly of type <typeparamref name="T"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssemblyOf<T>(this IAssemblySelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalFromAssembliesOf(new[] { typeof(T).GetTypeInfo() });
		}

		/// <summary>
		/// Will scan for types from the assemblies of each <see cref="Type"/> in <paramref name="types"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssembliesOf(this IAssemblySelector selector, params Type[] types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalFromAssembliesOf(types.Select(x => x.GetTypeInfo()));
		}

		/// <summary>
		/// Will scan for types from the assemblies of each <see cref="Type"/> in <paramref name="types"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssembliesOf(this IAssemblySelector selector, IEnumerable<Type> types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(types, nameof(types));

			return selector.InternalFromAssembliesOf(types.Select(x => x.GetTypeInfo()));
		}

		/// <summary>
		/// Will scan for types from the assemblies of each <see cref="Type"/> in <paramref name="typeInfos"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssembliesOf(this IAssemblySelector selector, IEnumerable<TypeInfo> typeInfos)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(typeInfos, nameof(typeInfos));

			return selector.InternalFromAssembliesOf(typeInfos);
		}

	}
}