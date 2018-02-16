// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	// ReSharper disable once InconsistentNaming
	public static class IImplementationTypeFilterExtensions
	{
		/// <summary>
		/// Will match all types that are assignable to <typeparamref name="T"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter AssignableTo<T>(this IImplementationTypeFilter selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AssignableTo(typeof(T));
		}

		/// <summary>
		/// Will match all types that are assignable to the specified <paramref name="type" />.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter AssignableTo(this IImplementationTypeFilter selector, Type type)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(type, nameof(type));

			return selector.AssignableToAny(type);
		}

		/// <summary>
		/// Will match all types that are assignable to any of the specified <paramref name="types" />.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter AssignableToAny(this IImplementationTypeFilter selector, params Type[] types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(types, nameof(types));

			return selector.AssignableToAny(types.AsEnumerable());
		}

		/// <summary>
		/// Will match all types that are assignable to any of the specified <paramref name="types" />.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter AssignableToAny(this IImplementationTypeFilter selector, IEnumerable<Type> types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(types, nameof(types));

			return selector.InternalWhere(t => types.Any(t.IsAssignableTo));
		}

		/// <summary>
		/// Will match all types that has an attribute of type <typeparamref name="T"/> defined.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter WithAttribute<T>(this IImplementationTypeFilter selector) where T : Attribute
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.WithAttribute(typeof(T));
		}

		/// <summary>
		/// Will match all types that has an attribute of <paramref name="attributeType" /> defined.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter WithAttribute(this IImplementationTypeFilter selector, Type attributeType)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(attributeType, nameof(attributeType));

			return selector.InternalWhere(t => t.HasAttribute(attributeType));
		}

		/// <summary>
		/// Will match all types that has an attribute of type <typeparamref name="T"/> defined,
		/// and where the attribute itself matches the <paramref name="predicate"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter WithAttribute<T>(this IImplementationTypeFilter selector, Func<T, bool> predicate)
			where T : Attribute
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(predicate, nameof(predicate));

			return selector.InternalWhere(t => t.HasAttribute(predicate));
		}

		/// <summary>
		/// Will match all types that doesn't have an attribute of type <typeparamref name="T"/> defined.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter WithoutAttribute<T>(this IImplementationTypeFilter selector)
			where T : Attribute
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.WithoutAttribute(typeof(T));
		}

		/// <summary>
		/// Will match all types that doesn't have an attribute of <paramref name="attributeType" /> defined.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter WithoutAttribute(this IImplementationTypeFilter selector, Type attributeType)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(attributeType, nameof(attributeType));

			return selector.InternalWhere(t => !t.HasAttribute(attributeType));
		}

		/// <summary>
		/// Will match all types that doesn't have an attribute of type <typeparamref name="T"/> defined,
		/// and where the attribute itself matches the <paramref name="predicate"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter WithoutAttribute<T>(this IImplementationTypeFilter selector, Func<T, bool> predicate)
			where T : Attribute
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(predicate, nameof(predicate));

			return selector.InternalWhere(t => !t.HasAttribute(predicate));
		}

		/// <summary>
		/// Will match all types in the same namespace as the type <typeparamref name="T"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter InNamespaceOf<T>(this IImplementationTypeFilter selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InNamespaceOf(typeof(T));
		}

		/// <summary>
		/// Will match all types in any of the namespaces of the <paramref name="types" /> specified.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter InNamespaceOf(this IImplementationTypeFilter selector, params Type[] types)
		{
			Guard.ArgumentNotNull(types, nameof(types));

			return selector.InNamespaces(types.Select(t => t.Namespace));
		}

		/// <summary>
		/// Will match all types in any of the <paramref name="namespaces"/> specified.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter InNamespaces(this IImplementationTypeFilter selector, params string[] namespaces)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(namespaces, nameof(namespaces));

			return selector.InNamespaces(namespaces.AsEnumerable());
		}

		/// <summary>
		/// Will match all types in the exact same namespace as the type <typeparamref name="T"/>
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter InExactNamespaceOf<T>(this IImplementationTypeFilter selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InExactNamespaceOf(typeof(T));
		}

		/// <summary>
		/// Will match all types in the exact same namespace as the type <paramref name="types"/>
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter InExactNamespaceOf(this IImplementationTypeFilter selector, params Type[] types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(types, nameof(types));

			return selector.InternalWhere(t => types.Any(x => t.IsInExactNamespace(x.Namespace)));
		}

		/// <summary>
		/// Will match all types in the exact same namespace as the type <paramref name="namespaces"/>
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter InExactNamespaces(this IImplementationTypeFilter selector, params string[] namespaces)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(namespaces, nameof(namespaces));

			return selector.InternalWhere(t => namespaces.Any(t.IsInExactNamespace));
		}

		/// <summary>
		/// Will match all types in any of the <paramref name="namespaces"/> specified.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter InNamespaces(this IImplementationTypeFilter selector, IEnumerable<string> namespaces)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(namespaces, nameof(namespaces));

			return selector.InternalWhere(t => namespaces.Any(t.IsInNamespace));
		}

		/// <summary>
		/// Will match all types outside of the same namespace as the type <typeparamref name="T"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter NotInNamespaceOf<T>(this IImplementationTypeFilter selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.NotInNamespaceOf(typeof(T));
		}

		/// <summary>
		/// Will match all types outside of all of the namespaces of the <paramref name="types" /> specified.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter NotInNamespaceOf(this IImplementationTypeFilter selector, params Type[] types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(types, nameof(types));

			return selector.NotInNamespaces(types.Select(t => t.Namespace));
		}

		/// <summary>
		/// Will match all types outside of all of the <paramref name="namespaces"/> specified.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter NotInNamespaces(this IImplementationTypeFilter selector, params string[] namespaces)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(namespaces, nameof(namespaces));

			return selector.NotInNamespaces(namespaces.AsEnumerable());
		}

		/// <summary>
		/// Will match all types outside of all of the <paramref name="namespaces"/> specified.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter NotInNamespaces(this IImplementationTypeFilter selector, IEnumerable<string> namespaces)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(namespaces, nameof(namespaces));

			return selector.InternalWhere(t => namespaces.All(ns => !t.IsInNamespace(ns)));
		}

		/// <summary>
		/// Will match types based on the specified <paramref name="predicate"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeFilter Where(this IImplementationTypeFilter selector, Func<Type, bool> predicate)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(predicate, nameof(predicate));

			return selector.Where(predicate);
		}
	}
}