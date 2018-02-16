namespace Neusta.Shared.ObjectProvider.Utils
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Text;
	using JetBrains.Annotations;

	internal static class TypeNameHelper
	{
		private static readonly Dictionary<Type, string> builtInTypeNames = new Dictionary<Type, string>
		{
			{ typeof(void), "void" },
			{ typeof(bool), "bool" },
			{ typeof(byte), "byte" },
			{ typeof(char), "char" },
			{ typeof(decimal), "decimal" },
			{ typeof(double), "double" },
			{ typeof(float), "float" },
			{ typeof(int), "int" },
			{ typeof(long), "long" },
			{ typeof(object), "object" },
			{ typeof(sbyte), "sbyte" },
			{ typeof(short), "short" },
			{ typeof(string), "string" },
			{ typeof(uint), "uint" },
			{ typeof(ulong), "ulong" },
			{ typeof(ushort), "ushort" }
		};

		/// <summary>
		/// Pretty print a type name.
		/// </summary>
		[PublicAPI]
		public static string GetTypeDisplayName(object item, bool fullName = true)
		{
			return item == null ? null : GetTypeDisplayName(item.GetType(), fullName);
		}

		/// <summary>
		/// Pretty print a type name.
		/// </summary>
		[PublicAPI]
		public static string GetTypeDisplayName(Type type, bool fullName = true, bool includeGenericParameterNames = false)
		{
			var builder = new StringBuilder();
			ProcessType(builder, type, new DisplayNameOptions(fullName, includeGenericParameterNames));
			return builder.ToString();
		}

		#region Private Methods

		private static void ProcessType(StringBuilder builder, Type type, DisplayNameOptions options)
		{
			var typeInfo = type.GetTypeInfo();

			if (typeInfo.IsGenericType)
			{
				var genericArguments = typeInfo.GenericTypeArguments;
				ProcessGenericType(builder, type, genericArguments, genericArguments.Length, options);
			}
			else if (type.IsArray)
			{
				ProcessArrayType(builder, type, options);
			}
			else if (builtInTypeNames.TryGetValue(type, out var builtInName))
			{
				builder.Append(builtInName);
			}
			else if (type.IsGenericParameter)
			{
				if (options.IncludeGenericParameterNames)
				{
					builder.Append(type.Name);
				}
			}
			else
			{
				builder.Append(options.FullName ? type.FullName : type.Name);
			}
		}

		private static void ProcessArrayType(StringBuilder builder, Type type, DisplayNameOptions options)
		{
			var innerType = type;
			while (innerType.IsArray)
			{
				innerType = innerType.GetElementType();
			}

			ProcessType(builder, innerType, options);

			while (type.IsArray)
			{
				builder.Append('[');
				builder.Append(',', type.GetArrayRank() - 1);
				builder.Append(']');
				type = type.GetElementType();
			}
		}

		private static void ProcessGenericType(StringBuilder builder, Type type, Type[] genericArguments, int length, DisplayNameOptions options)
		{
			var offset = 0;
			if (type.IsNested)
			{
				offset = type.DeclaringType.GetTypeInfo().GenericTypeArguments.Length;
			}

			if (options.FullName)
			{
				if (type.IsNested)
				{
					ProcessGenericType(builder, type.DeclaringType, genericArguments, offset, options);
					builder.Append('+');
				}
				else if (!string.IsNullOrEmpty(type.Namespace))
				{
					builder.Append(type.Namespace);
					builder.Append('.');
				}
			}

			var genericPartIndex = type.Name.IndexOf('`');
			if (genericPartIndex <= 0)
			{
				builder.Append(type.Name);
				return;
			}

			builder.Append(type.Name, 0, genericPartIndex);

			builder.Append('<');
			for (var i = offset; i < length; i++)
			{
				ProcessType(builder, genericArguments[i], options);
				if (i + 1 == length)
				{
					continue;
				}

				builder.Append(',');
				if (options.IncludeGenericParameterNames || !genericArguments[i + 1].IsGenericParameter)
				{
					builder.Append(' ');
				}
			}
			builder.Append('>');
		}

		private struct DisplayNameOptions
		{
			public DisplayNameOptions(bool fullName, bool includeGenericParameterNames)
			{
				this.FullName = fullName;
				this.IncludeGenericParameterNames = includeGenericParameterNames;
			}

			public bool FullName { get; }

			public bool IncludeGenericParameterNames { get; }
		}

		#endregion
	}
}