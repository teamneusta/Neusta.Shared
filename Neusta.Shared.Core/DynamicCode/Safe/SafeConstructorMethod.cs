namespace Neusta.Shared.Core.DynamicCode.Safe
{
	using System;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.Core.Utils;

	/// <summary>
	/// Safe wrapper for the dynamic constructor methods (that should be called as methods).
	/// </summary>
	internal sealed class SafeConstructorMethod : IDynamicMethod
	{
		private readonly ConstructorInfo constructorInfo;
		private readonly Func<object, object[], object> methodDelegate;
		private readonly Expression<Func<object, object[], object>> invokeExpression;

		/// <summary>
		/// Creates a new instance of the safe constructor method wrapper.
		/// </summary>
		public SafeConstructorMethod(ConstructorInfo constructorInfo)
		{
			Guard.ArgumentNotNull(constructorInfo, "constructorInfo");
			this.constructorInfo = constructorInfo;

			// Helper
			Expression<Func<ConstructorInfo, object, object[], object>> helperExpr = (ctor, instance, parms) => ctor.Invoke(instance, parms);
			MethodInfo invokeMethodInfo = ((MethodCallExpression)helperExpr.Body).Method;

			// Build the activator
			ParameterExpression instanceParameterExp = Expression.Parameter(typeof(object));
			ParameterExpression argumentsParameterExp = Expression.Parameter(typeof(object[]));
			ConstantExpression constructorExp = Expression.Constant(this.constructorInfo);
			Expression invokeExp = Expression.Call(constructorExp, invokeMethodInfo, new Expression[] {instanceParameterExp, argumentsParameterExp});
			this.invokeExpression = Expression.Lambda<Func<object, object[], object>>(invokeExp, instanceParameterExp, argumentsParameterExp);
			this.methodDelegate = this.invokeExpression.Compile();
		}

		/// <summary>
		/// Gets the method info.
		/// </summary>
		public MethodBase MethodInfo
		{
			[DebuggerStepThrough]
			get { return this.constructorInfo; }
		}

		/// <summary>
		/// Gets the invoke expression.
		/// </summary>
		public Expression<Func<object, object[], object>> InvokeExpression
		{
			[DebuggerStepThrough]
			get { return this.invokeExpression; }
		}


		/// <summary>
		/// Invokes dynamic static method without parameters.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Invoke() => throw new NotSupportedException();

		/// <summary>
		/// Invokes dynamic method on the specified target object without parameters.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Invoke(object target) => this.methodDelegate(target, SafeUtils.EmptyArrayOfObject);

		/// <summary>
		/// Invokes dynamic method.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Invoke(object target, object[] arguments) => this.methodDelegate(target, arguments);
	}
}