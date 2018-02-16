namespace Neusta.Shared.ObjectProvider
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public enum RegistrationStrategy
	{
		/// <summary>
		/// Appends a new registration for existing services.
		/// </summary>
		Append = 0,

		/// <summary>
		/// Skips registrations for services that already exists.
		/// </summary>
		Skip = 1,

		/// <summary>
		/// Replaces existing service registrations.
		/// </summary>
		Replace = ReplaceByServiceType,

		/// <summary>
		/// Replaces existing service registrations by service type (default).
		/// </summary>
		ReplaceByServiceType = 3,

		/// <summary>
		/// Replaces existing service registrations by implementation type.
		/// </summary>
		ReplaceByImplementationType = 4,

		/// <summary>
		/// Replaces existing service registrations by either service- or implementation type.
		/// </summary>
		ReplaceByServiceAndImplementationType = 5,
	}
}