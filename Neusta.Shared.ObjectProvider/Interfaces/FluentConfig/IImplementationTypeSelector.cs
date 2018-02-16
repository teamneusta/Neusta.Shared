namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IImplementationTypeSelector : IAssemblySelector
	{
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IServiceTypeSelector InternalAddClasses(Action<IImplementationTypeFilter> action, bool publicOnly);
	}
}