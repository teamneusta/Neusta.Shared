namespace Neusta.Shared.ObjectProvider.Base
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.ObjectProvider.Utils;

	public abstract class BaseObjectProviderImplementation : IObjectProvider
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseObjectProviderImplementation"/> class.
		/// </summary>
		protected BaseObjectProviderImplementation()
		{
		}

		#region Implementation of IServiceProvider

		/// <summary>
		/// Gets the service object of the specified type.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual object GetService(Type serviceType)
		{
			return this.DoGetInstance(serviceType);
		}

		#endregion

		#region Implementation of IServiceLocator

		/// <summary>
		/// Gets the instance.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual object GetInstance(Type serviceType)
		{
			return this.DoGetInstance(serviceType);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual object GetInstance(Type serviceType, string key)
		{
			return this.DoGetInstance(serviceType, key);
		}

		/// <summary>
		/// Gets all instances.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return this.DoGetAllInstances(serviceType);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual TService GetInstance<TService>()
		{
			var result = this.DoGetInstance(typeof(TService));
			if (result != null)
			{
				return (TService)result;
			}
			return default(TService);
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual TService GetInstance<TService>(string key)
		{
			var result = this.DoGetInstance(typeof(TService), key);
			if (result != null)
			{
				return (TService)result;
			}
			return default(TService);
		}

		/// <summary>
		/// Gets all instances.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual IEnumerable<TService> GetAllInstances<TService>()
		{
			foreach (var result in this.DoGetAllInstances(typeof(TService)))
			{
				if (result != null)
				{
					yield return (TService)result;
				}
				yield return default(TService);
			}
		}

		#endregion

		#region Implementation of IObjectProvider

		/// <summary>
		/// Checks if the provider has a binding for the specified service type.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool Has(Type serviceType)
		{
			return this.DoHas(serviceType);
		}

		/// <summary>
		/// Checks if the provider has a binding for the specified service type.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool Has<TService>()
		{
			return this.DoHas(typeof(TService));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, object[] dependencyOverrides)
		{
			return this.GetInstance(serviceType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, string key, object[] dependencyOverrides)
		{
			return this.GetInstance(serviceType, key);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(object[] dependencyOverrides)
		{
			return this.GetInstance<TService>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(string key, object[] dependencyOverrides)
		{
			return this.GetInstance<TService>(key);
		}

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Returns null if a binding for the specified type is not available.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual object QueryInstance(Type serviceType, object[] dependencyOverrides = null)
		{
			if (this.DoHas(serviceType))
			{
				return this.DoGetInstance(serviceType);
			}
			return null;
		}

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Returns null if a binding for the specified type is not available.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual object QueryInstance(Type serviceType, string key, object[] dependencyOverrides = null)
		{
			if (this.DoHas(serviceType))
			{
				return this.DoGetInstance(serviceType, key);
			}
			return null;
		}

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Returns null if a binding for the specified type is not available.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual TService QueryInstance<TService>(object[] dependencyOverrides = null)
		{
			var result = this.QueryInstance(typeof(TService));
			if (result != null)
			{
				return (TService)result;
			}
			return default(TService);
		}

		/// <summary>
		/// Creates and injects an instance of the the specified service type.
		/// Returns null if a binding for the specified type is not available.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual TService QueryInstance<TService>(string key, object[] dependencyOverrides = null)
		{
			var result = this.QueryInstance(typeof(TService), key);
			if (result != null)
			{
				return (TService)result;
			}
			return default(TService);
		}

		/// <summary>
		/// Gets a new scope.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual IObjectProviderScope CreateScope(string name = null)
		{
			return new AnonymousScope(this);
		}

		#endregion

		#region Abstract Methods

		protected abstract object DoGetInstance(Type serviceType);

		protected abstract object DoGetInstance(Type serviceType, string key);

		protected abstract IEnumerable<object> DoGetAllInstances(Type serviceType);

		protected abstract bool DoHas(Type serviceType);

		#endregion
	}
}
