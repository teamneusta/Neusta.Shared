namespace Neusta.Shared.AspNetCore.Extensions.Attributes
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Class)]
	public class TypeScriptClientAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TypeScriptClientAttribute"/> class.
		/// </summary>
		public TypeScriptClientAttribute()
		{
		}
	}
}