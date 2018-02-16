namespace Neusta.Shared.Core.DynamicCode.Safe
{
	using System;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.Core.Utils;

	/// <summary>
	/// Safe wrapper for the dynamic method.
	/// </summary>
	internal class SafeMethod : IDynamicMethod
	{
		private readonly MethodInfo methodInfo;
		private readonly Func<object, object[], object> methodDelegate;
		private readonly Expression<Func<object, object[], object>> invokeExpression;

		/// <summary>
		/// Creates a new instance of the safe method wrapper.
		/// </summary>
		public SafeMethod(MethodInfo methodInfo)
		{
			Guard.ArgumentNotNull(methodInfo, "methodInfo");
			this.methodInfo = methodInfo;

			// Build the activator
			Type declaringType = methodInfo.DeclaringType;
			ParameterExpression instanceParameterExp = Expression.Parameter(typeof(object));
			ParameterExpression argumentsParameterExp = Expression.Parameter(typeof(object[]));
			ParameterInfo[] parameterInfos = methodInfo.GetParameters();
			Expression invokeExp;
			int cnt = parameterInfos.Length;
			if (cnt > 0)
			{
				var argumentsExpArray = new Expression[cnt];
				for (var idx = 0; idx < cnt; idx++)
				{
					Expression argumentExp = Expression.ArrayIndex(argumentsParameterExp, Expression.Constant(idx));
					Type parameterType = parameterInfos[idx].ParameterType;
					if (parameterType != typeof(object))
					{
						if (parameterType.GetTypeInfo().IsValueType)
						{
							argumentExp = Expression.Unbox(argumentExp, parameterType);
						}
						else
						{
							argumentExp = Expression.TypeAs(argumentExp, parameterType);
						}
					}
					argumentsExpArray[idx] = argumentExp;
				}
				if (methodInfo.IsStatic)
				{
					invokeExp = Expression.Call(methodInfo, argumentsExpArray);
				}
				else
				{
					UnaryExpression castExp = Expression.Convert(instanceParameterExp, declaringType);
					invokeExp = Expression.Call(castExp, methodInfo, argumentsExpArray);
				}
			}
			else
			{
				if (methodInfo.IsStatic)
				{
					invokeExp = Expression.Call(methodInfo);
				}
				else
				{
					invokeExp = Expression.Call(instanceParameterExp, this.methodInfo);
				}
			}
			this.invokeExpression = Expression.Lambda<Func<object, object[], object>>(invokeExp, instanceParameterExp, argumentsParameterExp);
			this.methodDelegate = this.invokeExpression.Compile();
		}

		/// <summary>
		/// Gets the method info.
		/// </summary>
		public MethodBase MethodInfo
		{
			[DebuggerStepThrough]
			get { return this.methodInfo; }
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
		public object Invoke() => this.methodDelegate(null, SafeUtils.EmptyArrayOfObject);

		/// <summary>
		/// Invokes dynamic method without parameters.
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