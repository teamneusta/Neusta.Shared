namespace Neusta.Shared.ObjectProvider.Base
{
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.DisposableObjects;
	using Neusta.Shared.Logging;
	using Neusta.Shared.ObjectProvider.Utils;

	public abstract class BaseObjectProviderScope<T> : BaseObjectProviderScope
		where T : BaseObjectProviderScope<T>, IObjectProviderScope
	{
		private static readonly ILogger logger = LogManager.GetLogger<T>();

		private BaseObjectProviderScope savedCurrentScope;
		private ContextData threadContextData;

		private readonly IObjectProvider parentProvider;
		private readonly string name;
		private bool releaseOnDispose;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisposableObject"/> class.
		/// </summary>
		protected BaseObjectProviderScope(IObjectProvider parentProvider, string name)
		{
			this.parentProvider = parentProvider;
			this.name = name;

			// Some logging
			if (logger.IsTraceEnabled)
			{
				if (string.IsNullOrEmpty(this.name))
				{
					logger.Trace("Create scope (#{0})", this.ScopeId);
				}
				else
				{
					logger.Trace("Create named scope: {0} (#{1})", this.name, this.ScopeId);
				}
			}

			// Push scope to the stack
			this.PushScope();
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

			// Release the scope
			if (this.releaseOnDispose)
			{
				this.InternalRelease();
			}

			// Remove scope from the stack
			this.PopScope();
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
					logger.Trace("Release scope (#{0})", this.ScopeId);
				}
				else
				{
					logger.Trace("Release named scope: {0} (#{1})", this.name, this.ScopeId);
				}
			}
		}

		#endregion

		#region Private Methods

		private void PushScope()
		{
			// Get current CallContext data
			this.threadContextData = CallContextCurrentData.CreateOrGetCurrentData(this.ContextKey);

			// Save the previous scope and save our scope
			this.savedCurrentScope = this.threadContextData.CurrentScope;
			this.threadContextData.CurrentScope = this;
		}

		private void PopScope()
		{
			// Clear the current scope in CallContext data
			CallContextCurrentData.ClearCurrentData(this.ContextKey, true);

			// Restore threadContextData to parent CallContext or TLS data
			if (this.savedCurrentScope != null)
			{
				this.threadContextData = CallContextCurrentData.CreateOrGetCurrentData(this.savedCurrentScope.ContextKey);
				this.threadContextData.CurrentScope = this.savedCurrentScope;
			}
			else
			{
				// Clear any CallContext data
				CallContextCurrentData.ClearCurrentData(null, false);
			}
		}

		#endregion
	}
}