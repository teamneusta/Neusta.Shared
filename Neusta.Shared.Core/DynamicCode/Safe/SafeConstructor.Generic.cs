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
	internal sealed class SafeConstructor<TType> : SafeConstructor, IDynamicConstructor<TType>
		where TType : class
	{
		private readonly Func<object[], TType> constructorDelegate;
		private readonly Expression<Func<object[], TType>> invokeExpression;
		private readonly Expression<Func<object[], object>> untypedInvokeExpression;

		/// <summary>
		/// Creates a new instance of the safe constructor wrapper.
		/// </summary>
		public SafeConstructor(ConstructorInfo constructorInfo)
			: base(constructorInfo)
		{
			Guard.ArgumentNotNull(constructorInfo, "constructorInfo");

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
			this.invokeExpression = Expression.Lambda<Func<object[], TType>>(newExp, argumentsParameterExp);
			Expression untypedNewExp = Expression.TypeAs(newExp, typeof(object));
			this.untypedInvokeExpression = Expression.Lambda<Func<object[], object>>(untypedNewExp, argumentsParameterExp);
			this.constructorDelegate = this.invokeExpression.Compile();
		}

		/// <summary>
		/// Gets the invoke expression.
		/// </summary>
		public new Expression<Func<object[], TType>> InvokeExpression
		{
			[DebuggerStepThrough]
			get { return this.invokeExpression; }
		}

		/// <summary>
		/// Invokes a dynamic constructor without parameters.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public new TType Invoke() => this.constructorDelegate(SafeUtils.EmptyArrayOfObject);

		/// <summary>
		/// Invokes dynamic constructor.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public new TType Invoke(object[] arguments) => this.constructorDelegate(arguments);

		#region Explicit Implementation of IDynamicConstructor

		/// <summary>
		/// Gets the invoke expression.
		/// </summary>
		Expression<Func<object[], object>> IDynamicConstructor.InvokeExpression
		{
			[DebuggerStepThrough]
			get { return this.untypedInvokeExpression; }
		}

		/// <summary>
		/// Invokes a dynamic constructor without parameters.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		object IDynamicConstructor.Invoke() => this.Invoke();

		/// <summary>
		/// Invokes a dynamic constructor.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		object IDynamicConstructor.Invoke(object[] arguments) => this.Invoke(arguments);

		#endregion
	}
}