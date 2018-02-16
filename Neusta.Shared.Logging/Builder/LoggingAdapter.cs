// ReSharper disable once CheckNamespace
namespace Neusta.Shared.Logging
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;
	using Neusta.Shared.Logging.Adapter.Debug;
	using Neusta.Shared.Logging.Adapter.Null;
	using Neusta.Shared.Logging.Configuration;

	public static class LoggingAdapter
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static Lazy<ILoggingAdapter> defaultAdapter = new Lazy<ILoggingAdapter>(InitializeDefaultLoggerAdapter);
		private static bool isRegistered;

		/// <summary>
		/// Gets the default adapter.
		/// </summary>
		[PublicAPI]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static ILoggingAdapter Default
		{
			[DebuggerStepThrough]
			get { return defaultAdapter.Value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether a logging adapter is set.
		/// </summary>
		[PublicAPI]
		public static bool IsRegistered
		{
			[DebuggerStepThrough]
			get { return isRegistered; }
		}

		/// <summary>
		/// Gets a value indicating whether the logging adapter is initialized.
		/// </summary>
		[PublicAPI]
		public static bool IsInitialized
		{
			get
			{
				if (defaultAdapter.IsValueCreated)
				{
					return defaultAdapter.Value.IsInitialized;
				}
				return false;
			}
		}

		/// <summary>
		/// Registers the given adapter as default.
		/// </summary>
		[PublicAPI]
		public static ILoggingAdapter Register(ILoggingAdapter adapter)
		{
			ILoggingAdapter oldAdapter = null;
			if (defaultAdapter.IsValueCreated)
			{
				oldAdapter = defaultAdapter.Value;
			}
			if (!ReferenceEquals(adapter, oldAdapter))
			{
				if (adapter == null)
				{
					defaultAdapter = new Lazy<ILoggingAdapter>(InitializeDefaultLoggerAdapter);
				}
				else
				{
					defaultAdapter = new Lazy<ILoggingAdapter>(() => adapter);
				}
				ILoggingAdapter newAdapter = defaultAdapter.Value;
				isRegistered = true;

				// Remap existing forwarding loggers to new adapter
				if (oldAdapter != null)
				{
					foreach (var kvp in oldAdapter.GetKnownLoggers())
					{
						if (kvp.Value is IForwardingLogger forwardingLogger)
						{
							forwardingLogger.TargetLogger = newAdapter.GetLogger(forwardingLogger.Name);
						}
					}
				}

				// Initialize the new adapter
				if (!newAdapter.IsInitialized)
				{
					newAdapter.Initialize();
				}

				return newAdapter;
			}
			else
			{
				return oldAdapter;
			}
		}

		#region Private Methods

		/// <summary>
		/// Initializes the logger adapter.
		/// </summary>
		private static ILoggingAdapter InitializeDefaultLoggerAdapter()
		{
			if (Debugger.IsAttached)
			{
				return new DebugLoggingAdapter(LoggingConfiguration.Empty);
			}
			else
			{
				return new NullLoggingAdapter(LoggingConfiguration.Empty);
			}
		}

		#endregion
	}
}