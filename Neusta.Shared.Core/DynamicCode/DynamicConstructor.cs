namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DynamicCode.Safe;

	/// <summary>
	/// Factory class for dynamic constructors.
	/// </summary>
	public static class DynamicConstructor
	{
		/// <summary>
		/// Cache for dynamic constructor types.
		/// </summary>
		private static readonly ConcurrentDictionary<ConstructorInfo, IDynamicConstructor> ctorCache = new ConcurrentDictionary<ConstructorInfo, IDynamicConstructor>();

		/// <summary>
		/// Creates a dynamic constructor instance for the default constructor of the specified <see cref="Type"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicConstructor<T> For<T>()
			where T : class
		{
			ConstructorInfo constructorInfo = typeof(T).GetTypeInfo()
				.DeclaredConstructors
				.FirstOrDefault(match => match.GetParameters().Length == 0);
			if (constructorInfo == null)
			{
				return null;
			}
			return ctorCache.GetOrAdd(constructorInfo, callback => new SafeConstructor<T>(callback)) as IDynamicConstructor<T>;
		}

		/// <summary>
		/// Creates a dynamic constructor instance for the default constructor of the specified <see cref="Type"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicConstructor<T> For<T>(Type[] parameterTypes)
			where T : class
		{
			ConstructorInfo constructorInfo = typeof(T).GetTypeInfo()
				.DeclaredConstructors
				.FirstOrDefault(match => match.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual(parameterTypes));
			if (constructorInfo == null)
			{
				return null;
			}
			return ctorCache.GetOrAdd(constructorInfo, callback => new SafeConstructor<T>(callback)) as IDynamicConstructor<T>;
		}

		/// <summary>
		/// Creates a dynamic constructor instance for the default constructor of the specified <see cref="Type"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicConstructor For(Type type)
		{
			ConstructorInfo constructorInfo = type.GetTypeInfo()
				.DeclaredConstructors
				.FirstOrDefault(match => match.GetParameters().Length == 0);
			if (constructorInfo == null)
			{
				return null;
			}
			return ctorCache.GetOrAdd(constructorInfo, callback =>
				Activator.CreateInstance(
					typeof(SafeConstructor<>).MakeGenericType(callback.DeclaringType),
					callback
				) as IDynamicConstructor);
		}

		/// <summary>
		/// Creates a dynamic constructor instance for the specified <see cref="ConstructorInfo"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicConstructor For(ConstructorInfo constructorInfo)
		{
			return ctorCache.GetOrAdd(constructorInfo, callback =>
				Activator.CreateInstance(
					typeof(SafeConstructor<>).MakeGenericType(callback.DeclaringType),
					callback
				) as IDynamicConstructor);
		}
	}
}