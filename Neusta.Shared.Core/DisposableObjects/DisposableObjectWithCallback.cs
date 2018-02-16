namespace Neusta.Shared.Core.DisposableObjects
{
	using System;
	using Neusta.Shared.Logging;

	public abstract class DisposableObjectWithCallback : DisposableObject, IDisposeCallback
	{
		private static readonly ILogger logger = LogManager.GetLogger<DisposableObjectWithCallback>();

		#region Implementation of IDisposeCallback

		/// <summary>
		/// This event should be triggered when implementing instance is disposed.
		/// </summary>
		public event EventHandler OnDispose;

		#endregion

		#region Overrides of DisposableObject

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			// Invoke the OnDispse event
			EventHandler handler = this.OnDispose;
			if (handler != null)
			{
				try
				{
					handler(this, EventArgs.Empty);
				}
				catch (Exception ex)
				{
					// Log all exceptions, but ignore them
					logger.Error("Unhandled exception when executing OnDispose event: " + ex.Message, ex);
				}
			}
		}

		#endregion
	}
}