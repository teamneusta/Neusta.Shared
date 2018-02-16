namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using JetBrains.Annotations;

	public interface IDynamicConstructor<TType> : IDynamicConstructor
		where TType : class
	{
		/// <summary>
		/// Gets the invoke expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		new Expression<Func<object[], TType>> InvokeExpression { get; }

		/// <summary>
		/// Invokes a dynamic constructor without parameters.
		/// </summary>
		[PublicAPI]
		new TType Invoke();

		/// <summary>
		/// Invokes a dynamic constructor.
		/// </summary>
		[PublicAPI]
		new TType Invoke(params object[] arguments);
	}
}