namespace Neusta.Shared.ObjectProvider.Stashbox.Attributes
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class FactoryTypeAttribute : Attribute
	{
		private readonly Type factoryType;

		/// <summary>
		/// Initializes a new instance of the <see cref="FactoryTypeAttribute"/> class.
		/// </summary>
		public FactoryTypeAttribute(Type factoryType)
		{
			this.factoryType = factoryType;
		}

		/// <summary>
		/// Gets the type of the factory class.
		/// </summary>
		public Type FactoryType
		{
			[DebuggerStepThrough]
			get { return this.factoryType; }
		}
	}
}