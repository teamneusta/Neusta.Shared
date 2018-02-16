namespace Neusta.Shared.Core.DynamicCode
{
	using System.Collections.Concurrent;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DynamicCode.Safe;

	/// <summary>
	/// Factory class for dynamic indexers.
	/// </summary>
	public static class DynamicIndexedProperty
	{
		/// <summary>
		/// Cache for dynamic indexer types.
		/// </summary>
		private static readonly ConcurrentDictionary<PropertyInfo, IDynamicIndexedProperty> indexedPropertyCache = new ConcurrentDictionary<PropertyInfo, IDynamicIndexedProperty>();

		/// <summary>
		/// Creates dynamic property instance for the specified <see cref="PropertyInfo"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicIndexedProperty For(PropertyInfo propertyInfo)
		{
			return indexedPropertyCache.GetOrAdd(propertyInfo, callback => new SafeIndexedProperty(callback));
		}
	}
}