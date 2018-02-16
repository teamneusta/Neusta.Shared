namespace Neusta.Shared.Logging
{
	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	public interface ILoggingConfiguration
	{
		/// <summary>
		/// Gets or sets the name of the configuration file.
		/// </summary>
		[PublicAPI]
		string ConfigFileName { get; set; }

		/// <summary>
		/// Gets or sets the the global log threshold.
		/// </summary>
		[PublicAPI]
		LogLevel MinimumLogLevel { get; set; }

		/// <summary>
		/// Gets the filter rules.
		/// </summary>
		[PublicAPI]
		ICollection<IFilterRule> Filters { get; }

		/// <summary>
		/// Gets the logging rules.
		/// </summary>
		[PublicAPI]
		ICollection<ILoggingRule> Rules { get; }

		/// <summary>
		/// Gets the logging targets.
		/// </summary>
		[PublicAPI]
		ICollection<ILoggingTarget> Targets { get; }
	}
}