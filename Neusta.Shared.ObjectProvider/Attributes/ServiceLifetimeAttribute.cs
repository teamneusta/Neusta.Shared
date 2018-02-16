namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ServiceLifetimeAttribute : Attribute
	{
		private readonly ServiceLifetime serviceLifetime;
		private string scopeName;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceLifetimeAttribute"/> class.
		/// </summary>
		public ServiceLifetimeAttribute(ServiceLifetime serviceLifetime)
		{
			this.serviceLifetime = serviceLifetime;
		}

		/// <summary>
		/// Gets the lifetime scope.
		/// </summary>
		public ServiceLifetime ServiceLifetime
		{
			[DebuggerStepThrough]
			get { return this.serviceLifetime; }
		}

		/// <summary>
		/// Gets or sets the name of the scope.
		/// </summary>
		public string ScopeName
		{
			[DebuggerStepThrough]
			get { return this.scopeName; }
			[DebuggerStepThrough]
			set { this.scopeName = value; }
		}
	}
}
