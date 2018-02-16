namespace Neusta.Shared.AspNetCore.Extensions.ContainerService
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider;

	internal sealed class ServiceProvider : IServiceProvider, ISupportRequiredService
	{
		private readonly IContainerAdapter containerAdapter;
		private readonly IObjectProvider objectProvider;

		/// <summary>
		/// Constructs a <see cref="ServiceProvider"/>
		/// </summary>
		public ServiceProvider(IContainerAdapter containerAdapter)
		{
			Guard.ArgumentNotNull(containerAdapter, nameof(containerAdapter));

			this.containerAdapter = containerAdapter;
			this.objectProvider = containerAdapter.ObjectProvider.CreateScope();
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

		#region Implementation of IServiceProvider

		/// <summary>
		/// Gets the service object of the specified type.
		/// </summary>
		public object GetService(Type serviceType)
		{
			return this.objectProvider.QueryInstance(serviceType);
		}

		#endregion

		#region Implementation of ISupportRequiredService

		/// <summary>
		/// Gets service of type <paramref name="serviceType" /> from the <see cref="IServiceProvider" /> implementing
		/// this interface.
		/// </summary>
		public object GetRequiredService(Type serviceType)
		{
			return this.objectProvider.GetInstance(serviceType);
		}

		#endregion
	}
}