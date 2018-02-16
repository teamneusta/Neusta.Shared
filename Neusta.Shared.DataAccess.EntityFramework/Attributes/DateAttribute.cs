namespace Neusta.Shared.DataAccess.EntityFramework
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DateAttribute : Attribute
	{
	}
}