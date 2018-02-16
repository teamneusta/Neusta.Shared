namespace Neusta.Shared.Services
{
	using JetBrains.Annotations;

	public interface IServiceResult<TData> : IServiceResult
	{
		/// <summary>
		/// Gets the data.
		/// </summary>
		[PublicAPI]
		TData Data { get; }
	}
}