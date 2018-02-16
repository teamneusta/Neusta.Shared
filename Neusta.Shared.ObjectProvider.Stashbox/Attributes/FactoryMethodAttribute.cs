namespace Neusta.Shared.ObjectProvider.Stashbox.Attributes
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class FactoryMethodAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FactoryMethodAttribute"/> class.
		/// </summary>
		public FactoryMethodAttribute()
		{
		}
	}
}