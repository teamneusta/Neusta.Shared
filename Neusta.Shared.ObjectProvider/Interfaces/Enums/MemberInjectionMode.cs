namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;

	[PublicAPI]
	public enum MemberInjectionMode
	{
		None,
		PrivateFields,
		PropertiesWithLimitedAccess,
		PropertiesWithPublicSetter
	}
}