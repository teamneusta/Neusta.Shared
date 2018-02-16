namespace Neusta.Shared.Logging.Adapter.Null
{
	using System;
	using System.Collections.Concurrent;
	using Neusta.Shared.Logging.Base;

	internal sealed class NullLoggingAdapter : BaseLoggingAdapter
	{
		private readonly ConcurrentDictionary<Type, object> loggerMap = new ConcurrentDictionary<Type, object>();

		/// <summary>
		/// Initializes a new instance of the <see cref="NullLoggingAdapter"/> class.
		/// </summary>
		public NullLoggingAdapter(ILoggingConfiguration configuration)
			: base(configuration)
		{
		}

		#region Overrides of BaseLoggerAdapter

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override ILogger CreateLogger(string name)
		{
			return this.GetNullLogger();
		}

		/// <summary>
		/// Creates a new logger.
		/// </summary>
		protected override ILogger<T> CreateLogger<T>(string name)
		{
			return this.loggerMap.GetOrAdd(typeof(T), type => new GenericLogger<T>(this.GetNullLogger())) as ILogger<T>;
		}

		#endregion
	}
}