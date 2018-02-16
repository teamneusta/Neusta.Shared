// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using CommonServiceLocator;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.Logging;
	using Neusta.Shared.ObjectProvider.Configuration;
	using Neusta.Shared.ObjectProvider.Configuration.Helper;

	public class ContainerBuilder : IContainerBuilder
	{
		private static readonly ILogger logger = LogManager.GetLogger<ContainerBuilder>();

		private IContainerConfiguration configuration;
		private IContainerAdapterBuilder adapterBuilder;

		/// <summary>
		/// Gets a new <see cref="ContainerBuilder"/>.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder Create()
		{
			return new ContainerBuilder(new ContainerConfiguration());
		}

		/// <summary>
		/// Gets a new <see cref="ContainerBuilder"/> for the specified main <see cref="Assembly" />.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder For<T>()
		{
			return new ContainerBuilder(new ContainerConfiguration(typeof(T).GetTypeInfo().Assembly));
		}

		/// <summary>
		/// Gets a new <see cref="ContainerBuilder"/> for the specified main object instance.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder For(object instance)
		{
			Assembly assembly = null;
			if (instance != null)
			{
				if (instance is Assembly instanceAssembly)
				{
					assembly = instanceAssembly;
				}
				else if (instance is TypeInfo typeInfo)
				{
					assembly = typeInfo.Assembly;
				}
				else if (instance is Type type)
				{
					assembly = type.GetTypeInfo().Assembly;
				}
				else if (instance != null)
				{
					assembly = instance.GetType().GetTypeInfo().Assembly;
				}
			}
			return new ContainerBuilder(new ContainerConfiguration(assembly));
		}


		/// <summary>
		/// Creates a new <see cref="ContainerBuilder"/> for the specified configuration.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder From(IContainerConfiguration configuration)
		{
			return new ContainerBuilder(configuration);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerBuilder"/> class.
		/// </summary>
		private ContainerBuilder(IContainerConfiguration configuration)
		{
			this.configuration = configuration;
		}

		#region Implementation of IContainerBuilderBase

		/// <summary>
		/// Gets the container builder configuration data.
		/// </summary>
		public IContainerConfiguration Configuration
		{
			[DebuggerStepThrough]
			get { return this.configuration; }
		}

		/// <summary>
		/// Updates the configuration.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void UpdateConfiguration(IContainerConfiguration configuration)
		{
			Guard.ArgumentNotNull(configuration, nameof(configuration));
			this.configuration = configuration;
		}

		/// <summary>
		/// Gets the container adapter builder.
		/// </summary>
		public IContainerAdapterBuilder AdapterBuilder
		{
			[DebuggerStepThrough]
			get { return this.adapterBuilder; }
		}

		/// <summary>
		/// Registers the given container adapter builder.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void RegisterAdapterBuilder(IContainerAdapterBuilder adapterBuilder)
		{
			Guard.ArgumentNotNull(adapterBuilder, nameof(adapterBuilder));
			this.adapterBuilder = adapterBuilder;
		}

		/// <summary>
		/// Registers the given container adapter builder.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public TBuilder RegisterAdapterBuilder<TBuilder>()
			where TBuilder : class, IContainerAdapterBuilder, new()
		{
			var builder = new TBuilder();
			this.adapterBuilder = builder;
			return builder;
		}

		#endregion

		#region Implementation of IContainerBuilder

		/// <summary>
		/// Builds the object provider using the previously registered builders.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IContainerAdapter Build()
		{
			return this.InternalBuild(!ServiceLocator.IsLocationProviderSet);
		}

		/// <summary>
		/// Builds the object provider using the previously registered builder.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IContainerAdapter BuildAsDefault()
		{
			return this.InternalBuild(true);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Builds the object provider adapter using the previously registered builders.
		/// </summary>
		private IContainerAdapter InternalBuild(bool registerAsDefault)
		{
			if (this.adapterBuilder == null)
			{
				logger.Warn("Cannot build logger adapter as no builder was set.");
				return null;
			}
			else
			{
				logger.Trace("Building logger adapter using {0}", this.adapterBuilder.GetType());

				// Check for service registrations
				if (!this.configuration.ServiceDescriptors.Any() && (this.configuration.ApplicationAssembly != null))
				{
					var helper = new RootSyntaxHelper(this.configuration);
					helper.Scan(cfg => cfg
						.FromApplicationAssembly()
						.AddClasses(false)
						.AsSelfWithInterfaces());
				}

				// Build the adapter
				IContainerAdapter adapter = this.adapterBuilder.Build(this.configuration);
				if (registerAsDefault && (adapter != null))
				{
					this.adapterBuilder.RegisterAsDefault(this.configuration, adapter);
				}
				return adapter;
			}
		}

		#endregion
	}
}
