namespace Neusta.Shared.ObjectProvider
{
	using System.ComponentModel;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ITypeSourceSelector : IAssemblySelector, ITypeSelector, IFluentSyntax
	{
	}
}