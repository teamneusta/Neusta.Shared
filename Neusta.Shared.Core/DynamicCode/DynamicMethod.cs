namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.Collections.Concurrent;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DynamicCode.Safe;

	/// <summary>
	/// Factory class for dynamic methods.
	/// </summary>
	public static class DynamicMethod
	{
		/// <summary>
		/// Cache for dynamic method types.
		/// </summary>
		private static readonly ConcurrentDictionary<MethodInfo, IDynamicMethod> methodCache = new ConcurrentDictionary<MethodInfo, IDynamicMethod>();

		/// <summary>
		/// Creates dynamic method instance for the specified <see cref="MethodInfo"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicMethod For(MethodInfo methodInfo)
		{
			return methodCache.GetOrAdd(methodInfo, callback =>
				Activator.CreateInstance(
					typeof(SafeMethod<>).MakeGenericType(callback.DeclaringType),
					callback
				) as IDynamicMethod);
		}

		/// <summary>
		/// Creates dynamic method instance for the specified <see cref="MethodInfo"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicMethod<TType> For<TType>(MethodInfo methodInfo)
		{
			if (typeof(TType) != methodInfo.DeclaringType)
			{
				throw new ArgumentException("Generic type should match declaring type.");
			}
			return methodCache.GetOrAdd(methodInfo, callback => new SafeMethod<TType>(callback)) as IDynamicMethod<TType>;
		}
	}
}