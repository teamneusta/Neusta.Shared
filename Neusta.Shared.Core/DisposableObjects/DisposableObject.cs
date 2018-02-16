namespace Neusta.Shared.Core.DisposableObjects
{
	using System;
	using System.Diagnostics;
	using System.Threading;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging;

	/// <summary>
	/// An <see cref="object"/> that implements the <see cref="IDisposable"/> interface.
	/// </summary>
	[PublicAPI]
	public abstract class DisposableObject : IDisposable, IDisposableObject
	{
		private static readonly ILogger logger = LogManager.GetLogger<DisposableObject>();

		// ReSharper disable once InconsistentNaming
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal int isDisposed;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisposableObject"/> class.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		protected DisposableObject()
		{
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="DisposableObject"/> is reclaimed by garbage collection.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		~DisposableObject()
		{
			if (Interlocked.Exchange(ref this.isDisposed, 1) == 0)
			{
				try
				{
					this.Dispose(false);
				}
				catch (Exception ex)
				{
					// Log this fatal exception, but do not rethrow it
					logger.Error("Unhandled exception in Finalizer: " + ex.Message, ex);
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is disposed.
		/// </summary>
		[PublicAPI]
		public bool IsDisposed
		{
			[DebuggerStepThrough]
			[DebuggerNonUserCode]
			get { return this.isDisposed != 0; }
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		[PublicAPI]
		public void Dispose()
		{
			if (Interlocked.Exchange(ref this.isDisposed, 1) == 0)
			{
				GC.SuppressFinalize(this);
				this.Dispose(true);
			}
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <remarks>
		/// Dispose(bool disposing) executes in two distinct scenarios.
		/// If disposing equals true, the method has been called directly
		/// or indirectly by a user's code. Managed and unmanaged resources
		/// can be disposed.
		/// If disposing equals false, the method has been called by the
		/// runtime from inside the finalizer and you should not reference
		/// other objects. Only unmanaged resources can be disposed.
		/// </remarks>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		protected virtual void Dispose(bool disposing)
		{
		}

		/// <summary>
		/// Checks if the object already was disposed and raises an <see cref="ObjectDisposedException"/> exception if it is disposed.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		[PublicAPI]
		protected virtual void CheckIsDisposed()
		{
			if (this.isDisposed != 0)
			{
				throw new ObjectDisposedException("Instance of type " + this.GetType().Name + " already disposed.");
			}
		}

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		#endregion
	}
}