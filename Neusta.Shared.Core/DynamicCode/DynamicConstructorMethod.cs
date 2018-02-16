namespace Neusta.Shared.Core.DynamicCode
{
	using System.Collections.Concurrent;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DynamicCode.Safe;

	/// <summary>
	/// Factory class for dynamic methods.
	/// </summary>
	public static class DynamicConstructorMethod
	{
		/// <summary>
		/// Cache for dynamic method types.
		/// </summary>
		private static readonly ConcurrentDictionary<ConstructorInfo, IDynamicMethod> constructorCache = new ConcurrentDictionary<ConstructorInfo, IDynamicMethod>();

		/// <summary>
		/// Creates dynamic method instance for the specified <see cref="ConstructorInfo"/>.
		/// </summary>
		[PublicAPI]
		public static IDynamicMethod For(ConstructorInfo constructorInfo)
		{
			return constructorCache.GetOrAdd(constructorInfo, callback => new SafeConstructorMethod(callback));
		}
	}
}