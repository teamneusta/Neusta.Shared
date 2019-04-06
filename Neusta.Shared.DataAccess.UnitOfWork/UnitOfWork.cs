namespace Neusta.Shared.DataAccess.UnitOfWork
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess.UnitOfWork.Base;
	using Neusta.Shared.DataAccess.UnitOfWork.Utils;

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

		/// <summary>
		/// Gets the current <see cref="IUnitOfWork" />.
		/// </summary>
		[PublicAPI]
		public static IUnitOfWork Current
		{
			get
			{
				ContextData threadContextData;
				if (CallContextCurrentData.TryGetCurrentData(out threadContextData))
				{
					return threadContextData.CurrentUnitOfWork;
				}
				return null;
			}
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