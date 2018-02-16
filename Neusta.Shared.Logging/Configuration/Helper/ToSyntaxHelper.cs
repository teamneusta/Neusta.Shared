namespace Neusta.Shared.Logging.Configuration.Helper
{
	using System;
	using Neusta.Shared.Logging.Configuration.Targets;

	internal sealed class ToSyntaxHelper : BaseSyntaxHelper<IToSyntaxResult>, IToSyntax
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToSyntaxHelper" /> class.
		/// </summary>
		public ToSyntaxHelper(ILoggingConfiguration configuration, IToSyntaxResult result)
			: base(configuration, result)
		{
		}

		#region Implementation of IToSyntax

		/// <summary>
		/// Adds a default Console target.
		/// </summary>
		public IToSyntaxResult Console(string layout = null)
		{
			return this.Console(cfg => cfg.Layout = layout);
		}

		/// <summary>
		/// Adds a configured Console target.
		/// </summary>
		public IToSyntaxResult Console(Action<ConsoleTarget> configure)
		{
			var target = new ConsoleTarget(@"console");
			configure(target);
			this.Configuration.Targets.Add(target);
			return this.Result;
		}

		/// <summary>
		/// Adds a default Debug target.
		/// </summary>
		public IToSyntaxResult Debugger(string layout = null)
		{
			return this.Debugger(cfg => cfg.Layout = layout);
		}

		/// <summary>
		/// Adds a configured Debug target.
		/// </summary>
		public IToSyntaxResult Debugger(Action<DebuggerTarget> configure)
		{
			var target = new DebuggerTarget(@"debug");
			configure(target);
			this.Configuration.Targets.Add(target);
			return this.Result;
		}

		/// <summary>
		/// Adds a default Logstash target.
		/// </summary>
		public IToSyntaxResult Logstash(string listenerAddress, string applicationName = null, string environmentName = null, string instanceName = null)
		{
			return this.Logstash(cfg =>
			{
				cfg.ListenerAddress = listenerAddress;
				cfg.ApplicationName = applicationName;
				cfg.EnvironmentName = environmentName;
				cfg.InstanceName = instanceName;
			});
		}

		/// <summary>
		/// Adds a configured Logstash target.
		/// </summary>
		public IToSyntaxResult Logstash(Action<LogstashTarget> configure)
		{
			var target = new LogstashTarget(@"logstash");
			configure(target);
			this.Configuration.Targets.Add(target);
			return this.Result;
		}

		/// <summary>
		/// Adds a default LoggingViewer target.
		/// </summary>
		public IToSyntaxResult Viewer()
		{
			return this.Viewer(cfg => { });
		}

		/// <summary>
		/// Adds a configured LoggingViewer target.
		/// </summary>
		public IToSyntaxResult Viewer(Action<ViewerTarget> configure)
		{
			var target = new ViewerTarget(@"viewer");
			configure(target);
			this.Configuration.Targets.Add(target);
			return this.Result;
		}

		/// <summary>
		/// Adds the specified target.
		/// </summary>
		public IToSyntaxResult Target(ILoggingTarget target)
		{
			this.Configuration.Targets.Add(target);
			return this.Result;
		}

		#endregion
	}
}