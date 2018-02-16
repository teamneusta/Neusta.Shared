namespace Neusta.Shared.ObjectProvider.Base
{
	using System.Diagnostics;
	using System.Threading;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DisposableObjects;
	using Neusta.Shared.Logging;

	public abstract class BaseObjectProviderScope<T> : DisposableObject
		where T : BaseObjectProviderScope<T>, IObjectProviderScope
	{
		private static readonly ILogger logger = LogManager.GetLogger<T>();

		private readonly IObjectProvider parentProvider;
		private readonly string name;
		private readonly long scopeId;
		private bool releaseOnDispose;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisposableObject"/> class.
		/// </summary>
		protected BaseObjectProviderScope(IObjectProvider parentProvider, string name)
		{
			this.parentProvider = parentProvider;
			this.name = name;
			this.scopeId = Interlocked.Increment(ref BaseObjectProviderScope.ScopeIdCounter);

			// Some logging
			if (logger.IsTraceEnabled)
			{
				if (string.IsNullOrEmpty(this.name))
				{
					logger.Trace("Create scope (#{0})", this.scopeId);
				}
				else
				{
					logger.Trace("Create named scope: {0} (#{1})", this.name, this.scopeId);
				}
			}
		}

		/// <summary>
		/// Gets the logger.
		/// </summary>
		[UsedImplicitly]
		protected static ILogger Logger
		{
			[DebuggerStepThrough]
			get { return logger; }
		}

		#region Implementation of IObjectProviderScope

		/// <summary>
		/// Gets the scope name.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return this.name; }
		}

		/// <summary>
		/// Gets the parent provider.
		/// </summary>
		public IObjectProvider ParentProvider
		{
			[DebuggerStepThrough]
			get { return this.parentProvider; }
		}

		/// <summary>
		/// Gets a value indicating whether the scope is released on Dispose.
		/// </summary>
		public bool ReleaseOnDispose
		{
			[DebuggerStepThrough]
			get { return this.releaseOnDispose; }
		}

		/// <summary>
		/// Keeps the scope alive on Dispose until Release method is called.
		/// </summary>
		public void EnforceKeepAliveOnDispose()
		{
			this.releaseOnDispose = false;
		}

		/// <summary>
		/// Releases the scope.
		/// </summary>
		public void Release()
		{
			this.InternalRelease();
		}

		#endregion

		#region Overrides of DisposableObject

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		protected sealed override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (this.releaseOnDispose)
			{
				this.InternalRelease();
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Releases the scope.
		/// </summary>
		protected virtual void InternalRelease()
		{
			// Some logging
			if (logger.IsTraceEnabled)
			{
				if (string.IsNullOrEmpty(this.name))
				{
					logger.Trace("Release scope (#{0})", this.scopeId);
				}
				else
				{
					logger.Trace("Release named scope: {0} (#{1})", this.name, this.scopeId);
				}
			}
		}

		#endregion
	}
}