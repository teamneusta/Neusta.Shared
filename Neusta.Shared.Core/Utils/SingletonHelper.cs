namespace Neusta.Shared.Core.Utils
{
	using System;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;

	public static class SingletonHelper
	{
		private static readonly TypeInfo ISingletonTypeInfo = typeof(ISingleton).GetTypeInfo();

		/// <summary>
		/// Gets the singleton instance property information.
		/// </summary>
		[PublicAPI]
		public static PropertyInfo GetSingletonInstancePropertyInfo(Type singletonType)
		{
			Guard.ArgumentNotNull(singletonType, nameof(singletonType));

			// ISingleton/Singleton<T> implementation
			TypeInfo singletonTypeInfo = singletonType.GetTypeInfo();
			if (ISingletonTypeInfo.IsAssignableFrom(singletonTypeInfo))
			{
				var implType = typeof(Singleton<>).MakeGenericType(singletonType);
				if (implType.GetTypeInfo().IsAssignableFrom(singletonTypeInfo))
				{
					return implType.GetRuntimeProperty(@"Instance");
				}
			}

			// Singleton attribute
			SingletonAttribute singletonAttr = singletonTypeInfo.GetCustomAttribute<SingletonAttribute>(true);
			if (singletonAttr != null)
			{
				string propertyName = singletonAttr.PropertyName;
				Type baseType = singletonType;
				while (baseType != null)
				{
					TypeInfo baseTypeInfo = baseType.GetTypeInfo();
					if (baseTypeInfo.GetCustomAttribute<SingletonAttribute>(false) != null)
					{
						PropertyInfo propertyInfo = null;
						if (string.IsNullOrEmpty(propertyName))
						{
							propertyInfo = baseTypeInfo.DeclaredProperties.SingleOrDefault(x => x.GetCustomAttribute<SingletonInstancePropertyAttribute>(false) != null);
						}
						else
						{
							propertyInfo = baseType.GetRuntimeProperty(propertyName);
						}
						if (propertyInfo != null)
						{
							return propertyInfo;
						}
					}
					baseType = baseTypeInfo.BaseType;
				}
			}
			else
			{
				// Instance property
				Type baseType = singletonType;
				while (baseType != null)
				{
					PropertyInfo propertyInfo = baseType.GetRuntimeProperty(@"Instance");
					if (propertyInfo != null)
					{
						return propertyInfo;
					}
					baseType = baseType.GetTypeInfo().BaseType;
				}
			}

			// No idea....
			return null;
		}


		/// <summary>
		/// Sets the singleton value.
		/// </summary>
		[PublicAPI]
		public static object GetSingletonValue(Type singletonType)
		{
			PropertyInfo propInfo = GetSingletonInstancePropertyInfo(singletonType);
			if (propInfo == null)
			{
				throw new InvalidOperationException(string.Format(@"Singleton instance property not found on '{0}'", singletonType.Name));
			}
			return propInfo.GetValue(null, null);
		}

		/// <summary>
		/// Sets the singleton value.
		/// Only use this helper class in unit tests!
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void SetSingletonValue(Type singletonType, ISingleton value)
		{
			PropertyInfo propInfo = GetSingletonInstancePropertyInfo(singletonType);
			if (propInfo == null)
			{
				throw new InvalidOperationException(string.Format(@"Singleton instance property not found on '{0}'", singletonType.Name));
			}
			MethodInfo methodInfo = propInfo.SetMethod;
			if (methodInfo == null)
			{
				throw new InvalidOperationException(
					string.Format(@"Private setter method not found for property 'Instance' on '{0}'", singletonType.Name));
			}
			methodInfo.Invoke(null, new object[] {value});
		}

		/// <summary>
		/// Sets the singleton value.
		/// Only use this helper class in unit tests!
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static void SetSingletonValue<T>(T value)
			where T : Singleton<T>, ISingleton
		{
			SetSingletonValue(typeof(Singleton<T>), value);
		}
	}
}