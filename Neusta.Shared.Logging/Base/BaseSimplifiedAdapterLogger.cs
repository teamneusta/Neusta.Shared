namespace Neusta.Shared.Logging.Base
{
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using JetBrains.Annotations;

	public abstract class BaseSimplifiedAdapterLogger<TAdapter> : BaseSimplifiedLogger
		where TAdapter : BaseLoggingAdapter
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly TAdapter adapter;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseSimplifiedAdapterLogger{TAdapter}"/> class.
		/// </summary>
		protected BaseSimplifiedAdapterLogger(TAdapter adapter, string name)
			: base(name)
		{
			this.adapter = adapter;

			// ReSharper disable once VirtualMemberCallInConstructor
			this.UpdateIsEnabledValues();
		}

		/// <summary>
		/// Gets the adapter.
		/// </summary>
		[PublicAPI]
		public TAdapter Adapter
		{
			[DebuggerStepThrough]
			get { return this.adapter; }
		}

		#region Overrides of BaseSimplifiedLogger

		/// <summary>
		/// Gets a value indicating whether logging is enabled for the specified level.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool IsEnabled(LogLevel level)
		{
			return this.adapter.IsLoggingEnabled(level);
		}

		/// <summary>
		/// Updates the IsEnabled values.
		/// </summary>
		protected override void UpdateIsEnabledValues()
		{
			if (this.adapter != null)
			{
				base.UpdateIsEnabledValues();
			}
		}

		#endregion
	}
}