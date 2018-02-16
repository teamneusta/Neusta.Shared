// ReSharper disable once CheckNamespace
namespace System
{
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class StringExtensions
	{
		/// <summary>
		/// Indicates whether the specified string is null or an empty string ("").
		/// </summary>
		[PublicAPI]
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		/// <summary>
		/// Returns a String.Empty when the value is null.
		/// </summary>
		[PublicAPI]
		public static string EnsureNullAsEmptyString(this string value)
		{
			if (value != null)
			{
				return value;
			}
			return string.Empty;
		}

		/// <summary>
		/// Shrinks the string to the given maximum length.
		/// </summary>
		[PublicAPI]
		public static string MaxLength(this string value, int maxLength)
		{
			if ((value != null) && (value.Length > maxLength))
			{
				value = value.Substring(0, maxLength);
			}
			return value;
		}
	}
}