namespace Neusta.Shared.Core.DynamicCode.Safe
{
	using System;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.Core.Utils;

	internal sealed class SafeMethod<TType> : SafeMethod, IDynamicMethod<TType>
	{
		private readonly Func<TType, object[], object> methodDelegate;
		private readonly Expression<Func<TType, object[], object>> invokeExpression;

		/// <summary>
		/// Creates a new instance of the safe method wrapper.
		/// </summary>
		public SafeMethod(MethodInfo methodInfo)
			: base(methodInfo)
		{
			Guard.ArgumentNotNull(methodInfo, "methodInfo");

			// Build the activator
			Type declaringType = methodInfo.DeclaringType;
			ParameterExpression instanceParameterExp = Expression.Parameter(typeof(TType));
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
					invokeExp = Expression.Call(instanceParameterExp, methodInfo);
				}
			}
			this.invokeExpression = Expression.Lambda<Func<TType, object[], object>>(invokeExp, instanceParameterExp, argumentsParameterExp);
			this.methodDelegate = this.invokeExpression.Compile();
		}

		/// <summary>
		/// Gets the invoke expression.
		/// </summary>
		public new Expression<Func<TType, object[], object>> InvokeExpression
		{
			[DebuggerStepThrough]
			get { return this.invokeExpression; }
		}

		/// <summary>
		/// Invokes dynamic method without parameters.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Invoke(TType target) => this.methodDelegate(target, SafeUtils.EmptyArrayOfObject);

		/// <summary>
		/// Invokes dynamic method.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Invoke(TType target, object[] arguments) => this.methodDelegate(target, arguments);
	}
}