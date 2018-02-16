namespace Neusta.Shared.Core.DynamicCode
{
	using System.ComponentModel;
	using System.Reflection;
	using JetBrains.Annotations;

	/// <summary>
	/// Defines methods that dynamic property class has to implement.
	/// </summary>
	public interface IDynamicProperty : IDynamicValueMember
	{
		/// <summary>
		/// Gets the property info.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PropertyInfo PropertyInfo { get; }
	}
}