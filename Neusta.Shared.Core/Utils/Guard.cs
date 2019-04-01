namespace Neusta.Shared.Core.Utils
{
	using System;
	using System.Diagnostics;
	using System.Globalization;
	using System.Reflection;
	using JetBrains.Annotations;

	/// <summary>
	/// Routinen zum schnellen Pr�fen von �bergabeparametern
	/// </summary>
	public static class Guard
	{
		/// <summary>
		/// Assert a bool expression, throwing <code>ArgumentException</code>
		/// if the expression is <code>false</code>.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentAssert(
			[AssertionCondition(AssertionConditionType.IS_TRUE)]
			bool expression,
			string message)
		{
			if (!expression)
			{
				throw new ArgumentException(message);
			}
		}

		/// <summary>
		/// Assert a bool expression, throwing <code>ArgumentException</code>
		/// if the expression is <code>false</code>.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentAssert(
			[AssertionCondition(AssertionConditionType.IS_TRUE)]
			bool expression,
			[InvokerParameterName, NotNull] string name,
			string message)
		{
			if (!expression)
			{
				throw new ArgumentException(message, name);
			}
		}

		/// <summary>
		/// Checks an argument to ensure that its 32-bit signed value isn't negative.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotNegativeValue(int argumentValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue < 0)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture,
						"The specified argument {0} cannot be initialized with a negative value.", argumentName));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that its 64-bit signed value isn't negative.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotNegativeValue(long argumentValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue < 0)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture,
						"The specified argument {0} cannot be initialized with a negative value.", argumentName));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that its 64-bit signed value isn't negative.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotNegativeValue(double argumentValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue < 0)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture,
						"The specified argument {0} cannot be initialized with a negative value.", argumentName));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that the value is not zero or negative.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotZeroOrNegativeValue(int argumentValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue <= 0)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture, "The specified argument {0} cannot be zero or negative.", argumentName));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that the value is not zero or negative.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotZeroOrNegativeValue(long argumentValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue <= 0)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture, "The specified argument {0} cannot be zero or negative.", argumentName));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that the value is not zero or negative.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotZeroOrNegativeValue(double argumentValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue <= 0)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture, "The specified argument {0} cannot be zero or negative.", argumentName));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that its value doesn't exceed the specified ceiling baseline.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotGreaterThan(int argumentValue, int ceilingValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue > ceilingValue)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture,
						"The specified argument {0} cannot be greater than its ceiling value of {1}.", argumentName, ceilingValue));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that its value doesn't exceed the specified ceiling baseline.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotGreaterThan(long argumentValue, long ceilingValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue > ceilingValue)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture,
						"The specified argument {0} cannot be greater than its ceiling value of {1}.", argumentName, ceilingValue));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that its value doesn't exceed the specified ceiling baseline.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotGreaterThan(double argumentValue, double ceilingValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue > ceilingValue)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture,
						"The specified argument {0} cannot be greater than its ceiling value of {1}.", argumentName, ceilingValue));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that its value doesn't go below the specified ceiling baseline.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotSmallerThan(int argumentValue, int ceilingValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue < ceilingValue)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture,
						"The specified argument {0} cannot be smaller than its ceiling value of {1}.", argumentName, ceilingValue));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that its value doesn't go below the specified ceiling baseline.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotSmallerThan(long argumentValue, long ceilingValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue < ceilingValue)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture,
						"The specified argument {0} cannot be smaller than its ceiling value of {1}.", argumentName, ceilingValue));
			}
		}

		/// <summary>
		/// Checks an argument to ensure that its value doesn't go below the specified ceiling baseline.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotSmallerThan(double argumentValue, double ceilingValue, [InvokerParameterName, NotNull] string argumentName)
		{
			if (argumentValue < ceilingValue)
			{
				throw new ArgumentOutOfRangeException(argumentName, argumentValue,
					string.Format(CultureInfo.CurrentCulture,
						"The specified argument {0} cannot be smaller than its ceiling value of {1}.", argumentName, ceilingValue));
			}
		}

		/// <summary>
		/// Assert a bool expression, throwing <code>ArgumentOutOfRangeException</code>
		/// if the expression is <code>false</code>.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentAssertRange(
			[AssertionCondition(AssertionConditionType.IS_TRUE)]
			bool expression,
			[InvokerParameterName, NotNull] string name)
		{
			if (!expression)
			{
				throw new ArgumentOutOfRangeException(name);
			}
		}

		/// <summary>
		/// Pr�ft einen Parameter auf "null"
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotNull(
			[AssertionCondition(AssertionConditionType.IS_NOT_NULL)]
			object value,
			[InvokerParameterName, NotNull] string name)
		{
			if (ReferenceEquals(value, null))
			{
				throw new ArgumentNullException(name);
			}
		}

		/// <summary>
		/// Pr�ft einen Parameter auf "null"
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotNull(
			[AssertionCondition(AssertionConditionType.IS_NOT_NULL)]
			object value,
			[InvokerParameterName, NotNull] string name,
			string message)
		{
			if (ReferenceEquals(value, null))
			{
				throw new ArgumentNullException(name, message);
			}
		}

		/// <summary>
		/// Pr�ft einen String-Parameter auf "null" oder leer
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotNullOrEmpty(
			[AssertionCondition(AssertionConditionType.IS_NOT_NULL)]
			string value,
			[InvokerParameterName, NotNull] string name)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(name);
			}
		}

		/// <summary>
		/// Pr�ft einen String-Parameter auf "null" oder leer
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotNullOrEmpty(
			[AssertionCondition(AssertionConditionType.IS_NOT_NULL)]
			string value,
			[InvokerParameterName, NotNull] string name,
			string message)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(name, message);
			}
		}

		/// <summary>
		/// Pr�ft einen String-Parameter auf "null" oder leer oder Whitespaces
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotNullOrWhiteSpace(
			[AssertionCondition(AssertionConditionType.IS_NOT_NULL)]
			string value,
			[InvokerParameterName, NotNull] string name)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException(name);
			}
		}

		/// <summary>
		/// Pr�ft einen String-Parameter auf "null" oder leer oder Whitespaces
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		[AssertionMethod]
		public static void ArgumentNotNullOrWhiteSpace(
			[AssertionCondition(AssertionConditionType.IS_NOT_NULL)]
			string value,
			[InvokerParameterName, NotNull] string name,
			string message)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException(name, message);
			}
		}

		/// <summary>
		/// Pr�ft eine Guid auf Guid.Empty
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		public static void ArgumentNotEmpty(Guid guid, [InvokerParameterName, NotNull] string name)
		{
			if (Equals(guid, Guid.Empty))
			{
				throw new ArgumentNullException(name);
			}
		}

		/// <summary>
		/// Pr�ft eine Guid auf Guid.Empty
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		public static void ArgumentNotEmpty(Guid guid, [InvokerParameterName, NotNull] string name, string message)
		{
			if (Equals(guid, Guid.Empty))
			{
				throw new ArgumentNullException(name, message);
			}
		}

		/// <summary>
		/// Pr�ft die G�ltigkeit eines Enum-Wertes.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		public static void ArgumentIsDefined<TEnum>(TEnum value, [InvokerParameterName, NotNull] string argumentName)
			where TEnum : struct
		{
			if (!Enum.IsDefined(typeof(TEnum), value))
			{
				throw new ArgumentOutOfRangeException(argumentName);
			}
		}

		/// <summary>
		/// Pr�ft die G�ltigkeit eines Enum-Wertes.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		public static void ArgumentIsDefined<TEnum>(Enum value, [InvokerParameterName, NotNull] string argumentName, string message)
			where TEnum : struct
		{
			if (!Enum.IsDefined(typeof(TEnum), value))
			{
				throw new ArgumentOutOfRangeException(argumentName, message);
			}
		}

		/// <summary>
		/// Pr�ft die G�ltigkeit eines Enum-Wertes.
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		public static void ArgumentIsDefined(Type enumType, object value, string argumentName)
		{
			if (!Enum.IsDefined(enumType, value))
			{
				throw new ArgumentException(
					string.Format(CultureInfo.CurrentCulture, "Argument value is not valid for enumeration type '{0}'.", argumentName,
						enumType), argumentName);
			}
		}

		/// <summary>
		/// Pr�ft, ob ein Typ von einem bestimmten Typen ist (d.h. zu diesem zuweisbar ist)
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		public static void ArgumentType(Type type, Type mustBeAssigneableTo, [InvokerParameterName, NotNull] string argumentName)
		{
			if (mustBeAssigneableTo != null && !mustBeAssigneableTo.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
			{
				throw new ArgumentException(
					string.Format(CultureInfo.CurrentCulture, "Argument must be assignable to '{0}'.", mustBeAssigneableTo), argumentName);
			}
		}

		/// <summary>
		/// Pr�ft, ob der Typ eines Parameters von einem bestimmten Typen ist (d.h. zu diesem zuweisbar ist)
		/// </summary>
		[PublicAPI]
		[DebuggerStepThrough]
		public static void ArgumentTypeOf(object typeOf, Type mustBeAssigneableTo, [InvokerParameterName, NotNull] string argumentName)
		{
			Type type;
			if (ReferenceEquals(typeOf, null))
			{
				type = typeof(void);
			}
			else
			{
				type = typeOf.GetType();
			}
			if (mustBeAssigneableTo != null && !mustBeAssigneableTo.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
			{
				throw new ArgumentException(
					string.Format(CultureInfo.CurrentCulture, "Argument must be assignable to '{0}'.", mustBeAssigneableTo), argumentName);
			}
		}

		/// <summary>
		/// Pr�ft, ob die Methode <paramref name="method"/> von Objekt <paramref name="target"/> aufgerufen werden kann.
		/// Dabei werden TransparentProxy-Objekte korrekt verarbeitet.
		///</summary>
		[PublicAPI]
		[DebuggerStepThrough]
		public static void ArgumentUnderstands(object target, string targetName, MethodBase method)
		{
			ArgumentNotNull(method, "method");
			if (ReferenceEquals(target, null))
			{
				if (method.IsStatic)
				{
					return;
				}
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
					"Target '{0}' is null and target method '{1}.{2}' is not static.", targetName, method.DeclaringType.FullName,
					method.Name));
			}
			ArgumentUnderstands(target, targetName, method.DeclaringType);
		}

		/// <summary>
		/// Pr�ft, ob das Objekt <paramref name="target"/> die Methoden von Typ <paramref name="target"/> unterst�tzt.
		/// Dabei werden TransparentProxy-Objekte korrekt verarbeitet.
		///</summary>
		[PublicAPI]
		[DebuggerStepThrough]
		public static void ArgumentUnderstands(object target, string targetName, Type requiredType)
		{
			ArgumentNotNull(requiredType, "requiredType");
			if (ReferenceEquals(target, null))
			{
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Target '{0}' is null.", targetName));
			}
			Type targetType = target.GetType();
			if (!requiredType.GetTypeInfo().IsAssignableFrom(targetType.GetTypeInfo()))
			{
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture,
					"Target '{0}' of type '{1}' does not support methods of '{2}'.", targetName, targetType, requiredType.FullName));
			}
		}
	}
}