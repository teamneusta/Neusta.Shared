namespace Neusta.Shared.DataAccess
{
	using JetBrains.Annotations;

	public interface IPropertyChange
	{
		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		[PublicAPI]
		string PropertyName { get; }

		/// <summary>
		/// Gets the original value.
		/// </summary>
		[PublicAPI]
		object OriginalValue { get; }

		/// <summary>
		/// Gets the current value.
		/// </summary>
		[PublicAPI]
		object CurrentValue { get; }
	}
}