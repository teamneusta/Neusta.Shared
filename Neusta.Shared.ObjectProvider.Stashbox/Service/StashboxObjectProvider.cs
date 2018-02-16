namespace Neusta.Shared.ObjectProvider.Stashbox.Service
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using global::Stashbox;
	using JetBrains.Annotations;

	internal class StashboxObjectProvider : IObjectProviderRoot
	{
		private readonly StashboxContainer container;

		/// <summary>
		/// Initializes a new instance of the <see cref="StashboxObjectProvider"/> class.
		/// </summary>
		public StashboxObjectProvider(StashboxContainer container)
		{
			this.container = container;
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

		#region Implementation of IServiceProvider

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetService(Type serviceType) => this.container.Resolve(serviceType);

		#endregion

		#region Implementation of IServiceLocator

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType) => this.container.Resolve(serviceType);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, string key) => this.container.Resolve(serviceType, key);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IEnumerable<object> GetAllInstances(Type serviceType) => this.container.ResolveAll(serviceType);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>() => this.container.Resolve<TService>();

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(string key) => this.container.Resolve<TService>(key);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IEnumerable<TService> GetAllInstances<TService>() => this.container.ResolveAll<TService>();

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
		public object GetInstance(Type serviceType, object[] dependencyOverrides) => this.container.Resolve(serviceType, false, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, string key, object[] dependencyOverrides) => this.container.Resolve(serviceType, key,false, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(object[] dependencyOverrides) => this.container.Resolve<TService>(false, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(string key, object[] dependencyOverrides) => this.container.Resolve<TService>(key, false, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object QueryInstance(Type serviceType, object[] dependencyOverrides = null) => this.container.Resolve(serviceType, true, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object QueryInstance(Type serviceType, string key, object[] dependencyOverrides = null) => this.container.Resolve(serviceType, key,true, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService QueryInstance<TService>(object[] dependencyOverrides = null) => this.container.Resolve<TService>(true, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService QueryInstance<TService>(string key, object[] dependencyOverrides = null) => this.container.Resolve<TService>(key, true, dependencyOverrides);

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IObjectProviderScope CreateScope(string name = null)
		{
			return new StashboxScope(this, name, this.container, this.container, true);
		}

		#endregion
	}
}