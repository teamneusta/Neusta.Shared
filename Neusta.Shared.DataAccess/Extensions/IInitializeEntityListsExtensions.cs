// ReSharper disable once CheckNamespace

namespace Neusta.Shared.DataAccess
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	// ReSharper disable once InconsistentNaming
	public static class IInitializeEntityListsExtensions
	{
		/// <summary>
		/// Returns a new instance of T if the given instance is null and returns the instance.
		/// </summary>
		[PublicAPI]
		public static ICollection<T> InitializeIfNull<T>(this IInitializeEntityLists entity, ICollection<T> instance)
			where T : class, new()
		{
			return instance ?? new List<T>();
		}

		/// <summary>
		/// Returns a new instance of T if the given instance is null and returns the instance.
		/// </summary>
		[PublicAPI]
		public static T InitializeIfNull<T>(this IInitializeEntityLists entity, T instance)
			where T : class, new()
		{
			return instance ?? new T();
		}

		/// <summary>
		/// Invokes the callback function if the given instance is null and returns the instance.
		/// </summary>
		[PublicAPI]
		public static T InitializeIfNull<T>(this IInitializeEntityLists entity, T instance, Func<T> callback)
		{
			if (instance == null)
			{
				instance = callback();
			}
			return instance;
		}
	}
}