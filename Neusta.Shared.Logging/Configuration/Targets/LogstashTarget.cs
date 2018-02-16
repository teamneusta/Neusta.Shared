namespace Neusta.Shared.Logging.Configuration.Targets
{
	using System.Diagnostics;

	public sealed class LogstashTarget : ILoggingTarget
	{
		private readonly string name;
		private string listenerAddress;
		private string applicationName;
		private string environmentName;
		private string instanceName;

		/// <summary>
		/// Initializes a new instance of the <see cref="LogstashTarget"/> class.
		/// </summary>
		internal LogstashTarget(string name)
		{
			this.name = name;
		}

		#region Implementation of ILoggingTarget

		/// <summary>
		/// Gets the target name.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get { return this.name; }
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the listener address.
		/// </summary>
		public string ListenerAddress
		{
			[DebuggerStepThrough]
			get { return this.listenerAddress; }
			[DebuggerStepThrough]
			set { this.listenerAddress = value; }
		}

		/// <summary>
		/// Gets or sets the name of the application.
		/// </summary>
		public string ApplicationName
		{
			[DebuggerStepThrough]
			get { return this.applicationName; }
			[DebuggerStepThrough]
			set { this.applicationName = value; }
		}

		/// <summary>
		/// Gets or sets the name of the environment.
		/// </summary>
		public string EnvironmentName
		{
			[DebuggerStepThrough]
			get { return this.environmentName; }
			[DebuggerStepThrough]
			set { this.environmentName = value; }
		}

		/// <summary>
		/// Gets or sets the name of the instance.
		/// </summary>
		public string InstanceName
		{
			[DebuggerStepThrough]
			get { return this.instanceName; }
			[DebuggerStepThrough]
			set { this.instanceName = value; }
		}

		#endregion
	}
}