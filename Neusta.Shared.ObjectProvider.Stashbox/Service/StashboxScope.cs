namespace Neusta.Shared.ObjectProvider.Stashbox.Service
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using CommonServiceLocator;
	using global::Stashbox;
	using JetBrains.Annotations;
	using Neusta.Shared.ObjectProvider.Base;

	internal class StashboxScope : BaseObjectProviderScope<StashboxScope>, IObjectProviderScope
	{
		private readonly StashboxContainer container;
		private readonly IDependencyResolver dependencyResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="StashboxScope"/> class.
		/// </summary>
		internal StashboxScope(IObjectProvider parentProvider, string name, StashboxContainer container, IDependencyResolver dependencyResolver, bool attachToParent)
			: base(parentProvider, name)
		{
			this.container = container;

			this.dependencyResolver = dependencyResolver
				.BeginScope(name, attachToParent)
				.PutInstanceInScope<IServiceLocator>(this, true)
				.PutInstanceInScope<IServiceProvider>(this, true)
				.PutInstanceInScope<IObjectProvider>(this, true)
				.PutInstanceInScope<IObjectProviderScope>(this, true);
		}

		/// <summary>
		/// Gets the container.
		/// </summary>
		[PublicAPI]
		public StashboxContainer Container
		{
			[DebuggerStepThrough]
			get { return this.container; }
		}

		/// <summary>
		/// Gets the dependency resolver.
		/// </summary>
		[PublicAPI]
		public IDependencyResolver DependencyResolver
		{
			[DebuggerStepThrough]
			get { return this.dependencyResolver; }
		}

		#region Implementation of IServiceProvider

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetService(Type serviceType) => this.dependencyResolver.Resolve(serviceType);

		#endregion

		#region Implementation of IServiceLocator

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType) => this.dependencyResolver.Resolve(serviceType);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, string key) => this.dependencyResolver.Resolve(serviceType, key);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IEnumerable<object> GetAllInstances(Type serviceType) => this.dependencyResolver.ResolveAll(serviceType);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>() => this.dependencyResolver.Resolve<TService>();

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(string key) => this.dependencyResolver.Resolve<TService>(key);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IEnumerable<TService> GetAllInstances<TService>() => this.dependencyResolver.ResolveAll<TService>();

		#endregion

		#region Implementation of IObjectProvider

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Has(Type serviceType) => this.container.CanResolve(serviceType);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Has<TService>() => this.container.CanResolve<TService>();

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, object[] dependencyOverrides) => this.dependencyResolver.Resolve(serviceType, false, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(object[] dependencyOverrides) => this.dependencyResolver.Resolve<TService>(false, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, string key, object[] dependencyOverrides) => this.dependencyResolver.Resolve(serviceType, key, false, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(string key, object[] dependencyOverrides) => this.dependencyResolver.Resolve<TService>(key, false, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object QueryInstance(Type serviceType, object[] dependencyOverrides) => this.dependencyResolver.Resolve(serviceType, true, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object QueryInstance(Type serviceType, string key, object[] dependencyOverrides) => this.dependencyResolver.Resolve(serviceType, key, true, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService QueryInstance<TService>(object[] dependencyOverrides) => this.dependencyResolver.Resolve<TService>(true, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService QueryInstance<TService>(string key, object[] dependencyOverrides) => this.dependencyResolver.Resolve<TService>(key, true, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IObjectProviderScope CreateScope(string name = null)
		{
			return new StashboxScope(this, name, this.container, this.dependencyResolver, true);
		}

		#endregion

		#region Overrides of BaseObjectProviderScope

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		protected override void InternalRelease()
		{
			base.InternalRelease();
			this.dependencyResolver.Dispose();
		}

		#endregion
	}
}