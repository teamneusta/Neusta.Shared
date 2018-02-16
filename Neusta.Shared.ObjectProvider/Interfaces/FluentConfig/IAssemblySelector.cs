namespace Neusta.Shared.ObjectProvider
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Reflection;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IAssemblySelector : IFluentSyntax
	{
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		Assembly InternalGetApplicationAssembly();

		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IImplementationTypeSelector InternalFromAssemblies(IEnumerable<Assembly> assemblies);

		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IImplementationTypeSelector InternalFromAssembliesOf(IEnumerable<TypeInfo> typeInfos);
	}
}