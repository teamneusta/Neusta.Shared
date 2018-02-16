// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.CompilerServices;

	internal static class TypeExtensions
	{
		internal static bool IsPublic(this Type type, bool publicOnly)
		{
			if (publicOnly)
			{
				var typeInfo = type.GetTypeInfo();
				return typeInfo.IsPublic || typeInfo.IsNestedPublic;
			}
			return true;
		}

		internal static bool IsNonAbstractClass(this Type type, bool publicOnly)
		{
			var typeInfo = type.GetTypeInfo();

			if (typeInfo.IsClass && !typeInfo.IsAbstract)
			{
				if (typeInfo.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true))
				{
					return false;
				}

				if (publicOnly)
				{
					return typeInfo.IsPublic || typeInfo.IsNestedPublic;
				}

				return true;
			}

			return false;
		}

		internal static IEnumerable<Type> GetBaseTypes(this Type type)
		{
			var typeInfo = type.GetTypeInfo();

			foreach (var implementedInterface in typeInfo.ImplementedInterfaces)
			{
				yield return implementedInterface;
			}

			var baseType = typeInfo.BaseType;

			while (baseType != null)
			{
				var baseTypeInfo = baseType.GetTypeInfo();

				yield return baseType;

				baseType = baseTypeInfo.BaseType;
			}
		}

		internal static bool IsInNamespace(this Type type, string @namespace)
		{
			var typeNamespace = type.Namespace ?? string.Empty;

			if (@namespace.Length > typeNamespace.Length)
			{
				return false;
			}

			var typeSubNamespace = typeNamespace.Substring(0, @namespace.Length);

			if (typeSubNamespace.Equals(@namespace, StringComparison.Ordinal))
			{
				if (typeNamespace.Length == @namespace.Length)
				{
					//exactly the same
					return true;
				}

				//is a subnamespace?
				return typeNamespace[@namespace.Length] == '.';
			}

			return false;
		}

		internal static bool IsInExactNamespace(this Type type, string @namespace)
		{
			return string.Equals(type.Namespace, @namespace, StringComparison.Ordinal);
		}

		internal static bool HasAttribute(this Type type, Type attributeType)
		{
			return type.GetTypeInfo().IsDefined(attributeType, inherit: true);
		}

		internal static bool HasAttribute<T>(this Type type, Func<T, bool> predicate) where T : Attribute
		{
			return type.GetTypeInfo().GetCustomAttributes<T>(inherit: true).Any(predicate);
		}

		internal static bool IsAssignableTo(this Type type, Type otherType)
		{
			var typeInfo = type.GetTypeInfo();
			var otherTypeInfo = otherType.GetTypeInfo();

			if (otherTypeInfo.IsGenericTypeDefinition)
			{
				return typeInfo.IsAssignableToGenericTypeDefinition(otherTypeInfo);
			}

			return otherTypeInfo.IsAssignableFrom(typeInfo);
		}

		internal static bool GenericParametersMatch(this Type[] parameters, Type[] interfaceArguments)
		{
			if (parameters.Length != interfaceArguments.Length)
			{
				return false;
			}

			for (var idx = 0; idx < parameters.Length; idx++)
			{
				if (parameters[idx] != interfaceArguments[idx])
				{
					return false;
				}
			}

			return true;
		}

		internal static bool HasMatchingGenericArity(this Type interfaceType, TypeInfo typeInfo)
		{
			if (typeInfo.IsGenericType)
			{
				var interfaceTypeInfo = interfaceType.GetTypeInfo();

				if (interfaceTypeInfo.IsGenericType)
				{
					var argumentCount = interfaceType.GenericTypeArguments.Length;
					var parameterCount = typeInfo.GenericTypeParameters.Length;

					return argumentCount == parameterCount;
				}

				return false;
			}

			return true;
		}

		internal static Type GetRegistrationType(this Type interfaceType, TypeInfo typeInfo)
		{
			if (typeInfo.IsGenericTypeDefinition)
			{
				var interfaceTypeInfo = interfaceType.GetTypeInfo();

				if (interfaceTypeInfo.IsGenericType)
				{
					return interfaceType.GetGenericTypeDefinition();
				}
			}

			return interfaceType;
		}
	}
}