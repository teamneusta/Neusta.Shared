namespace Neusta.Shared.AspNetCore.Extensions.ContainerService
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Neusta.Shared.Core.DisposableObjects;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider;

	internal sealed class ServiceScope : DisposableObject, IServiceScope, IServiceProvider, ISupportRequiredService
	{
		private readonly IContainerAdapter containerAdapter;
		private readonly IObjectProviderScope childScope;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceScope"/> class.
		/// </summary>
		public ServiceScope(IContainerAdapter containerAdapter)
		{
			Guard.ArgumentNotNull(containerAdapter, nameof(containerAdapter));

			this.containerAdapter = containerAdapter;
			this.childScope = containerAdapter.ObjectProvider.CreateScope();
		}

		/// <summary>
		/// Gets the <see cref="IContainerAdapter" />.
		/// </summary>
		[PublicAPI]
		public IContainerAdapter ContainerAdapter
		{
			[DebuggerStepThrough]
			get { return this.containerAdapter; }
		}

		/// <summary>
		/// Gets the scope.
		/// </summary>
		[PublicAPI]
		public IObjectProviderScope Scope
		{
			[DebuggerStepThrough]
			get { return this.childScope; }
		}

		#region Implementation of IServiceScope

		/// <summary>
		/// The <see cref="IServiceProvider" /> used to resolve dependencies from the scope.
		/// </summary>
		public IServiceProvider ServiceProvider
		{
			[DebuggerStepThrough]
			get { return this.childScope; }
		}

		#endregion

		#region Implementation of IServiceProvider

		/// <summary>
		/// Gets the service object of the specified type.
		/// </summary>
		public object GetService(Type serviceType)
		{
			this.CheckIsDisposed();
			return this.childScope.QueryInstance(serviceType);
		}

		#endregion

		#region Implementation of ISupportRequiredService

		/// <summary>
		/// Gets service of type <paramref name="serviceType" /> from the <see cref="IServiceProvider" /> implementing
		/// this interface.
		/// </summary>
		public object GetRequiredService(Type serviceType)
		{
			this.CheckIsDisposed();
			return this.childScope.GetInstance(serviceType);
		}

		#endregion

		#region Overrides of DisposableObject

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.childScope.Dispose();
		}

		#endregion
	}
}