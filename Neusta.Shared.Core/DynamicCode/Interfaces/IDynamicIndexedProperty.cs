namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Reflection;
	using JetBrains.Annotations;

	/// <summary>
	/// Defines methods that dynamic indexer class has to implement.
	/// </summary>
	public interface IDynamicIndexedProperty
	{
		/// <summary>
		/// Gets the property info.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PropertyInfo PropertyInfo { get; }

		/// <summary>
		/// Gets the getter expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Expression<Func<object, object[], object>> GetterExpression { get; }

		/// <summary>
		/// Gets the setter expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Expression<Action<object, object, object[]>> SetterExpression { get; }

		/// <summary>
		/// Gets the value of the dynamic property for the specified target object.
		/// </summary>
		[PublicAPI]
		object GetValue(object target, object[] index);

		/// <summary>
		/// Gets the value of the dynamic property for the specified target object.
		/// </summary>
		[PublicAPI]
		void SetValue(object target, object value, object[] index);
	}
}