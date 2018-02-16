namespace Neusta.Shared.ObjectProvider.Utils
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;

	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class AnonymousScope : IObjectProviderScope
	{
		private readonly IObjectProvider objectProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="AnonymousScope"/> class.
		/// </summary>
		public AnonymousScope(IObjectProvider objectProvider)
		{
			this.objectProvider = objectProvider;
		}

		#region Implementation of IObjectProviderScope

		/// <summary>
		/// Gets the scope name.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return null; }
		}

		/// <summary>
		/// Gets the parent provider.
		/// </summary>
		public IObjectProvider ParentProvider
		{
			[DebuggerStepThrough]
			get { return null; }
		}

		#endregion

		#region Implementation of IServiceProvider

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetService(Type serviceType)
		{
			return this.objectProvider.GetService(serviceType);
		}

		#endregion

		#region Implementation of IServiceLocator

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType)
		{
			return this.objectProvider.GetInstance(serviceType);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, string key)
		{
			return this.objectProvider.GetInstance(serviceType, key);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IEnumerable<object> GetAllInstances(Type serviceType)
		{
			return this.objectProvider.GetAllInstances(serviceType);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>()
		{
			return this.objectProvider.GetInstance<TService>();
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(string key)
		{
			return this.objectProvider.GetInstance<TService>(key);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IEnumerable<TService> GetAllInstances<TService>()
		{
			return this.objectProvider.GetAllInstances<TService>();
		}

		#endregion

		#region Implementation of IObjectProvider

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Has(Type serviceType)
		{
			return this.objectProvider.Has(serviceType);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Has<TService>()
		{
			return this.objectProvider.Has<TService>();
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, object[] dependencyOverrides)
		{
			return this.objectProvider.GetInstance(serviceType, dependencyOverrides);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetInstance(Type serviceType, string key, object[] dependencyOverrides)
		{
			return this.objectProvider.GetInstance(serviceType, key, dependencyOverrides);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(object[] dependencyOverrides)
		{
			return this.objectProvider.GetInstance<TService>(dependencyOverrides);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService GetInstance<TService>(string key, object[] dependencyOverrides)
		{
			return this.objectProvider.GetInstance<TService>(key, dependencyOverrides);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object QueryInstance(Type serviceType, object[] dependencyOverrides = null)
		{
			return this.objectProvider.QueryInstance(serviceType, dependencyOverrides);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object QueryInstance(Type serviceType, string key, object[] dependencyOverrides = null)
		{
			return this.objectProvider.QueryInstance(serviceType, key, dependencyOverrides);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService QueryInstance<TService>(object[] dependencyOverrides = null)
		{
			return this.objectProvider.QueryInstance<TService>(dependencyOverrides);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TService QueryInstance<TService>(string key, object[] dependencyOverrides = null)
		{
			return this.objectProvider.QueryInstance<TService>(key, dependencyOverrides);
		}

		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IObjectProviderScope CreateScope(string name = null)
		{
			return this.objectProvider.CreateScope(name);
		}

		#endregion

		#region Implementation of IDisposable

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
		}

		#endregion
	}
}