// ReSharper disable once CheckNamespace
namespace Neusta.Shared.Logging
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Adapter.Multi;
	using Neusta.Shared.Logging.Adapter.Null;
	using Neusta.Shared.Logging.Configuration;

	public sealed class LoggingBuilder : ILoggingBuilder
	{
		private static readonly ConcurrentDictionary<string, ILoggingConfiguration> fileDataMap = new ConcurrentDictionary<string, ILoggingConfiguration>();
		private static ILoggingConfiguration defaultConfiguration;

		private ILoggingConfiguration configuration;
		private readonly Dictionary<Type, ILoggingAdapterBuilder> adapterBuilders;

		/// <summary>
		/// Creates a new, default instance of the <see cref="LoggingBuilder"/> class.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder Default()
		{
			ILoggingConfiguration configuration = Volatile.Read(ref defaultConfiguration);
			if (configuration == null)
			{
				configuration = new LoggingConfiguration();
				Interlocked.CompareExchange(ref defaultConfiguration, configuration, null);
				configuration = Volatile.Read(ref defaultConfiguration);
			}
			return new LoggingBuilder(configuration);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="LoggingBuilder"/> class that references to the given
		/// configuration file name.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder From(ILoggingConfiguration configuration)
		{
			return new LoggingBuilder(configuration);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="LoggingBuilder"/> class that references to the given
		/// configuration file name.
		/// </summary>
		[PublicAPI]
		public static ILoggingBuilder FromConfigurationFile(string configFileName)
		{
			if (!File.Exists(configFileName))
			{
				throw new FileNotFoundException("Configuration file not found", configFileName);
			}
			configFileName = Path.GetFullPath(configFileName);
			ILoggingConfiguration configuration = Volatile.Read(ref defaultConfiguration);
			if (configuration != null)
			{
				if (!string.Equals(configuration.ConfigFileName, configFileName))
				{
					configuration = null;
				}
			}
			if (configuration == null)
			{
				configuration = fileDataMap.GetOrAdd(configFileName, innerFileName => new LoggingConfiguration(innerFileName));
			}
			return new LoggingBuilder(configuration);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LoggingBuilder"/> class.
		/// </summary>
		private LoggingBuilder(ILoggingConfiguration configuration)
		{
			this.configuration = configuration;
			this.adapterBuilders = new Dictionary<Type, ILoggingAdapterBuilder>();
		}

		#region Implementation of ILoggingBuilderBase

		/// <summary>
		/// Gets the logging builder configuration data.
		/// </summary>
		public ILoggingConfiguration Configuration
		{
			[DebuggerStepThrough]
			get { return this.configuration; }
		}

		/// <summary>
		/// Updates the configuration.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void UpdateConfiguration(ILoggingConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration));
			}
			this.configuration = configuration;
		}

		/// <summary>
		/// Gets the logging adapter builders.
		/// </summary>
		public IEnumerable<ILoggingAdapterBuilder> AdapterBuilders
		{
			[DebuggerStepThrough]
			get { return this.adapterBuilders.Values; }
		}

		/// <summary>
		/// Registers the given adapter builder.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public void RegisterAdapterBuilder(ILoggingAdapterBuilder adapterBuilder)
		{
			if (adapterBuilder == null)
			{
				throw new ArgumentNullException(nameof(adapterBuilder));
			}
			this.adapterBuilders[adapterBuilder.GetType()] = adapterBuilder;
		}

		/// <summary>
		/// Registers the given adapter builder.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public TBuilder RegisterAdapterBuilder<TBuilder>()
			where TBuilder : class, ILoggingAdapterBuilder, new()
		{
			var builder = new TBuilder();
			this.adapterBuilders[typeof(TBuilder)] = builder;
			return builder;
		}

		#endregion

		#region Implementation of ILoggingBuilder

		/// <summary>
		/// Builds the logging adapter using the previously registered builders.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public ILoggingAdapter Build()
		{
			return this.InternalBuild(!LoggingAdapter.IsRegistered);
		}

		/// <summary>
		/// Builds the logging adapter using the previously registered builders.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public ILoggingAdapter BuildAsDefault()
		{
			ILoggingAdapter adapter = this.InternalBuild(true);
			Interlocked.Exchange(ref defaultConfiguration, this.Configuration);
			return adapter;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Builds the logging adapter using the previously registered builders.
		/// </summary>
		private ILoggingAdapter InternalBuild(bool registerAsDefault)
		{
			ILoggingAdapter adapter;
			ILoggingAdapterBuilder[] adapterBuilders = this.adapterBuilders.Values.ToArray();
			int count = adapterBuilders.Length;
			if (count == 1)
			{
				adapter = adapterBuilders[0].Build(this.configuration);
				if (registerAsDefault)
				{
					adapterBuilders[0].RegisterAsDefault(this.configuration, adapter);
				}
			}
			else if (count == 0)
			{
				adapter = new NullLoggingAdapter(this.configuration);
			}
			else
			{
				var adapters = new ILoggingAdapter[count];
				for (int idx = 0; idx < count; idx++)
				{
					adapters[idx] = adapterBuilders[idx].Build(this.configuration);
				}
				adapter = new MultiLoggingAdapter(this.configuration, adapters);
				if (registerAsDefault)
				{
					for (int idx = 0; idx < count; idx++)
					{
						adapterBuilders[idx].RegisterAsDefault(this.configuration, adapters[idx]);
					}
				}
			}
			if (registerAsDefault)
			{
				LoggingAdapter.Register(adapter);
			}
			return adapter;
		}

		#endregion
	}
}
