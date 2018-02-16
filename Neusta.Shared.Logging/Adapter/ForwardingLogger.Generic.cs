namespace Neusta.Shared.Logging.Adapter
{
	using System;
	using System.Diagnostics;

	public class ForwardingLogger<T> : ForwardingLogger, ILogger<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ForwardingLogger{T}"/> class.
		/// </summary>
		public ForwardingLogger(ILogger targetLogger)
			: base(targetLogger)
		{
		}

		#region Implementation of ILogger<T>

		/// <summary>
		/// Gets the generic class.
		/// </summary>
		/// <value>The generic class.</value>
		public Type GenericClass
		{
			[DebuggerStepThrough]
			get { return typeof(T); }
		}

		#endregion
	}
}