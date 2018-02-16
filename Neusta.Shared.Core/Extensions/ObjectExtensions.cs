// ReSharper disable once CheckNamespace

namespace System
{
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class ObjectExtensions
	{
		/// <summary>
		/// Determines whether the reference is reference equal (using object.ReferenceEquals) to the specified reference.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsReferenceEqual(this object value, object compareTo)
		{
			return ReferenceEquals(value, compareTo);
		}

		/// <summary>
		/// Determines whether the reference is null.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[ContractAnnotation("value:null => true")]
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNull(this object value)
		{
			return ReferenceEquals(value, null);
		}

		/// <summary>
		/// Determines whether the reference is not null.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[ContractAnnotation("value:null => false")]
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsNotNull(this object value)
		{
			return !ReferenceEquals(value, null);
		}

		/// <summary>
		/// Throws an <see cref="ArgumentNullException"/> if the instance is null.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T ThrowIfNull<T>(this T source)
			where T : class
		{
			if (ReferenceEquals(source, null))
			{
				throw new ArgumentNullException();
			}
			return source;
		}

		/// <summary>
		/// Throws an <see cref="ArgumentNullException"/> if the instance is null.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T ThrowIfNull<T>(this T source, string paramName)
		{
			if (ReferenceEquals(source, null))
			{
				throw new ArgumentNullException(paramName);
			}
			return source;
		}

		/// <summary>
		/// Uses the specified source explicitly
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void UseExplicity<T>(this T source)
		{
		}
	}
}