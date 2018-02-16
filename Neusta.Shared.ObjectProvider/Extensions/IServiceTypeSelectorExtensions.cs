// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider.Configuration.Helper;

	// ReSharper disable once InconsistentNaming
	public static class IServiceTypeSelectorExtensions
	{
		private static readonly Expression<Func<IServiceProvider, object>> serviceProviderGetServiceFunc = y => y.GetService(typeof(object));
		private static readonly MethodInfo serviceProviderGetServiceMethodInfo = ((MethodCallExpression)serviceProviderGetServiceFunc.Body).Method;

		/// <summary>
		/// Registers each matching concrete type as itself.
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector AsSelf(this IServiceTypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.As(t => new[] { t });
		}

		/// <summary>
		/// Registers each matching concrete type as <typeparamref name="T"/>.
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector As<T>(this IServiceTypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.As(typeof(T));
		}

		/// <summary>
		/// Registers each matching concrete type as each of the specified <paramref name="types" />.
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector As(this IServiceTypeSelector selector, params Type[] types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.As(types.AsEnumerable());
		}

		/// <summary>
		/// Registers each matching concrete type as each of the specified <paramref name="types" />.
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector As(this IServiceTypeSelector selector, IEnumerable<Type> types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(types, nameof(types));

			return selector.InternalAddSelector(selector.InternalTypes.Select(t => new TypeMap(t, types)), Enumerable.Empty<TypeFactoryMap>());
		}

		/// <summary>
		/// Registers each matching concrete type as all of its implemented interfaces.
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector AsImplementedInterfaces(this IServiceTypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AsImplementedInterfaces(true);
		}

		/// <summary>
		/// Registers each matching concrete type as all of its implemented interfaces.
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector AsImplementedInterfaces(this IServiceTypeSelector selector, bool publicOnly)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AsTypeInfo(t => t.ImplementedInterfaces
				.Where(x => x.IsPublic(publicOnly) && x.HasMatchingGenericArity(t))
				.Select(x => x.GetRegistrationType(t)));
		}

		/// <summary>
		/// Registers each matching concrete type as all of its implemented interfaces, by returning an instance of the main type
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector AsSelfWithInterfaces(this IServiceTypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AsSelfWithInterfaces(true);
		}

		/// <summary>
		/// Registers each matching concrete type as all of its implemented interfaces, by returning an instance of the main type
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector AsSelfWithInterfaces(this IServiceTypeSelector selector, bool publicOnly)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalAddSelector(
				selector.InternalTypes
					.Where(t => t.IsNonAbstractClass(false))
					.Select(t =>
					{
						var typeInfo = t.GetTypeInfo();
						var types = typeInfo.ImplementedInterfaces
							.Where(x => x.IsPublic(publicOnly) && x.HasMatchingGenericArity(typeInfo))
							.Select(x => x.GetRegistrationType(typeInfo))
							.Union(new [] { t })
							.ToArray();
						return new TypeMap(t, types);
					}),
				Enumerable.Empty<TypeFactoryMap>());
		}

		/// <summary>
		/// Registers each matching concrete type as all of its implemented interfaces, by returning an instance of the main type
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector AsSelfFactoryWithInterfaces(this IServiceTypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AsSelfFactoryWithInterfaces(true);
		}

		/// <summary>
		/// Registers each matching concrete type as all of its implemented interfaces, by returning an instance of the main type
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector AsSelfFactoryWithInterfaces(this IServiceTypeSelector selector, bool publicOnly)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalAddSelector(
				selector.InternalTypes
					.Where(t => t.IsNonAbstractClass(false))
					.Select(t => new TypeMap(t, new[] { t })),
				selector.InternalTypes
					.Where(t => t.IsNonAbstractClass(false))
					.Select(t =>
					{
						// Query types
						var typeInfo = t.GetTypeInfo();
						var types = typeInfo.ImplementedInterfaces
							.Where(x => x.IsPublic(publicOnly) && x.HasMatchingGenericArity(typeInfo))
							.Select(x => x.GetRegistrationType(typeInfo))
							.ToArray();
						return new TypeMap(t, types);
					})
					.Where(t => t.ServiceTypes.Any())
					.Select(t =>
					{
						// Build constructor expression
						var providerExpr = Expression.Parameter(typeof(IServiceProvider));
						var serviceTypeExpr = Expression.Constant(t.ImplementationType, typeof(Type));
						var callExpr = Expression.Call(providerExpr, serviceProviderGetServiceMethodInfo, serviceTypeExpr);
						var lambdaExpr = Expression.Lambda<Func<IServiceProvider, object>>(callExpr, providerExpr);

						return new TypeFactoryMap(lambdaExpr.Compile(), t.ServiceTypes);
					}));
		}

		/// <summary>
		/// Registers the type with the first found matching interface name.  (e.g. ClassName is matched to IClassName)
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector AsMatchingInterface(this IServiceTypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AsMatchingInterface(null);
		}

		/// <summary>
		/// Registers the type with the first found matching interface name.  (e.g. ClassName is matched to IClassName)
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector AsMatchingInterface(this IServiceTypeSelector selector, Action<TypeInfo, IImplementationTypeFilter> action)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AsTypeInfo(t => t.FindMatchingInterface(action));
		}

		/// <summary>
		/// Registers each matching concrete type as each of the types returned
		/// from the <paramref name="query"/> function.
		/// </summary>
		[PublicAPI]
		public static ILifetimeSelector As(this IServiceTypeSelector selector, Func<Type, IEnumerable<Type>> query)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(query, nameof(query));

			return selector.InternalAddSelector(selector.InternalTypes.Select(t => new TypeMap(t, query(t))), Enumerable.Empty<TypeFactoryMap>());
		}

		/// <summary>
		/// Registers each matching concrete type according to their <see cref="ServiceDescriptorAttribute"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector UsingAttributes(this IServiceTypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalUsingAttributes();
		}

		[PublicAPI]
		public static IServiceTypeSelector UsingRegistrationStrategy(this IServiceTypeSelector selector, RegistrationStrategy registrationStrategy)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(registrationStrategy, nameof(registrationStrategy));

			return selector.InternalUsingRegistrationStrategy(registrationStrategy);
		}

		private static ILifetimeSelector AsTypeInfo(this IServiceTypeSelector selector, Func<TypeInfo, IEnumerable<Type>> query)
		{
			return selector.As(t => query(t.GetTypeInfo()));
		}
	}
}