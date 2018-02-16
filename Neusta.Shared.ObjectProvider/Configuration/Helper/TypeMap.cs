namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	public struct TypeMap
	{
		private readonly Type implementationType;
		private readonly IEnumerable<Type> serviceTypes;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeMap"/> struct.
		/// </summary>
		internal TypeMap(Type implementationType, IEnumerable<Type> serviceTypes)
		{
			this.implementationType = implementationType;
			this.serviceTypes = serviceTypes;
		}

		/// <summary>
		/// Gets the type of the implementation.
		/// </summary>
		public Type ImplementationType
		{
			[DebuggerStepThrough]
			get { return this.implementationType; }
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