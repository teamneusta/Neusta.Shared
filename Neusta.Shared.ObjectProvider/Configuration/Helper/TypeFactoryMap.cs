namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	public struct TypeFactoryMap
	{
		private readonly Func<IServiceProvider, object> implementationFactory;
		private readonly IEnumerable<Type> serviceTypes;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeFactoryMap"/> struct.
		/// </summary>
		internal TypeFactoryMap(Func<IServiceProvider, object> implementationFactory, IEnumerable<Type> serviceTypes)
		{
			this.implementationFactory = implementationFactory;
			this.serviceTypes = serviceTypes;
		}

		/// <summary>
		/// Gets the implementation factory.
		/// </summary>
		public Func<IServiceProvider, object> ImplementationFactory
		{
			[DebuggerStepThrough]
			get { return this.implementationFactory; }
		}

		/// <summary>
		/// Gets the service types.
		/// </summary>
		public IEnumerable<Type> ServiceTypes
		{
			[DebuggerStepThrough]
			get { return this.serviceTypes; }
		}
	}
}