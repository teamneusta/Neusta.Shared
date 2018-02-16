namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ITypeSelector : IFluentSyntax
	{
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IServiceTypeSelector InternalAddTypes(IEnumerable<Type> types);
	}
}