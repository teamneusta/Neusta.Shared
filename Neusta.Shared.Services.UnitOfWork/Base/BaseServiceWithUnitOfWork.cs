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
	public abstract class BaseServiceWithUnitOfWork<TService> : BaseService<TService>, IUnitOfWorkOwner, IObjectProviderProvider
		where TService : BaseServiceWithUnitOfWork<TService>
	{
		private readonly IUnitOfWork unitOfWork;

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseServiceWithUnitOfWork{TService}"/> class.
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

		/// <summary>
		/// Gets the object provider.
		/// </summary>
		[PublicAPI]
		public virtual IObjectProvider ObjectProvider
		{
			[DebuggerStepThrough]
			get { return this.unitOfWork.ObjectProvider; }
		}

		/// <summary>
		/// Creates a child service.
		/// </summary>
		[PublicAPI]
		public virtual TChildService GetChildService<TChildService>(object[] dependencyOverrides = null)
		{
			IObjectProvider objectProvider = this.ObjectProvider;
			if (typeof(IUnitOfWorkOwner).IsAssignableFrom(typeof(TChildService)))
			{
				IUnitOfWork unitOfWorkClone = this.UnitOfWork.Clone();
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
			else if (objectProvider != null)
			{
				return objectProvider.GetInstance<TChildService>(dependencyOverrides);
			}
			else
			{
				return (TChildService)Activator.CreateInstance(typeof(TChildService));
			}
		}
	}
}