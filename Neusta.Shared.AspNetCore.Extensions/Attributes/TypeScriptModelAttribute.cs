namespace Neusta.Shared.AspNetCore.Extensions.Attributes
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
	public class TypeScriptModelAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TypeScriptModelAttribute"/> class.
		/// </summary>
		public TypeScriptModelAttribute()
		{
		}
	}
}