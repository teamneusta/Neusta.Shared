namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;

	public interface IObjectProviderProvider : IFluentSyntax
	{
		/// <summary>
		/// Gets the <see cref="IObjectProvider"/>.
		/// </summary>
		[PublicAPI]
		IObjectProvider ObjectProvider { get; }
	}
}