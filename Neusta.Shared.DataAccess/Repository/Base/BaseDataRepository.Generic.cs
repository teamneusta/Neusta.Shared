namespace Neusta.Shared.DataAccess.Repository
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;

	public abstract class BaseDataRepository<TDataContext> : BaseDataRepository, IDataRepository<TDataContext>
		where TDataContext : IDataContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseDataRepository{TDataContext}"/> class.
		/// </summary>
		[PublicAPI]
		protected BaseDataRepository(TDataContext dataContext)
			: base(dataContext)
		{
		}

		#region Implementation of IDataContextOwner<TDataContext>

		/// <summary>
		/// Gets the <see cref="IDataContext" />.
		/// </summary>
		public new TDataContext DataContext
		{
			[DebuggerStepThrough]
			get { return (TDataContext)base.DataContext; }
		}

		#endregion
	}
}