namespace Neusta.Shared.DataAccess
{
	using System.ComponentModel;
	using System.Diagnostics;
	using JetBrains.Annotations;

	public interface IDataContextOwner
	{
		/// <summary>
		/// Gets the <see cref="IDataContext" />.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[DebuggerHidden]
		IDataContext DataContext { get; }

		/// <summary>
		/// Gets the <see cref="IDataContextExtensions" />.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[DebuggerHidden]
		IDataContextExtensions Extensions { get; }
	}
}