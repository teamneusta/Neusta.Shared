namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.Collections.Concurrent;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DynamicCode.Safe;

	/// <summary>
	/// Factory class for dynamic fields.
	/// </summary>
	public static class DynamicField
	{
		/// <summary>
		/// Cache for dynamic field types.
		/// </summary>
		private static readonly ConcurrentDictionary<FieldInfo, IDynamicField> fieldCache = new ConcurrentDictionary<FieldInfo, IDynamicField>();

		/// <summary>
		/// Creates dynamic field instance for the specified <see cref="FieldInfo"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicField For(FieldInfo fieldInfo)
		{
			return fieldCache.GetOrAdd(fieldInfo, callback =>
				Activator.CreateInstance(
					typeof(SafeField<,>).MakeGenericType(callback.DeclaringType, callback.FieldType),
					callback
				) as IDynamicField);
		}

		/// <summary>
		/// Creates dynamic property instance for the specified <see cref="PropertyInfo"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicField<TType, TValue> For<TType, TValue>(FieldInfo fieldInfo)
		{
			if (typeof(TType) != fieldInfo.DeclaringType)
			{
				throw new ArgumentException("Generic type should match declaring type.");
			}
			return fieldCache.GetOrAdd(fieldInfo, callback => new SafeField<TType, TValue>(callback)) as IDynamicField<TType, TValue>;
		}
	}
}