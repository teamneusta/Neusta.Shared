namespace Neusta.Shared.Logging
{
	using System;
	using System.ComponentModel;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Configuration.Targets;

	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IToSyntax : IFluentSyntax
	{
		/// <summary>
		/// Adds a default Console target.
		/// </summary>
		[PublicAPI]
		IToSyntaxResult Console(string layout = null);

		/// <summary>
		/// Adds a configured Console target.
		/// </summary>
		[PublicAPI]
		IToSyntaxResult Console(Action<ConsoleTarget> configure);

		/// <summary>
		/// Adds a default Debug target.
		/// </summary>
		[PublicAPI]
		IToSyntaxResult Debugger(string layout = null);

		/// <summary>
		/// Adds a configured Debug target.
		/// </summary>
		[PublicAPI]
		IToSyntaxResult Debugger(Action<DebuggerTarget> configure);

		/// <summary>
		/// Adds a default Logstash target.
		/// </summary>
		[PublicAPI]
		IToSyntaxResult Logstash(string listenerAddress, string applicationName = null, string environmentName = null, string instanceName = null);

		/// <summary>
		/// Adds a configured Logstash target.
		/// </summary>
		[PublicAPI]
		IToSyntaxResult Logstash(Action<LogstashTarget> configure);

		/// <summary>
		/// Adds a default LoggingViewer target.
		/// </summary>
		[PublicAPI]
		IToSyntaxResult Viewer();

		/// <summary>
		/// Adds a configured LoggingViewer target.
		/// </summary>
		[PublicAPI]
		IToSyntaxResult Viewer(Action<ViewerTarget> configure);

		/// <summary>
		/// Adds the specified target.
		/// </summary>
		[PublicAPI]
		IToSyntaxResult Target(ILoggingTarget target);
	}
}