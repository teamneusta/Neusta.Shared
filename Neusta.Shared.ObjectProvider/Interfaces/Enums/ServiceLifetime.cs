namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;

	[PublicAPI]
	public enum ServiceLifetime
	{
		/// <summary>
		/// Specifies that a new instance of the service will be created every time it is requested.
		/// </summary>
		Transient,

		/// <summary>
		/// Specifies that a new instance of the service will be created for each resolution request.
		/// </summary>
		PerResolutionRequest,

		/// <summary>
		/// Specifies that a new instance of the service will be created for each scope.
		/// </summary>
		Scoped,

		/// <summary>
		/// Specifies that a single instance of the service will be created.
		/// </summary>
		Singleton,
	}
}