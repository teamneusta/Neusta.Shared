namespace Neusta.Shared.Core.DynamicCode.Safe
{
	using System;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.Core.Utils;

	/// <summary>
	/// Safe wrapper for the dynamic constructor.
	/// </summary>
	internal class SafeConstructor : IDynamicConstructor
	{
		private readonly ConstructorInfo constructorInfo;
		private readonly Func<object[], object> constructorDelegate;
		private readonly Expression<Func<object[], object>> invokeExpression;

		/// <summary>
		/// Creates a new instance of the safe constructor wrapper.
		/// </summary>
		public SafeConstructor(ConstructorInfo constructorInfo)
		{
			Guard.ArgumentNotNull(constructorInfo, "constructorInfo");
			this.constructorInfo = constructorInfo;

			// Build the activator
			ParameterExpression argumentsParameterExp = Expression.Parameter(typeof(object[]));
			ParameterInfo[] parameterInfos = constructorInfo.GetParameters();
			Expression newExp;
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
				newExp = Expression.New(constructorInfo, argumentsExpArray);
			}
			else
			{
				newExp = Expression.New(constructorInfo);
			}
			this.invokeExpression = Expression.Lambda<Func<object[], object>>(newExp, argumentsParameterExp);
			this.constructorDelegate = this.invokeExpression.Compile();
		}

		/// <summary>
		/// Gets the constructor info.
		/// </summary>
		public ConstructorInfo ConstructorInfo
		{
			[DebuggerStepThrough]
			get { return this.constructorInfo; }
		}

		/// <summary>
		/// Gets the invoke expression.
		/// </summary>
		public Expression<Func<object[], object>> InvokeExpression
		{
			[DebuggerStepThrough]
			get { return this.invokeExpression; }
		}

		/// <summary>
		/// Invokes a dynamic constructor without parameters.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Invoke() => this.constructorDelegate(SafeUtils.EmptyArrayOfObject);

		/// <summary>
		/// Invokes dynamic constructor.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object Invoke(object[] arguments) => this.constructorDelegate(arguments);
	}
}