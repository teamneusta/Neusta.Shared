namespace Neusta.Shared.Logging.NLog.Adapter
{
	using System;
	using System.Diagnostics;
	using global::NLog;

	internal class NLogLogger<T> : NLogLogger, ILogger<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NLogLogger{T}"/> class.
		/// </summary>
		public NLogLogger(NLogLoggingAdapter adapter, ILogger nlogLogger, string name)
			: base(adapter, nlogLogger, name)
		{
		}

		#region Implementation of ILogger<T>

		/// <summary>
		/// Gets the generic class.
		/// </summary>
		public Type GenericClass
		{
			[DebuggerStepThrough]
			get { return typeof(T); }
		}

		#endregion
	}
}