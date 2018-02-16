namespace Neusta.Shared.DataAccess.UnitOfWork.Factory
{
	using System;
	using System.Collections.Concurrent;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;
	using Neusta.Shared.Core;
	using Neusta.Shared.Core.Utils;

	internal sealed class SimpleUnitOfWorkServiceFactory : Singleton<SimpleUnitOfWorkServiceFactory>, IUnitOfWorkServiceFactory
	{
		private static readonly ConcurrentDictionary<Type, ConstructorInfo> ctorMapCache = new ConcurrentDictionary<Type, ConstructorInfo>();

		private static readonly TypeInfo dataContextTypeInfo = typeof(IDataContext).GetTypeInfo();
		private static readonly TypeInfo dataRepositoryTypeInfo = typeof(IDataRepository).GetTypeInfo();
		private static readonly TypeInfo unitOfWorkTypeInfo = typeof(IUnitOfWork).GetTypeInfo();
		private static readonly TypeInfo serviceProviderTypeInfo = typeof(IServiceProvider).GetTypeInfo();
		private static readonly TypeInfo singletonTypeInfo = typeof(ISingleton).GetTypeInfo();

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleUnitOfWorkServiceFactory"/> class.
		/// </summary>
		[UsedImplicitly]
		private SimpleUnitOfWorkServiceFactory()
		{
		}

		#region Implementation of IUnitOfWorkServiceFactory

		/// <summary>
		/// Requests a data context of the specified type.
		/// </summary>
		public IDataContext CreateDataContext(IUnitOfWork unitOfWork, Type dataContextType)
		{
			ConstructorInfo ctorInfo = SelectConstructor(dataContextType);
			int count = ctorInfo.GetParameters().Length;
			var values = new object[count];
			for (var idx = 0; idx < count; idx++)
			{
				ParameterInfo parameter = ctorInfo.GetParameters()[idx];
				Type parameterType = parameter.ParameterType;
				TypeInfo parameterTypeInfo = parameterType.GetTypeInfo();
				if (unitOfWorkTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = this;
				}
				else if (singletonTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = SingletonHelper.GetSingletonValue(parameterType);
				}
				else if (parameterTypeInfo.IsValueType)
				{
					values[idx] = Activator.CreateInstance(parameterType);
				}
				else
				{
					values[idx] = this.CreateServiceInstance(unitOfWork, parameterType);
				}
			}
			return ctorInfo.Invoke(values) as IDataContext;
		}

		/// <summary>
		/// Creates a data repository of the specified type.
		/// </summary>
		public IDataRepository CreateDataRepository(IUnitOfWork unitOfWork, Type dataRepositoryType)
		{
			ConstructorInfo ctorInfo = SelectConstructor(dataRepositoryType);
			int count = ctorInfo.GetParameters().Length;
			var values = new object[count];
			for (var idx = 0; idx < count; idx++)
			{
				ParameterInfo parameter = ctorInfo.GetParameters()[idx];
				Type parameterType = parameter.ParameterType;
				TypeInfo parameterTypeInfo = parameterType.GetTypeInfo();
				if (dataContextTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = unitOfWork.GetDataContext(parameterType);
				}
				else if (dataRepositoryTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = unitOfWork.GetRepository(parameterType);
				}
				else if (unitOfWorkTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = unitOfWork;
				}
				else if (singletonTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = SingletonHelper.GetSingletonValue(parameterType);
				}
				else if (parameterTypeInfo.IsValueType)
				{
					values[idx] = Activator.CreateInstance(parameterType);
				}
				else
				{
					values[idx] = this.CreateServiceInstance(unitOfWork, parameterType);
				}
			}
			return ctorInfo.Invoke(values) as IDataRepository;
		}

		/// <summary>
		/// Creates a service instance of the specified type.
		/// </summary>
		public object CreateServiceInstance(IUnitOfWork unitOfWork, Type serviceType)
		{
			ConstructorInfo ctorInfo = SelectConstructor(serviceType);
			int count = ctorInfo.GetParameters().Length;
			var values = new object[count];
			for (var idx = 0; idx < count; idx++)
			{
				ParameterInfo parameter = ctorInfo.GetParameters()[idx];
				Type type = parameter.ParameterType;
				TypeInfo parameterTypeInfo = type.GetTypeInfo();
				if (dataContextTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = unitOfWork.GetDataContext(type);
				}
				else if (dataRepositoryTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = unitOfWork.GetRepository(type);
				}
				else if (unitOfWorkTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = unitOfWork;
				}
				else if (serviceProviderTypeInfo.IsAssignableFrom(parameterTypeInfo))
				{
					values[idx] = null;
				}
				else if (parameterTypeInfo.IsValueType)
				{
					values[idx] = Activator.CreateInstance(type);
				}
				else
				{
					values[idx] = this.CreateServiceInstance(unitOfWork, type);
				}
			}
			return ctorInfo.Invoke(values) as IDataRepository;
		}

		#endregion

		#region Private Methods

		private static ConstructorInfo SelectConstructor(Type serviceType)
		{
			return ctorMapCache.GetOrAdd(serviceType, delegate (Type innerType)
			{
				TypeInfo typeInfo = innerType.GetTypeInfo();
				if (typeInfo.IsInterface || typeInfo.IsAbstract)
				{
					throw new UnitOfWorkException(string.Format("Cannot create an instance of abstract/interface type {0}.",
						typeInfo.Name));
				}
				ConstructorInfo[] ctorList = typeInfo.DeclaredConstructors.ToArray();
				if (ctorList.Length > 1)
				{
					// Filter for public constructors which are not Obsolete
					ctorList = ctorList.Where(x => x.IsPublic && x.GetCustomAttribute<ObsoleteAttribute>(true) == null).ToArray();
					if (ctorList.Length == 0)
					{
						throw new UnitOfWorkException(string.Format(
							"Cannot create an instance of type {0} because it has multiple private and no public constructors.",
							typeInfo.Name));
					}
					if (ctorList.Length > 1)
					{
						// Filter for constructors that accept IUnitOfWork
						ctorList = ctorList.Where(delegate (ConstructorInfo x)
						{
							ParameterInfo[] parameters = x.GetParameters();
							if (parameters.Any(y => unitOfWorkTypeInfo.IsAssignableFrom(y.ParameterType.GetTypeInfo())))
							{
								return true;
							}
							return false;
						}).ToArray();

						if (ctorList.Length > 1)
						{
							ctorList = ctorList.Where(x => x.GetParameters().Length == 1).ToArray();
						}

						// Fail if we have more than one candidate
						if (ctorList.Length > 1)
						{
							throw new UnitOfWorkException(string.Format(
								"Cannot create an instance of type {0} because it has multiple public constructors.", typeInfo.Name));
						}
					}
				}
				return ctorList[0];
			});
		}

		#endregion
	}
}