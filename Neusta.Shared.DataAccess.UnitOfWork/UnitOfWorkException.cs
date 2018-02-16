namespace Neusta.Shared.DataAccess.UnitOfWork
{
	using System;

	public class UnitOfWorkException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWorkException"/> class.
		/// </summary>
		public UnitOfWorkException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UnitOfWorkException"/> class.
		/// </summary>
		public UnitOfWorkException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}