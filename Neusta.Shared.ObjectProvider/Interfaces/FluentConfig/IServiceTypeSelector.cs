namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Neusta.Shared.ObjectProvider.Configuration.Helper;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IServiceTypeSelector : IImplementationTypeSelector, IFluentSyntax
	{
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		IEnumerable<Type> InternalTypes { get; }

		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		ILifetimeSelector InternalAddSelector(IEnumerable<TypeMap> types, IEnumerable<TypeFactoryMap> factories);

		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IImplementationTypeSelector InternalUsingAttributes();

		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Never)]
		IServiceTypeSelector InternalUsingRegistrationStrategy(RegistrationStrategy registrationStrategy);
	}
}