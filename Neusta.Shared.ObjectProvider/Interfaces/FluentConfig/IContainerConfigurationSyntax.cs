namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.ComponentModel;
	using JetBrains.Annotations;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IContainerConfigurationSyntax : IFluentSyntax
	{
		/// <summary>
		/// Adds registrations using conventions specified using the <paramref name="scan"/>.
		/// </summary>
		[PublicAPI]
		IContainerConfigurationSyntax Scan(Action<ITypeSourceSelector> scan);

		/// <summary>
		/// Adds registrations using conventions specified using the <paramref name="scan"/>.
		/// </summary>
		[PublicAPI]
		IContainerConfigurationSyntax Scan(Action<ITypeSourceSelector> scan, RegistrationStrategy strategy);

		/// <summary>
		/// Unknown types are resolved automatically.
		/// </summary>
		[PublicAPI]
		IContainerConfigurationSyntax AutoResolveUnknownTypes();
	}
}
