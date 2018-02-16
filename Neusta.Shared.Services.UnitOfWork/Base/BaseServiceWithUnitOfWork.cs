namespace Neusta.Shared.Services.UnitOfWork
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using JetBrains.Annotations;
	using Neusta.Shared.DataAccess;
	using Neusta.Shared.ObjectProvider;
	using Neusta.Shared.Services.Base;

	[UsedImplicitly]
	public abstract class BaseServiceWithUnitOfWork<TService> : BaseService<TService>, IUnitOfWorkOwner
		where TService : BaseServiceWithUnitOfWork<TService>
	{
		private readonly IUnitOfWork unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseService{TService}"/> class.
		/// </summary>
		protected BaseServiceWithUnitOfWork(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		/// <summary>
		/// Gets the unit of work.
		/// </summary>
		[PublicAPI]
		public IUnitOfWork UnitOfWork
		{
			[DebuggerStepThrough]
			get { return this.unitOfWork; }
		}

		public TChildService GetChildService<TChildService>(object[] dependencyOverrides = null)
		{
			IUnitOfWork unitOfWorkClone = this.UnitOfWork.Clone();
			IObjectProvider objectProvider = this.UnitOfWork.ObjectProvider;
			if (objectProvider != null)
			{
				if ((dependencyOverrides != null) && dependencyOverrides.Any())
				{
					int length = dependencyOverrides.Length;
					Array.Resize(ref dependencyOverrides, length + 1);
					dependencyOverrides[length] = unitOfWorkClone;
				}
				else
				{
					dependencyOverrides = new object[] { unitOfWorkClone };
				}
				return objectProvider.GetInstance<TChildService>(dependencyOverrides);
			}
			else
			{
				return (TChildService)Activator.CreateInstance(typeof(TChildService), unitOfWorkClone);
			}
		}
	}
}