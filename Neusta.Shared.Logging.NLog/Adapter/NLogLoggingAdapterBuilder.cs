namespace Neusta.Shared.Logging.NLog.Adapter
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using global::NLog;
	using global::NLog.Config;
	using global::NLog.Layouts;
	using global::NLog.Targets;
	using global::NLog.Targets.Wrappers;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Base;
	using Neusta.Shared.Logging.Configuration.Targets;

	[UsedImplicitly]
	internal sealed class NLogLoggingAdapterBuilder : BaseLoggingAdapterBuilder
	{
		private const string DefaultLayout = @"${longdate} | ${level:uppercase=true:padding=-5} | ${logger:shortname=true:padding=32:fixedlength=true} | ${event-properties:item=RequestId:padding=22:fixedLength=true:alignmentOnTruncation=right} | ${message}${exception:format=toString}";

		private string configFileName;
		private Action<global::NLog.Config.LoggingConfiguration> configure;

		/// <summary>
		/// Initializes a new instance of the <see cref="NLogLoggingAdapterBuilder"/> class.
		/// </summary>
		public NLogLoggingAdapterBuilder()
		{
		}

		/// <summary>
		/// Gets or sets the name of the configuration file.
		/// </summary>
		public string ConfigFileName
		{
			[DebuggerStepThrough]
			get { return this.configFileName; }
			[DebuggerStepThrough]
			set { this.configFileName = value; }
		}

		/// <summary>
		/// Gets or sets the configuration action.
		/// </summary>
		public Action<global::NLog.Config.LoggingConfiguration> Configure
		{
			[DebuggerStepThrough]
			get { return this.configure; }
			[DebuggerStepThrough]
			set { this.configure = value; }
		}

		#region Overrides of BaseLoggerAdapterBuilder

		/// <summary>
		/// Builds the logger adapter.
		/// </summary>
		public override ILoggingAdapter Build(ILoggingConfiguration configuration)
		{
			// Create nlog configuration
			global::NLog.Config.LoggingConfiguration nlogConfig;
			string configFileName = this.configFileName ?? configuration.ConfigFileName;
			if (string.IsNullOrEmpty(configFileName))
			{
				nlogConfig = new global::NLog.Config.LoggingConfiguration();
			}
			else
			{
				nlogConfig = new XmlLoggingConfiguration(configFileName);
			}

			// Apply configuration settings
			ApplyConfigurationSettings(configuration, nlogConfig);

			// Additional configuration
			this.configure?.Invoke(nlogConfig);

			// Create the adapter
			var adapter = new NLogLoggingAdapter(configuration, nlogConfig);
			return adapter;
		}

		/// <summary>
		/// Registers the given adapter as default logger.
		/// </summary>
		public override void RegisterAsDefault(ILoggingConfiguration configuration, ILoggingAdapter adapter)
		{
			base.RegisterAsDefault(configuration, adapter);

			var globalThreshold = configuration.MinimumLogLevel.Translate();
			global::NLog.LogManager.GlobalThreshold = globalThreshold;
			global::NLog.LogManager.Configuration = ((NLogLoggingAdapter)adapter).Configuration;
		}

		#endregion

		#region Private Methods

		private static void ApplyConfigurationSettings(ILoggingConfiguration configuration, LoggingConfiguration nlogConfig)
		{
			// Default target
			var defaultTarget = new SplitGroupTarget();
			defaultTarget.Name = "__default";
			nlogConfig.AddTarget("default", defaultTarget);

			// Rules
			var globalThreshold = configuration.MinimumLogLevel.Translate();
			foreach (var rule in configuration.Rules)
			{
				var minLevel = rule.MinLevel.Translate();
				if (minLevel < globalThreshold)
				{
					minLevel = globalThreshold;
				}

				var maxLevel = rule.MaxLevel.Translate();
				if (maxLevel >= minLevel)
				{
					string targetName = rule.TargetName;
					if (string.IsNullOrWhiteSpace(targetName))
					{
						targetName = @"__default";
					}

					string loggerNamePattern = rule.LoggerNamePattern;
					if (string.IsNullOrWhiteSpace(loggerNamePattern))
					{
						loggerNamePattern = @"*";
					}

					var target = nlogConfig.FindTargetByName(targetName);
					if (target != null)
					{
						nlogConfig.AddRule(minLevel, maxLevel, target, loggerNamePattern, rule.Final);
					}
				}
			}

			if (!nlogConfig.LoggingRules.Any())
			{
				nlogConfig.AddRule(global::NLog.LogLevel.Trace, global::NLog.LogLevel.Fatal, defaultTarget);
			}

			// Apply the global threshold to all rules
			if (globalThreshold > global::NLog.LogLevel.Trace)
			{
				var disableLevels = LogLevel.AllLoggingLevels.Where(level => level < globalThreshold).ToArray();
				foreach (var rule in nlogConfig.LoggingRules)
				{
					foreach (var level in disableLevels)
					{
						rule.DisableLoggingForLevel(level);
					}
				}
			}

			// Targets
			bool hasConsole = false;
			bool hasDebug = false;
			foreach (var target in configuration.Targets)
			{
				if (target is global::Neusta.Shared.Logging.Configuration.Targets.ConsoleTarget consoleTarget)
				{
					defaultTarget.Targets.Add(CreateConsoleTarget(consoleTarget));
					hasConsole = true;
				}
				else if (target is global::Neusta.Shared.Logging.Configuration.Targets.DebuggerTarget debuggerTarget)
				{
					defaultTarget.Targets.Add(CreateDebuggerTarget(debuggerTarget));
					hasDebug = true;
				}
				else if (target is global::Neusta.Shared.Logging.Configuration.Targets.LogstashTarget logstashTarget)
				{
					defaultTarget.Targets.Add(CreateLogstashTarget(logstashTarget));
				}
			}

			// Debugger
			if (Debugger.IsAttached)
			{
				if (!hasDebug)
				{
					var debugTarget = new global::NLog.Targets.DebuggerTarget();
					debugTarget.Name = "__debug";
					debugTarget.Layout = DefaultLayout;
					defaultTarget.Targets.Add(debugTarget);
				}

				if (!hasConsole)
				{
					var consoleTarget = new ColoredConsoleTarget();
					consoleTarget.Name = "__console";
					consoleTarget.Layout = DefaultLayout;
					defaultTarget.Targets.Add(consoleTarget);
				}
			}
		}

		private static Target CreateConsoleTarget(global::Neusta.Shared.Logging.Configuration.Targets.ConsoleTarget consoleTarget)
		{
			var target = new ColoredConsoleTarget();
			target.Name = consoleTarget.Name;
			if (string.IsNullOrEmpty(target.Name))
			{
				target.Name = "__console";
			}
			if (string.IsNullOrWhiteSpace(consoleTarget.Layout))
			{
				target.Layout = DefaultLayout;
			}
			else
			{
				target.Layout = consoleTarget.Layout;
			}
			return target;
		}

		private static Target CreateDebuggerTarget(global::Neusta.Shared.Logging.Configuration.Targets.DebuggerTarget debuggerTarget)
		{
			var target = new global::NLog.Targets.DebuggerTarget();
			target.Name = debuggerTarget.Name;
			if (string.IsNullOrEmpty(target.Name))
			{
				target.Name = "__debug";
			}
			if (string.IsNullOrWhiteSpace(debuggerTarget.Layout))
			{
				target.Layout = DefaultLayout;
			}
			else
			{
				target.Layout = debuggerTarget.Layout;
			}
			return target;
		}

		private static Target CreateLogstashTarget(LogstashTarget logstashTarget)
		{
			var jsonLayout = new JsonLayout();
			jsonLayout.Attributes.Add(new JsonAttribute("@timestamp", Layout.FromString("${date:universalTime=true:format=yyyy-MM-ddTHH\\:mm\\:ss.fffZ}")));
			jsonLayout.Attributes.Add(new JsonAttribute("@sequence", Layout.FromString("${sequenceid}")));
			jsonLayout.Attributes.Add(new JsonAttribute("level", Layout.FromString("${level:upperCase=true}")));
			jsonLayout.Attributes.Add(new JsonAttribute("category", Layout.FromString("${logger}"), true));
			jsonLayout.Attributes.Add(new JsonAttribute("message", Layout.FromString("${message:withException=false}"), true));
			jsonLayout.Attributes.Add(new JsonAttribute("exception", Layout.FromString("${exception:format=toString}"), true));
			if (!string.IsNullOrWhiteSpace(logstashTarget.ApplicationName))
			{
				jsonLayout.Attributes.Add(new JsonAttribute("application", Layout.FromString(logstashTarget.ApplicationName), true));
			}
			if (!string.IsNullOrWhiteSpace(logstashTarget.EnvironmentName))
			{
				jsonLayout.Attributes.Add(new JsonAttribute("environment", Layout.FromString(logstashTarget.EnvironmentName), true));
			}
			jsonLayout.Attributes.Add(new JsonAttribute("machinename", Layout.FromString("${machinename}"), true));
			jsonLayout.Attributes.Add(new JsonAttribute("processname", Layout.FromString("${processname}"), true));
			if (!string.IsNullOrWhiteSpace(logstashTarget.InstanceName))
			{
				jsonLayout.Attributes.Add(new JsonAttribute("instance", Layout.FromString(logstashTarget.InstanceName), true));
			}
			jsonLayout.RenderEmptyObject = false;

			string name = logstashTarget.Name;
			if (string.IsNullOrEmpty(name))
			{
				name = "__logstash";
			}

			var target = new NetworkTarget(name + "-network")
			{
				Address = Layout.FromString(logstashTarget.ListenerAddress),
				KeepConnection = true,
				Layout = jsonLayout,
				Encoding = Encoding.UTF8
			};

			var wrapper = new BufferingTargetWrapper(name, target)
			{
				BufferSize = 250,
				FlushTimeout = 250,
				OptimizeBufferReuse = true,
				OverflowAction = BufferingTargetWrapperOverflowAction.Flush
			};

			return wrapper;
		}

		#endregion
	}
}