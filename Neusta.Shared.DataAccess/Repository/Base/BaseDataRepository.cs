namespace Neusta.Shared.DataAccess.Repository
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	public abstract class BaseDataRepository : IDataRepository
	{
		private readonly IDataContext dataContext;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseDataRepository"/> class.
		/// </summary>
		[PublicAPI]
		protected BaseDataRepository(IDataContext dataContext)
		{
			Guard.ArgumentNotNull(dataContext, nameof(dataContext));

			this.dataContext = dataContext;
		}

		/// <summary>
		/// Gets the data context.
		/// </summary>
		[PublicAPI]
		public virtual IDataContext DataContext
		{
			[DebuggerStepThrough]
			get { return this.dataContext; }
		}


		#region Implementation of IExecuteThreadSafe

		/// <summary>
		/// Executes the specified action thread safe.
		/// </summary>
		public void ExecuteThreadSafe(Action action)
		{
			this.DataContext.ExecuteThreadSafe(action);
		}

		/// <summary>
		/// Executes the specified function thread safe.
		/// </summary>
		public TResult ExecuteThreadSafe<TResult>(Func<TResult> func)
		{
			return this.DataContext.ExecuteThreadSafe(func);
		}

		/// <summary>
		/// Executes the specified action thread safe.
		/// </summary>
		public Task ExecuteThreadSafeAsync(Func<Task> asyncAction)
		{
			return this.DataContext.ExecuteThreadSafeAsync(asyncAction);
		}

		/// <summary>
		/// Executes the specified function thread safe.
		/// </summary>
		public Task<TResult> ExecuteThreadSafeAsync<TResult>(Func<Task<TResult>> asyncFunc)
		{
			return this.DataContext.ExecuteThreadSafeAsync(asyncFunc);
		}

		#endregion

		#region Explicit Implementation of IDataContextOwner

		/// <summary>
		/// Gets the <see cref="IDataContext" />.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[DebuggerHidden]
		IDataContext IDataContextOwner.DataContext
		{
			[DebuggerStepThrough]
			get { return this.DataContext; }
		}

		/// <summary>
		/// Gets the <see cref="IDataContextExtensions" />.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[DebuggerHidden]
		IDataContextExtensions IDataContextOwner.Extensions
		{
			[DebuggerStepThrough]
			get { return this.DataContext.Extensions; }
		}

		#endregion
	}
}