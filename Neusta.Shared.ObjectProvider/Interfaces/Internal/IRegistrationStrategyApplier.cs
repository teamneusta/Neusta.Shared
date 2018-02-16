namespace Neusta.Shared.ObjectProvider.Internal
{
	public interface IRegistrationStrategyApplier
	{
		/// <summary>
		/// Applies the strategy for the specified service descriptors.
		/// </summary>
		void Apply(IServiceDescriptor descriptor, RegistrationStrategy strategy);
	}
}