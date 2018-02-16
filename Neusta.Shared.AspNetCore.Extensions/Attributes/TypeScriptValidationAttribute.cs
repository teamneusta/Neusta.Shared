namespace Neusta.Shared.AspNetCore.Extensions.Attributes
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Class)]
	public class TypeScriptValidationAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TypeScriptValidationAttribute"/> class.
		/// </summary>
		public TypeScriptValidationAttribute()
		{
		}
	}
}
