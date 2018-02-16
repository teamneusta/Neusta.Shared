namespace Neusta.Shared.Logging.Adapter
{
	public sealed class GenericLogger<T> : ForwardingLogger<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericLogger{T}"/> class.
		/// </summary>
		public GenericLogger(ILogger targetLogger)
			: base(targetLogger)
		{
		}
	}
}