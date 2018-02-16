namespace Neusta.Shared.Logging
{
	using System;
	using System.ComponentModel;
	using JetBrains.Annotations;

	/// <summary>
	/// A hack to hide methods defined on <see cref="object"/> for IntelliSense
	/// on fluent interfaces. Credit to Daniel Cazzulino.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IFluentSyntax
	{
		/// <summary>
		/// Gets the type.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		Type GetType();

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		int GetHashCode();

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		string ToString();

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		bool Equals(object other);
	}
}