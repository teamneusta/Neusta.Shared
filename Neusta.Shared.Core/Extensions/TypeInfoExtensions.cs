// ReSharper disable once CheckNamespace

namespace System
{
	using System.ComponentModel;
	using System.Reflection;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class TypeInfoExtensions
	{
		/// <summary>
		/// Gets the <see cref="TypeInfo"/>.
		/// </summary>
		[PublicAPI]
		public static TypeCode GetTypeCode(this TypeInfo typeInfo)
		{
			if (typeInfo.IsNull())
			{
				return TypeCode.Empty;
			}
			Type type = typeInfo.GetType();
			if (typeInfo.IsValueType)
			{
				if (type == typeof(bool))
				{
					return TypeCode.Boolean;
				}
				if (type == typeof(int))
				{
					return TypeCode.Int32;
				}
				if (type == typeof(uint))
				{
					return TypeCode.UInt32;
				}
				if (type == typeof(short))
				{
					return TypeCode.Int16;
				}
				if (type == typeof(ushort))
				{
					return TypeCode.UInt16;
				}
				if (type == typeof(long))
				{
					return TypeCode.Int64;
				}
				if (type == typeof(ulong))
				{
					return TypeCode.UInt64;
				}
				if (type == typeof(byte))
				{
					return TypeCode.Byte;
				}
				if (type == typeof(sbyte))
				{
					return TypeCode.SByte;
				}
				if (type == typeof(double))
				{
					return TypeCode.Double;
				}
				if (type == typeof(decimal))
				{
					return TypeCode.Decimal;
				}
				if (type == typeof(float))
				{
					return TypeCode.Single;
				}
				if (type == typeof(char))
				{
					return TypeCode.Char;
				}
				if (type == typeof(DateTime))
				{
					return TypeCode.DateTime;
				}
			}
			else
			{
				if (type == typeof(string))
				{
					return TypeCode.String;
				}
				if (type == typeof(void))
				{
					return TypeCode.Empty;
				}
			}
			return TypeCode.Object;
		}
	}
}