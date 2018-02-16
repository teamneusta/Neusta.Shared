namespace Neusta.Shared.Core.DynamicCode
{
	/// <summary>
	/// Defines methods that dynamic property class has to implement.
	/// </summary>
	public interface IDynamicProperty<TType, TValue> : IDynamicProperty, IDynamicValueMember<TType, TValue>
	{
	}
}