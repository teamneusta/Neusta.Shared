namespace Neusta.Shared.Core.DynamicCode
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Reflection;
	using JetBrains.Annotations;

	/// <summary>
	/// Defines constructors that dynamic constructor class has to implement.
	/// </summary>
	public interface IDynamicConstructor
	{
		/// <summary>
		/// Gets the property info.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		ConstructorInfo ConstructorInfo { get; }

		/// <summary>
		/// Gets the invoke expression.
		/// </summary>
		[PublicAPI]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		Expression<Func<object[], object>> InvokeExpression { get; }

		/// <summary>
		/// Invokes a dynamic constructor without parameters.
		/// </summary>
		[PublicAPI]
		object Invoke();

		/// <summary>
		/// Invokes a dynamic constructor.
		/// </summary>
		[PublicAPI]
		object Invoke(params object[] arguments);
	}
}