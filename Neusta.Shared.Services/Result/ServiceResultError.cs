namespace Neusta.Shared.Services.Result
{
	using System;
	using System.Diagnostics;

	internal class ServiceResultError : IServiceResultError
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string message;
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly Exception exception;

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResultError"/> class.
		/// </summary>
		public ServiceResultError(string message)
		{
			this.message = message;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResultError"/> class.
		/// </summary>
		public ServiceResultError(Exception exception)
		{
			this.message = exception.Message;
			this.exception = exception;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceResultError"/> class.
		/// </summary>
		public ServiceResultError(string message, Exception exception)
		{
			this.message = message;
			this.exception = exception;
		}

		#region Implementation of IServiceResultError

		/// <summary>
		/// Gets the message.
		/// </summary>
		public string Message
		{
			[DebuggerStepThrough]
			get { return this.message; }
		}

		/// <summary>
		/// Gets the exception.
		/// </summary>
		public Exception Exception
		{
			[DebuggerStepThrough]
			get { return this.exception; }
		}

		#endregion
	}
}