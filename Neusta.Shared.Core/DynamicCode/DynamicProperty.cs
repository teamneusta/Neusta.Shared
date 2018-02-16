namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.Collections.Concurrent;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DynamicCode.Safe;

	/// <summary>
	/// Factory class for dynamic properties.
	/// </summary>
	public static class DynamicProperty
	{
		/// <summary>
		/// Cache for dynamic property types.
		/// </summary>
		private static readonly ConcurrentDictionary<PropertyInfo, IDynamicProperty> propertyCache = new ConcurrentDictionary<PropertyInfo, IDynamicProperty>();

		/// <summary>
		/// Creates dynamic property instance for the specified <see cref="PropertyInfo"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicProperty For(PropertyInfo propertyInfo)
		{
			return propertyCache.GetOrAdd(propertyInfo, callback =>
				Activator.CreateInstance(
					typeof(SafeProperty<,>).MakeGenericType(callback.DeclaringType, callback.PropertyType),
					callback
				) as IDynamicProperty);
		}

		/// <summary>
		/// Creates dynamic property instance for the specified <see cref="PropertyInfo"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicProperty<TType, TValue> For<TType, TValue>(PropertyInfo propertyInfo)
		{
			if (typeof(TType) != propertyInfo.DeclaringType)
			{
				throw new ArgumentException("Generic type should match declaring type.");
			}
			return propertyCache.GetOrAdd(propertyInfo, callback => new SafeProperty<TType, TValue>(callback)) as IDynamicProperty<TType, TValue>;
		}
	}
}