// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	// ReSharper disable once InconsistentNaming
	public static class ITypeSelectorExtensions
	{
		/// <summary>
		/// Will scan the given type.
		/// </summary>
		[PublicAPI]
		public static IServiceTypeSelector AddType<T>(this ITypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AddTypes(typeof(T));
		}

		/// <summary>
		/// Will scan the given types.
		/// </summary>
		[PublicAPI]
		public static IServiceTypeSelector AddTypes<T1, T2>(this ITypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AddTypes(typeof(T1), typeof(T2));
		}

		/// <summary>
		/// Will scan the given types.
		/// </summary>
		[PublicAPI]
		public static IServiceTypeSelector AddTypes<T1, T2, T3>(this ITypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AddTypes(typeof(T1), typeof(T2), typeof(T3));
		}

		/// <summary>
		/// Will scan the given types.
		/// </summary>
		[PublicAPI]
		public static IServiceTypeSelector AddTypes<T1, T2, T3, T4>(this ITypeSelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.AddTypes(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		}

		/// <summary>
		/// Will scan the types <see cref="Type"/> in <paramref name="types"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceTypeSelector AddTypes(this ITypeSelector selector, params Type[] types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.InternalAddTypes(types);
		}

		/// <summary>
		/// Will scan the types <see cref="Type"/> in <paramref name="types"/>.
		/// </summary>
		[PublicAPI]
		public static IServiceTypeSelector AddTypes(this ITypeSelector selector, IEnumerable<Type> types)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(types, nameof(types));

			return selector.InternalAddTypes(types);
		}

	}
}