namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IImplementationTypeFilter : IFluentSyntax
	{
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IImplementationTypeFilter InternalWhere(Func<Type, bool> predicate);
	}
}