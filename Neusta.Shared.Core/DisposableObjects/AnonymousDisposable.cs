namespace Neusta.Shared.Core.DisposableObjects
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public class AnonymousDisposable : DisposableObject
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Action disposeAction;

		/// <summary>
		/// Initializes a new instance of the <see cref="AnonymousDisposable"/> class.
		/// </summary>
		public AnonymousDisposable()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AnonymousDisposable"/> class.
		/// </summary>
		public AnonymousDisposable(Action disposeAction)
		{
			Guard.ArgumentNotNull(disposeAction, "disposeAction");

			this.disposeAction = disposeAction;
		}

		/// <summary>
		/// Gets the dispose action.
		/// </summary>
		[PublicAPI]
		public Action DisposeAction
		{
			[DebuggerStepThrough]
			get { return this.disposeAction; }
		}

		#region Overrides of DisposableObject

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (this.disposeAction != null)
			{
				this.disposeAction();
			}
		}

		#endregion
	}
}