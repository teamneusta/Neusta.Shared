namespace Neusta.Shared.DataAccess.Utils
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using JetBrains.Annotations;

	public static class EntityActivator<TEntity>
		where TEntity : class, IEntity
	{
		private static readonly Func<TEntity> createFunc;
		private static readonly Action<TEntity> initializeFunc;

		/// <summary>
		/// Initializes static members of the <see cref="EntityActivator{TEntity}"/> class.
		/// </summary>
		static EntityActivator()
		{
			// Build a "new" expression, that initializes the lists
			NewExpression newExpr = Expression.New(typeof(TEntity));
			Expression bodyExpr;
			if (typeof(IInitializeEntityLists).GetTypeInfo().IsAssignableFrom(typeof(TEntity).GetTypeInfo()))
			{
				var commandList = new List<Expression>();
				ParameterExpression resultExpr = Expression.Variable(typeof(TEntity));
				MemberInitExpression memberInitExpr = Expression.MemberInit(newExpr);
				commandList.Add(Expression.Assign(resultExpr, memberInitExpr));
				ParameterExpression intfExpr = Expression.Variable(typeof(IInitializeEntityLists));
				UnaryExpression castExpr = Expression.TypeAs(resultExpr, typeof(IInitializeEntityLists));
				commandList.Add(Expression.Assign(intfExpr, castExpr));
				MethodInfo methodInfo = typeof(TEntity).GetRuntimeMethod(nameof(IInitializeEntityLists.InitializeLists), new Type[0]);
				commandList.Add(Expression.Call(intfExpr, methodInfo));
				commandList.Add(resultExpr);
				bodyExpr = Expression.Block(commandList);
			}
			else
			{
				var bindingList = new List<MemberAssignment>();
				IEnumerable<PropertyInfo> propInfos = typeof(TEntity).GetRuntimeProperties();
				foreach (PropertyInfo propInfo in propInfos)
				{
					Type propType = propInfo.PropertyType;
					if (propInfo.CanWrite && propType.IsCollection() && !propType.IsArray)
					{
						TypeInfo propTypeInfo = propType.GetTypeInfo();
						if (propTypeInfo.IsInterface)
						{
							propType = typeof(List<>).MakeGenericType(propType.GenericTypeArguments);
						}
						NewExpression propNewExpr = Expression.New(propType);
						MemberAssignment binding = Expression.Bind(propInfo, propNewExpr);
						bindingList.Add(binding);
					}
				}
				if (bindingList.Any())
				{
					bodyExpr = Expression.MemberInit(newExpr, bindingList);
				}
				else
				{
					bodyExpr = newExpr;
				}
			}
			Expression<Func<TEntity>> createLambdaExpr = Expression.Lambda<Func<TEntity>>(bodyExpr);
			createFunc = createLambdaExpr.Compile();

			// Build an initialization method that initializes the lists
			ParameterExpression parameterExpr = Expression.Parameter(typeof(TEntity), "entity");
			if (typeof(IInitializeEntityLists).GetTypeInfo().IsAssignableFrom(typeof(TEntity).GetTypeInfo()))
			{
				UnaryExpression castExpr = Expression.TypeAs(parameterExpr, typeof(IInitializeEntityLists));
				MethodInfo methodInfo = typeof(TEntity).GetRuntimeMethod(nameof(IInitializeEntityLists.InitializeLists), new Type[0]);
				bodyExpr = Expression.Call(castExpr, methodInfo);
			}
			else
			{
				var commandList = new List<Expression>();
				IEnumerable<PropertyInfo> propInfos = typeof(TEntity).GetRuntimeProperties();
				foreach (PropertyInfo propInfo in propInfos)
				{
					Type propType = propInfo.PropertyType;
					if (propInfo.CanWrite && propType.IsCollection() && !propType.IsArray)
					{
						TypeInfo propTypeInfo = propType.GetTypeInfo();
						if (propTypeInfo.IsInterface)
						{
							propType = typeof(List<>).MakeGenericType(propType.GenericTypeArguments);
						}
						NewExpression propNewExpr = Expression.New(propType);
						MemberExpression propExpr = Expression.MakeMemberAccess(parameterExpr, propInfo);
						BinaryExpression assignment = Expression.Assign(propExpr, propNewExpr);
						commandList.Add(assignment);
					}
				}
				if (commandList.Any())
				{
					bodyExpr = Expression.Block(commandList);
				}
				else
				{
					bodyExpr = Expression.Empty();
				}
			}
			Expression<Action<TEntity>> initializeLambdaExpr = Expression.Lambda<Action<TEntity>>(bodyExpr, parameterExpr);
			initializeFunc = initializeLambdaExpr.Compile();
		}

		/// <summary>
		/// Creates a new instance of the given entity type.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TEntity Create()
		{
			return createFunc();
		}

		/// <summary>
		/// Initializes all collections in the given entity.
		/// </summary>
		[PublicAPI]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Initialize(TEntity entity)
		{
			initializeFunc(entity);
		}
	}
}