namespace Neusta.Shared.ObjectProvider
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DisableDisposalTrackingAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DisableDisposalTrackingAttribute"/> class.
		/// </summary>
		public DisableDisposalTrackingAttribute()
		{
		}
	}
}