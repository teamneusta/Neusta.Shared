namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	public interface IDynamicMethod<TType> : IDynamicMethod
	{
		/// <summary>
		/// Gets the invoke expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		new Expression<Func<TType, object[], object>> InvokeExpression { get; }

		/// <summary>
		/// Invokes dynamic method on the specified target object with parameters.
		/// </summary>
		[PublicAPI]
		object Invoke(TType target);

		/// <summary>
		/// Invokes dynamic method on the specified target object.
		/// </summary>
		[PublicAPI]
		object Invoke(TType target, object[] arguments);
	}
}