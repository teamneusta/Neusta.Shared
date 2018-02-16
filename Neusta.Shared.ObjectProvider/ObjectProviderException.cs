namespace Neusta.Shared.ObjectProvider
{
	using System;

	public class ObjectProviderException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectProviderException"/> class.
		/// </summary>
		public ObjectProviderException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectProviderException"/> class.
		/// </summary>
		public ObjectProviderException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ObjectProviderException"/> class.
		/// </summary>
		public ObjectProviderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}