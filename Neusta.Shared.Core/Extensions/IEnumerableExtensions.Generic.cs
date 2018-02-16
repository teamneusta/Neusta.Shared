// ReSharper disable once CheckNamespace

namespace System.Collections.Generic
{
	using System.Collections.ObjectModel;
	using System.ComponentModel;
	using System.Linq;
	using JetBrains.Annotations;

	// ReSharper disable once InconsistentNaming
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Returns the minimum value of an enumerable or the default if the source is empty.
		/// </summary>
		[PublicAPI]
		public static T MinOrDefault<T>(this IEnumerable<T> source, T defaultValue = default(T))
		{
			List<T> sourceList = source.ToList();
			if (sourceList.Any())
			{
				return sourceList.Min();
			}
			return defaultValue;
		}

		/// <summary>
		/// Returns the maximum value of an enumerable or the default if the source is empty.
		/// </summary>
		[PublicAPI]
		public static TResult MaxOrDefault<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector, TResult defaultValue = default(TResult))
		{
			List<T> sourceList = source.ToList();
			if (sourceList.Any())
			{
				return sourceList.Max(selector);
			}
			return defaultValue;
		}

		/// <summary>
		/// Returns the maximum value of an enumerable or the default if the source is empty.
		/// </summary>
		[PublicAPI]
		public static T MaxOrDefault<T>(this IEnumerable<T> source, T defaultValue = default(T))
		{
			List<T> sourceList = source.ToList();
			if (sourceList.Any())
			{
				return sourceList.Max();
			}
			return defaultValue;
		}

		/// <summary>
		/// Returns the minimum value of an enumerable or the default if the source is empty.
		/// </summary>
		[PublicAPI]
		public static TResult MinOrDefault<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector, TResult defaultValue = default(TResult))
		{
			List<T> sourceList = source.ToList();
			if (sourceList.Any())
			{
				return sourceList.Min(selector);
			}
			return defaultValue;
		}

		/// <summary>
		/// Calls the action for each item in the source <see cref="IEnumerable{T}"/>.
		/// </summary>
		[PublicAPI]
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T item in source)
			{
				action(item);
			}
		}

		/// <summary>
		/// Copies the given source <see cref="IEnumerable{T}"/> to a <see cref="ReadOnlyCollection{T}"/>
		/// </summary>
		[PublicAPI]
		public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> source)
		{
			if (source is ReadOnlyCollection<T> roc)
			{
				return roc;
			}
			if (source == null)
			{
				roc = EmptyReadOnlyCollection<T>.Empty;
			}
			else
			{
				roc = new List<T>(source).AsReadOnly();
			}
			return roc;
		}

		#region Private container class for an empty ReadOnlyCollection<T>

		[PublicAPI]
		private class EmptyReadOnlyCollection<T>
		{
			internal static readonly ReadOnlyCollection<T> Empty = new List<T>().AsReadOnly();
		}

		#endregion
	}
}