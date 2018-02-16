namespace Neusta.Shared.DataAccess.UnitOfWork
{
	using System;
	using System.Transactions;
	using JetBrains.Annotations;

	public sealed class UnitOfWork : BaseUnitOfWork
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWork"/> class.
		/// </summary>
		[UsedImplicitly]
		public UnitOfWork()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWork"/> class.
		/// </summary>
		[UsedImplicitly]
		private UnitOfWork(IServiceProvider serviceProvider, UnitOfWork parentUnitOfWork = null)
			: base(serviceProvider, parentUnitOfWork)
		{
		}

		public TransactionScope BeginTransactionScope()
		{
			var options = new TransactionOptions()
			{
				IsolationLevel = IsolationLevel.RepeatableRead,
				Timeout = TimeSpan.FromMinutes(1)
			};
			var scope = new TransactionScope(TransactionScopeOption.Required, options, TransactionScopeAsyncFlowOption.Enabled);
			return scope;
		}

		#region Overrides of BaseUnitOfWork

		/// <summary>
		/// Clones this instance.
		/// </summary>
		public override IUnitOfWork Clone()
		{
			return new UnitOfWork(this.ServiceProvider, this);
		}

		#endregion
	}
}