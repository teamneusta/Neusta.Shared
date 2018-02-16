namespace Neusta.Shared.Core.DynamicCode
{
	/// <summary>
	/// Defines methods that dynamic field class has to implement.
	/// </summary>
	public interface IDynamicField<TType, TValue> : IDynamicField, IDynamicValueMember<TType, TValue>
	{
	}
}