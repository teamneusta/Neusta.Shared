namespace Neusta.Shared.Core.DynamicCode.Safe
{
	using System;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.Core.Utils;

	/// <summary>
	/// Safe wrapper for the dynamic indexer.
	/// </summary>
	internal sealed class SafeIndexedProperty : IDynamicIndexedProperty
	{
		private readonly PropertyInfo propertyInfo;
		private readonly Func<object, object[], object> getter;
		private Expression<Func<object, object[], object>> getterExpression;
		private readonly Action<object, object, object[]> setter;
		private Expression<Action<object, object, object[]>> setterExpression;

		/// <summary>
		/// Creates a new instance of the safe property wrapper.
		/// </summary>
		public SafeIndexedProperty(PropertyInfo propertyInfo)
		{
			Guard.ArgumentNotNull(propertyInfo, "propertyInfo");
			Guard.ArgumentAssert(propertyInfo.GetIndexParameters().Length > 0, "propertyInfo");
			this.propertyInfo = propertyInfo;

			// Build the getter
			ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
			Type declaringType = this.propertyInfo.DeclaringType;
			Type valueType = this.propertyInfo.PropertyType;
			if (this.propertyInfo.CanRead)
			{
				ParameterExpression indexParameterExp = Expression.Parameter(typeof(object[]));
				ParameterExpression sourceParameterExp = Expression.Parameter(typeof(object));
				int cnt = indexParameters.Length;
				var indexerExpArray = new Expression[cnt];
				for (var idx = 0; idx < cnt; idx++)
				{
					Expression indexerExp = Expression.ArrayIndex(indexParameterExp, Expression.Constant(idx));
					Type parameterType = indexParameters[idx].ParameterType;
					if (parameterType != typeof(object))
					{
						if (parameterType.GetTypeInfo().IsValueType)
						{
							indexerExp = Expression.Unbox(indexerExp, parameterType);
						}
						else
						{
							indexerExp = Expression.TypeAs(indexerExp, parameterType);
						}
					}
					indexerExpArray[idx] = indexerExp;
				}
				Expression indexExpr;
				if (this.propertyInfo.GetMethod.IsStatic)
				{
					indexExpr = Expression.MakeIndex(null, this.propertyInfo, indexerExpArray);
				}
				else
				{
					UnaryExpression castExp = Expression.Convert(sourceParameterExp, declaringType);
					indexExpr = Expression.MakeIndex(castExp, this.propertyInfo, indexerExpArray);
				}
				if (valueType != typeof(object))
				{
					indexExpr = Expression.TypeAs(indexExpr, typeof(object));
				}
				this.getterExpression = Expression.Lambda<Func<object, object[], object>>(indexExpr, sourceParameterExp, indexParameterExp);
				this.getter = this.getterExpression.Compile();
			}

			// Build the setter
			if (this.propertyInfo.CanWrite)
			{
				ParameterExpression indexParameterExp = Expression.Parameter(typeof(object[]));
				ParameterExpression targetParameterExp = Expression.Parameter(typeof(object));
				ParameterExpression valueParameterExp = Expression.Parameter(typeof(object));
				int cnt = indexParameters.Length;
				var indexerExpArray = new Expression[cnt];
				for (var idx = 0; idx < cnt; idx++)
				{
					Expression indexerExp = Expression.ArrayIndex(indexParameterExp, Expression.Constant(idx));
					Type parameterType = indexParameters[idx].ParameterType;
					if (parameterType != typeof(object))
					{
						if (parameterType.GetTypeInfo().IsValueType)
						{
							indexerExp = Expression.Unbox(indexerExp, parameterType);
						}
						else
						{
							indexerExp = Expression.TypeAs(indexerExp, parameterType);
						}
					}
					indexerExpArray[idx] = indexerExp;
				}
				IndexExpression fieldExp;
				if (this.propertyInfo.SetMethod.IsStatic)
				{
					fieldExp = Expression.Property(null, this.propertyInfo, indexerExpArray);
				}
				else
				{
					UnaryExpression castExp = Expression.Convert(targetParameterExp, declaringType);
					fieldExp = Expression.Property(castExp, this.propertyInfo, indexerExpArray);
				}
				BinaryExpression assignExp;
				if (valueType != typeof(object))
				{
					UnaryExpression valueCast;
					if (valueType.GetTypeInfo().IsValueType)
					{
						valueCast = Expression.Convert(valueParameterExp, valueType);
					}
					else
					{
						valueCast = Expression.TypeAs(valueParameterExp, valueType);
					}
					assignExp = Expression.Assign(fieldExp, valueCast);
				}
				else
				{
					assignExp = Expression.Assign(fieldExp, valueParameterExp);
				}
				this.setterExpression = Expression.Lambda<Action<object, object, object[]>>(assignExp, targetParameterExp, indexParameterExp, valueParameterExp);
				this.setter = this.setterExpression.Compile();
			}
		}

		/// <summary>
		/// Gets the property info.
		/// </summary>
		public PropertyInfo PropertyInfo
		{
			[DebuggerStepThrough]
			get { return this.propertyInfo; }
		}

		/// <summary>
		/// Gets the getter expression.
		/// </summary>
		public Expression<Func<object, object[], object>> GetterExpression
		{
			[DebuggerStepThrough]
			get { return this.getterExpression; }
		}

		/// <summary>
		/// Gets the setter expression.
		/// </summary>
		public Expression<Action<object, object, object[]>> SetterExpression
		{
			[DebuggerStepThrough]
			get { return this.setterExpression; }
		}

		/// <summary>
		/// Gets the value of the dynamic property for the specified target object.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetValue(object target, params object[] index) => this.getter(target, index);

		/// <summary>
		/// Gets the value of the dynamic property for the specified target object.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(object target, object value, params object[] index) => this.setter(target, value, index);
	}
}