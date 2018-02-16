namespace Neusta.Shared.Core.DynamicCode.Safe
{
	using System;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using Neusta.Shared.Core.Utils;

	/// <summary>
	/// Safe wrapper for the dynamic property.
	/// </summary>
	internal class SafeProperty : IDynamicProperty
	{
		private readonly PropertyInfo propertyInfo;
		private readonly Func<object, object> getter;
		private readonly Expression<Func<object, object>> getterExpression;
		private readonly Action<object, object> setter;
		private readonly Expression<Action<object, object>> setterExpression;

		/// <summary>
		/// Creates a new instance of the safe property wrapper.
		/// </summary>
		public SafeProperty(PropertyInfo propertyInfo)
		{
			Guard.ArgumentNotNull(propertyInfo, "propertyInfo");
			this.propertyInfo = propertyInfo;

			// Build the getter
			Type declaringType = propertyInfo.DeclaringType;
			Type valueType = propertyInfo.PropertyType;
			if (propertyInfo.CanRead)
			{
				ParameterExpression sourceParameterExp = Expression.Parameter(typeof(object));
				Expression memberAccessExp;
				if (propertyInfo.GetMethod.IsStatic)
				{
					memberAccessExp = Expression.MakeMemberAccess(null, propertyInfo);
				}
				else
				{
					UnaryExpression castExp = Expression.Convert(sourceParameterExp, declaringType);
					memberAccessExp = Expression.MakeMemberAccess(castExp, propertyInfo);
				}
				if (valueType != typeof(object))
				{
					memberAccessExp = Expression.TypeAs(memberAccessExp, typeof(object));
				}
				this.getterExpression = Expression.Lambda<Func<object, object>>(memberAccessExp, sourceParameterExp);
				this.getter = this.getterExpression.Compile();
			}

			// Build the setter
			if (propertyInfo.CanWrite)
			{
				ParameterExpression targetParameterExp = Expression.Parameter(typeof(object));
				ParameterExpression valueParameterExp = Expression.Parameter(typeof(object));
				MemberExpression fieldExp;
				if (propertyInfo.SetMethod.IsStatic)
				{
					fieldExp = Expression.Property(null, propertyInfo);
				}
				else
				{
					UnaryExpression castExp = Expression.Convert(targetParameterExp, declaringType);
					fieldExp = Expression.Property(castExp, propertyInfo);
				}
				BinaryExpression assignExp;
				if (valueType != typeof(object))
				{
					UnaryExpression valueCast;
					if (valueType.GetTypeInfo().IsValueType)
					{
						valueCast = Expression.Unbox(valueParameterExp, valueType);
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
				this.setterExpression = Expression.Lambda<Action<object, object>>(assignExp, targetParameterExp, valueParameterExp);
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
		public Expression<Func<object, object>> GetterExpression
		{
			[DebuggerStepThrough]
			get { return this.getterExpression; }
		}

		/// <summary>
		/// Gets the setter expression.
		/// </summary>
		public Expression<Action<object, object>> SetterExpression
		{
			[DebuggerStepThrough]
			get { return this.setterExpression; }
		}

		/// <summary>
		/// Gets the value of the dynamic property for the specified target object.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public object GetValue(object target) => this.getter(target);

		/// <summary>
		/// Gets the value of the dynamic property for the specified target object.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(object target, object value) => this.setter(target, value);
	}
}