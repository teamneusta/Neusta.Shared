namespace Neusta.Shared.ObjectProvider.Base
{
	using System.Diagnostics;
	using System.Threading;
	using Neusta.Shared.Core.DisposableObjects;
	using Neusta.Shared.ObjectProvider.Utils;

	public abstract class BaseObjectProviderScope : DisposableObject
	{
		internal static long ScopeIdCounter;

		private readonly long scopeId;
		private readonly ContextKey contextKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseObjectProviderScope"/> class.
		/// </summary>
		protected BaseObjectProviderScope()
		{
			this.scopeId = Interlocked.Increment(ref BaseObjectProviderScope.ScopeIdCounter);
			this.contextKey = new ContextKey();
		}

		/// <summary>
		/// Gets the scope identifier.
		/// </summary>
		public long ScopeId
		{
			[DebuggerStepThrough]
			get { return this.scopeId; }
		}

		/// <summary>
		/// Gets the context key.
		/// </summary>
		internal ContextKey ContextKey
		{
			[DebuggerStepThrough]
			get { return this.contextKey; }
		}
	}
}