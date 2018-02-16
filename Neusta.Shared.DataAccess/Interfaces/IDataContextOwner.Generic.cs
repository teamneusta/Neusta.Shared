namespace Neusta.Shared.DataAccess
{
	using System.ComponentModel;
	using System.Diagnostics;
	using JetBrains.Annotations;

	public interface IDataContextOwner<TDataContext> : IDataContextOwner
		where TDataContext : IDataContext
	{
		/// <summary>
		/// Gets the <see cref="IDataContext" />.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[DebuggerHidden]
		new TDataContext DataContext { get; }

	}
}