namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	/// <summary>
	/// Defines methods that dynamic value member class (field or property) has to implement.
	/// </summary>
	public interface IDynamicValueMember<TType, TValue> : IDynamicValueMember
	{
		/// <summary>
		/// Gets the getter expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		new Expression<Func<TType, TValue>> GetterExpression { get; }

		/// <summary>
		/// Gets the setter expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		new Expression<Action<TType, TValue>> SetterExpression { get; }

		/// <summary>
		/// Gets the value of the dynamic field for the specified target object.
		/// </summary>
		[PublicAPI]
		TValue GetValue(TType target);

		/// <summary>
		/// Gets the value of the dynamic field for the specified target object.
		/// </summary>
		[PublicAPI]
		void SetValue(TType target, TValue value);
	}
}