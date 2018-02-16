namespace Neusta.Shared.Core.DynamicCode
{
	using System.ComponentModel;
	using System.Reflection;
	using JetBrains.Annotations;

	/// <summary>
	/// Defines methods that dynamic field class has to implement.
	/// </summary>
	public interface IDynamicField : IDynamicValueMember
	{
		/// <summary>
		/// Gets the field info.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		FieldInfo FieldInfo { get; }
	}
}