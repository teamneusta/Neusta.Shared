namespace Neusta.Shared.Core.Utils
{
	using System;

	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SingletonInstancePropertyAttribute : Attribute
	{
	}
}