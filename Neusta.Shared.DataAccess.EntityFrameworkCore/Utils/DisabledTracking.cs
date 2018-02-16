namespace Neusta.Shared.DataAccess.EntityFrameworkCore.Utils
{
	using System;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;

	internal class DisabledTracking : IDisposable
	{
		private readonly ChangeTracker changeTracker;
		private readonly bool oldAutoDetectChanges;
		private readonly QueryTrackingBehavior oldTrackingBehavior;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisabledTracking"/> class.
		/// </summary>
		public DisabledTracking(ChangeTracker changeTracker)
		{
			this.changeTracker = changeTracker;

			this.oldAutoDetectChanges = this.changeTracker.AutoDetectChangesEnabled;
			this.changeTracker.AutoDetectChangesEnabled = false;

			this.oldTrackingBehavior = this.changeTracker.QueryTrackingBehavior;
			this.changeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.changeTracker.AutoDetectChangesEnabled = this.oldAutoDetectChanges;
			this.changeTracker.QueryTrackingBehavior = this.oldTrackingBehavior;
		}
	}
}