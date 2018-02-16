namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	/// Defines methods that dynamic value member class (field or property) has to implement.
	/// </summary>
	public interface IDynamicValueMember
	{
		/// <summary>
		/// Gets the getter expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Expression<Func<object, object>> GetterExpression { get; }

		/// <summary>
		/// Gets the setter expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Expression<Action<object, object>> SetterExpression { get; }

		/// <summary>
		/// Gets the value of the dynamic field for the specified target object.
		/// </summary>
		[PublicAPI]
		object GetValue(object target);

		/// <summary>
		/// Gets the value of the dynamic field for the specified target object.
		/// </summary>
		[PublicAPI]
		void SetValue(object target, object value);
	}
}