namespace Neusta.Shared.AspNetCore.Extensions.ContainerService
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider;

	[UsedImplicitly]
	internal sealed class ServiceScopeFactory : IServiceScopeFactory
	{
		private readonly IContainerAdapter containerAdapter;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceScopeFactory"/> class.
		/// </summary>
		public ServiceScopeFactory(IContainerAdapter containerAdapter)
		{
			Guard.ArgumentNotNull(containerAdapter, nameof(containerAdapter));

			this.containerAdapter = containerAdapter;
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

		#region Implementation of IServiceScopeFactory

		/// <summary>
		/// Create an <see cref="IServiceScope" /> which contains an <see cref="IServiceProvider" />
		/// used to resolve dependencies from a newly created scope.
		/// </summary>
		public IServiceScope CreateScope()
		{
			return new ServiceScope(this.containerAdapter);
		}

		#endregion
	}
}