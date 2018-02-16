// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Neusta.Shared.ObjectProvider.Configuration.Helper;

	internal static class TypeInfoExtensions
	{
		internal static bool IsAssignableToGenericTypeDefinition(this TypeInfo typeInfo, TypeInfo genericTypeInfo)
		{
			var interfaceTypes = typeInfo.ImplementedInterfaces.Select(t => t.GetTypeInfo());

			foreach (var interfaceType in interfaceTypes)
			{
				if (interfaceType.IsGenericType)
				{
					var typeDefinitionTypeInfo = interfaceType
						.GetGenericTypeDefinition()
						.GetTypeInfo();

					if (typeDefinitionTypeInfo.Equals(genericTypeInfo))
					{
						return true;
					}
				}
			}

			if (typeInfo.IsGenericType)
			{
				var typeDefinitionTypeInfo = typeInfo
					.GetGenericTypeDefinition()
					.GetTypeInfo();

				if (typeDefinitionTypeInfo.Equals(genericTypeInfo))
				{
					return true;
				}
			}

			var baseTypeInfo = typeInfo.BaseType?.GetTypeInfo();

			if (baseTypeInfo == null)
			{
				return false;
			}

			return baseTypeInfo.IsAssignableToGenericTypeDefinition(genericTypeInfo);
		}

		internal static IEnumerable<Type> FindMatchingInterface(this TypeInfo typeInfo, Action<TypeInfo, IImplementationTypeFilter> action)
		{
			var matchingInterfaceName = $"I{typeInfo.Name}";

			var matchedInterfaces = typeInfo
				.GetImplementedInterfacesToMap()
				.Where(x => string.Equals(x.Name, matchingInterfaceName, StringComparison.Ordinal))
				.ToArray();

			Type type;
			if (action != null)
			{
				var filter = new ImplementationTypeFilter(matchedInterfaces);

				action(typeInfo, filter);

				type = filter.Types.FirstOrDefault();
			}
			else
			{
				type = matchedInterfaces.FirstOrDefault();
			}

			if (type != null)
			{
				yield return type;
			}
		}

		internal static IEnumerable<Type> GetImplementedInterfacesToMap(this TypeInfo typeInfo)
		{
			if (!typeInfo.IsGenericType)
			{
				return typeInfo.ImplementedInterfaces;
			}

			if (!typeInfo.IsGenericTypeDefinition)
			{
				return typeInfo.ImplementedInterfaces;
			}

			return typeInfo.FilterMatchingGenericInterfaces();
		}

		internal static IEnumerable<Type> FilterMatchingGenericInterfaces(this TypeInfo typeInfo)
		{
			var genericTypeParameters = typeInfo.GenericTypeParameters;

			foreach (var current in typeInfo.ImplementedInterfaces)
			{
				var currentTypeInfo = current.GetTypeInfo();

				if (currentTypeInfo.IsGenericType && currentTypeInfo.ContainsGenericParameters
				                                  && genericTypeParameters.GenericParametersMatch(currentTypeInfo.GenericTypeArguments))
				{
					yield return currentTypeInfo.GetGenericTypeDefinition();
				}
			}
		}
	}
}