// ReSharper disable once CheckNamespace
namespace System
{
	using JetBrains.Annotations;
	using Neusta.Shared.ObjectProvider.Utils;

	public static class TypeExtensions
	{
		/// <summary>
		/// Gets a friends name of the type.
		/// </summary>
		[PublicAPI]
		public static string ToFriendlyName(this Type type)
		{
			return TypeNameHelper.GetTypeDisplayName(type, includeGenericParameterNames: true);
		}

	}
}