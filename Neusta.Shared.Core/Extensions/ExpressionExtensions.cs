// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	public static class ExpressionExtensions
    {
        public static Expression<Func<TObject, bool>> OrElse<TObject>(this Expression<Func<TObject, bool>> expr1, Expression<Func<TObject, bool>> expr2)
            where TObject : class
        {
            ParameterExpression p = expr1.Parameters[0];

            var visitor = new SubstExpressionVisitor();
            visitor.Subst[expr2.Parameters[0]] = p;

            Expression body = Expression.OrElse(expr1.Body, visitor.Visit(expr2.Body));
            return Expression.Lambda<Func<TObject, bool>>(body, p);
        }

        public static Expression<Func<TObject, bool>> AndAlso<TObject>(this Expression<Func<TObject, bool>> expr1, Expression<Func<TObject, bool>> expr2)
            where TObject : class
        {
            ParameterExpression p = expr1.Parameters[0];

            var visitor = new SubstExpressionVisitor();
            visitor.Subst[expr2.Parameters[0]] = p;

            Expression body = Expression.AndAlso(expr1.Body, visitor.Visit(expr2.Body));
            return Expression.Lambda<Func<TObject, bool>>(body, p);
        }

        public static Expression<Func<TObject, bool>> DateEquals<TObject>(this Expression<Func<TObject, DateTime?>> expression, string dateInput)
        {
            DateTime.TryParse(dateInput, out DateTime inputDateTime);

            Expression<Func<DateTime?, bool>> subExpression = s => s.HasValue && s.Value.Date.Equals(inputDateTime.Date);

            var swapVisitor = new SwapVisitor(subExpression.Parameters[0], expression.Body);
            Expression<Func<TObject, bool>> lambda = Expression.Lambda<Func<TObject, bool>>(swapVisitor.Visit(subExpression.Body), expression.Parameters);

            return lambda;
        }

        /// <summary>
        /// Creates an expression to validate that an field is included in the given values.
        /// </summary>
        /// <param name="expression">The field to check.</param>
        /// <param name="value">The Enum value (integer) as string, multiple values have to be passed separated by ','.</param>
        public static Expression<Func<TObject, bool>> EnumContains<TObject, TProperty>(this Expression<Func<TObject, TProperty>> expression, string value)
            where TProperty : struct, IConvertible
        {
            Expression<Func<TProperty, bool>> subExpression = s => value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Contains(s.ToString(CultureInfo.InvariantCulture));

            var swapVisitor = new SwapVisitor(subExpression.Parameters[0], expression.Body);
            Expression<Func<TObject, bool>> lambda = Expression.Lambda<Func<TObject, bool>>(swapVisitor.Visit(subExpression.Body), expression.Parameters);

            return lambda;
        }

        private class SubstExpressionVisitor : ExpressionVisitor
        {
            protected internal readonly Dictionary<Expression, Expression> Subst = new Dictionary<Expression, Expression>();

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (this.Subst.TryGetValue(node, out Expression newValue))
                {
                    return newValue;
                }
                return node;
            }
        }

        private class SwapVisitor : ExpressionVisitor
        {
            private readonly Expression _from, _to;

            public SwapVisitor(Expression from, Expression to)
            {
                this._from = from;
                this._to = to;
            }

            public override Expression Visit(Expression node)
            {
                return node == this._from ? this._to : base.Visit(node);
            }
        }
    }
}