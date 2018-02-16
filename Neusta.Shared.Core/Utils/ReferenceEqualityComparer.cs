namespace Neusta.Shared.Core.Utils
{
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// Equality comparer for comparing objects by reference reguardless of having the GetHashCode/Equals override implemented on an object.
	/// </summary>
	public sealed class ReferenceEqualityComparer<T> : IEqualityComparer, IEqualityComparer<T>
	{
		private static readonly ReferenceEqualityComparer<T> instance = new ReferenceEqualityComparer<T>();

		/// <summary>
		/// Initializes a new instance of the <see cref="ReferenceEqualityComparer{T}"/> class.
		/// </summary>
		private ReferenceEqualityComparer()
		{
		}

		/// <summary>
		/// Gets the singleton instance of the <see cref="ReferenceEqualityComparer{T}"/> class.
		/// </summary>
		public static ReferenceEqualityComparer<T> Instance
		{
			[DebuggerStepThrough]
			get { return instance; }
		}

		#region Implementation of IEqualityComparer

		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		bool IEqualityComparer.Equals(object x, object y)
		{
			return ReferenceEquals(x, y);
		}

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		int IEqualityComparer.GetHashCode(object obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}

		#endregion

		#region Implementation of IEqualityComparer<T>

		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		public bool Equals(T x, T y)
		{
			return ReferenceEquals(x, y);
		}

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		public int GetHashCode(T obj)
		{
			return RuntimeHelpers.GetHashCode(obj);
		}

		#endregion
	}
}