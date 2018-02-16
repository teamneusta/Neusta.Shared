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
	internal sealed class SafeProperty<TType, TValue> : SafeProperty, IDynamicProperty<TType, TValue>
	{
		private readonly Func<TType, TValue> getter;
		private readonly Expression<Func<TType, TValue>> getterExpression;
		private readonly Action<TType, TValue> setter;
		private readonly Expression<Action<TType, TValue>> setterExpression;

		/// <summary>
		/// Creates a new instance of the safe property wrapper.
		/// </summary>
		public SafeProperty(PropertyInfo propertyInfo)
			: base(propertyInfo)
		{
			Guard.ArgumentNotNull(propertyInfo, "propertyInfo");

			// Build the getter
			Type declaringType = propertyInfo.DeclaringType;
			Type valueType = propertyInfo.PropertyType;
			if (propertyInfo.CanRead)
			{
				ParameterExpression sourceParameterExp = Expression.Parameter(typeof(TType));
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
				this.getterExpression = Expression.Lambda<Func<TType, TValue>>(memberAccessExp, sourceParameterExp);
				this.getter = this.getterExpression.Compile();
			}

			// Build the setter
			if (propertyInfo.CanWrite)
			{
				ParameterExpression targetParameterExp = Expression.Parameter(typeof(TType));
				ParameterExpression valueParameterExp = Expression.Parameter(typeof(TValue));
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
				BinaryExpression assignExp = Expression.Assign(fieldExp, valueParameterExp);
				this.setterExpression = Expression.Lambda<Action<TType, TValue>>(assignExp, targetParameterExp, valueParameterExp);
				this.setter = this.setterExpression.Compile();
			}
		}

		/// <summary>
		/// Gets the getter expression.
		/// </summary>
		public new Expression<Func<TType, TValue>> GetterExpression
		{
			[DebuggerStepThrough]
			get { return this.getterExpression; }
		}

		/// <summary>
		/// Gets the setter expression.
		/// </summary>
		public new Expression<Action<TType, TValue>> SetterExpression
		{
			[DebuggerStepThrough]
			get { return this.setterExpression; }
		}

		/// <summary>
		/// Gets the value of the dynamic property for the specified target object.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TValue GetValue(TType target) => this.getter(target);

		/// <summary>
		/// Gets the value of the dynamic property for the specified target object.
		/// </summary>
		[DebuggerNonUserCode]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetValue(TType target, TValue value) => this.setter(target, value);
	}
}