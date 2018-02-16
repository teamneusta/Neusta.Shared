// ReSharper disable once CheckNamespace

namespace System
{
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Reflection;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class TypeExtensions
	{
		// ReSharper disable InconsistentNaming
		private static readonly TypeInfo IListTypeInfo = typeof(IList).GetTypeInfo();
		private static readonly TypeInfo IListGenericTypeInfo = typeof(IList<>).GetTypeInfo();
		private static readonly TypeInfo ICollectionTypeInfo = typeof(ICollection).GetTypeInfo();

		private static readonly TypeInfo ICollectionGenericTypeInfo = typeof(ICollection<>).GetTypeInfo();
		// ReSharper restore InconsistentNaming

		/// <summary>
		/// Determines whether the specified type is a list type.
		/// </summary>
		[PublicAPI]
		public static bool IsList(this Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			if (IListTypeInfo.IsAssignableFrom(typeInfo))
			{
				return true;
			}
			if (typeInfo.IsGenericType)
			{
				TypeInfo genericTypeDefinition = typeInfo.GetGenericTypeDefinition().GetTypeInfo();
				if (IListGenericTypeInfo.IsAssignableFrom(genericTypeDefinition))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified type is a collction type.
		/// </summary>
		[PublicAPI]
		public static bool IsCollection(this Type type)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			if (ICollectionTypeInfo.IsAssignableFrom(typeInfo))
			{
				return true;
			}
			if (typeInfo.IsGenericType)
			{
				TypeInfo genericTypeDefinition = typeInfo.GetGenericTypeDefinition().GetTypeInfo();
				if (ICollectionGenericTypeInfo.IsAssignableFrom(genericTypeDefinition))
				{
					return true;
				}
			}
			return false;
		}
	}
}