namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Reflection;
	using JetBrains.Annotations;

	/// <summary>
	/// Defines methods that dynamic method class has to implement.
	/// </summary>
	public interface IDynamicMethod
	{
		/// <summary>
		/// Gets the method info.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		MethodBase MethodInfo { get; }

		/// <summary>
		/// Gets the invoke expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Expression<Func<object, object[], object>> InvokeExpression { get; }

		/// <summary>
		/// Invokes dynamic static method object without parameters.
		/// </summary>
		[PublicAPI]
		object Invoke();

		/// <summary>
		/// Invokes dynamic method on the specified target object without parameters.
		/// </summary>
		[PublicAPI]
		object Invoke(object target);

		/// <summary>
		/// Invokes dynamic method on the specified target object.
		/// </summary>
		[PublicAPI]
		object Invoke(object target, object[] arguments);
	}
}