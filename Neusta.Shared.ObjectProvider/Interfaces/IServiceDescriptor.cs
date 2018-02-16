namespace Neusta.Shared.ObjectProvider
{
	using System;
	using JetBrains.Annotations;

	public interface IServiceDescriptor
	{
		/// <summary>
		/// Gets the service type.
		/// </summary>
		[PublicAPI]
		Type ServiceType { get; }

		/// <summary>
		/// Gets the resolved implementation source.
		/// </summary>
		[PublicAPI]
		ImplementationSource ImplementationSource { get; }

		/// <summary>
		/// Gets or sets the implementation type.
		/// </summary>
		[PublicAPI]
		Type ImplementationType { get; }

		/// <summary>
		/// Gets or sets the implementation instance.
		/// </summary>>
		[PublicAPI]
		object ImplementationInstance { get; }

		/// <summary>
		/// Gets or sets the implementation factory.
		/// </summary>
		[PublicAPI]
		Func<object> ImplementationFactory { get; }

		/// <summary>
		/// Gets or sets the implementation factory.
		/// </summary>
		[PublicAPI]
		Func<IServiceProvider, object> ImplementationFactoryWithProvider { get; }

		/// <summary>
		/// Gets or sets the service lifetime.
		/// </summary>
		[PublicAPI]
		ServiceLifetime ServiceLifetime { get; }

		/// <summary>
		/// Gets or sets the member injection mode.
		/// </summary>
		[PublicAPI]
		MemberInjectionMode MemberInjectionMode { get; }

		/// <summary>
		/// Gets or sets a value indicating whether disposal tracking is disabled.
		/// </summary>
		[PublicAPI]
		bool DisableDisposalTracking { get; }
	}
}