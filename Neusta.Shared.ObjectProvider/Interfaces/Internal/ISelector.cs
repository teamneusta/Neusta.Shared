namespace Neusta.Shared.ObjectProvider.Internal
{
	using System.ComponentModel;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ISelector
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		void Populate(IRegistrationStrategyApplier strategyApplier, RegistrationStrategy strategy);
	}
}