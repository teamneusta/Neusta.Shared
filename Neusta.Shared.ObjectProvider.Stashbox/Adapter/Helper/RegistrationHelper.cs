namespace Neusta.Shared.ObjectProvider.Stashbox.Adapter.Helper
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using CommonServiceLocator;
	using global::Stashbox;
	using global::Stashbox.Configuration;
	using global::Stashbox.Registration;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider.Stashbox.Attributes;

	internal static class RegistrationHelper
	{
		private static readonly Expression<Func<IDependencyResolver, object>> dependencyResolverResolveFunc = y => y.Resolve(typeof(object), true, null);
		private static readonly MethodInfo dependencyResolverResolveMethodInfo = ((MethodCallExpression)dependencyResolverResolveFunc.Body).Method;

		private static readonly Expression<Func<IObjectFactory, object>> objectFactoryCreateFunc = y => y.Create(null);
		private static readonly MethodInfo objectFactoryCreateMethodInfo = ((MethodCallExpression)objectFactoryCreateFunc.Body).Method;

		private static readonly ConstantExpression trueExpr = Expression.Constant(true, typeof(bool));
		private static readonly ConstantExpression falseExpr = Expression.Constant(true, typeof(bool));
		private static readonly ConstantExpression nullObjectArrayExpr = Expression.Constant(null, typeof(object[]));
		private static readonly ConstantExpression nullServiceProviderExpr = Expression.Constant(null, typeof(IServiceProvider));

		private static readonly TypeInfo IEnumerableTypeInfo = typeof(IEnumerable).GetTypeInfo();

		internal static bool RegisterService(IDependencyRegistrator container, IServiceDescriptor descriptor, IObjectProvider objectProvider, bool replaceExisting)
		{
			var serviceType = descriptor.ServiceType;

			// Make sure that we do not register IEnumerable
			if (IEnumerableTypeInfo.IsAssignableFrom(serviceType.GetTypeInfo()))
			{
				return false;
			}

			if (descriptor.IsSingletonBoundService())
			{
				// Special handling for singletons
				var propertyInfo = SingletonHelper.GetSingletonInstancePropertyInfo(descriptor.ImplementationType);
				container.Register(serviceType, delegate (IFluentServiceRegistrator context)
				{
					ConfigureContextForSingleton(context, propertyInfo, objectProvider);
				});
			}
			else
			{
				switch (descriptor.ImplementationSource)
				{
					case ImplementationSource.Type:
						// Register by implementation type
						container.Register(serviceType, descriptor.ImplementationType, delegate(IFluentServiceRegistrator context)
						{
							if (replaceExisting)
							{
								context.ReplaceExisting();
							}
							ConfigureContextFromServiceDescriptor(context, descriptor, objectProvider);
						});
						break;

					case ImplementationSource.Instance:
						// Register by implementation instance
						container.Register(serviceType, delegate(IFluentServiceRegistrator context)
						{
							if (replaceExisting)
							{
								context.ReplaceExisting();
							}
							context.WithInstance(descriptor.ImplementationInstance);
						});
						break;

					case ImplementationSource.Factory:
						// Register by factory method
						container.Register(serviceType, delegate(IFluentServiceRegistrator context)
						{
							if (replaceExisting)
							{
								context.ReplaceExisting();
							}
							bool hasFactoryMethod = ConfigureContextFromServiceDescriptor(context, descriptor, objectProvider);
							if (!hasFactoryMethod)
							{
								// Use expressions instead of Lambda statement
								var resolverExpr = Expression.Parameter(typeof(IDependencyResolver));
								var factoryFunc = descriptor.ImplementationFactory;
								var factoryInstanceExpr = Expression.Constant(factoryFunc);
								var factoryCallExpr = Expression.Invoke(factoryInstanceExpr);
								var lambdaExpr = Expression.Lambda<Func<IDependencyResolver, object>>(factoryCallExpr, resolverExpr);
								context.WithFactory(lambdaExpr.Compile());
							}
						});
						break;

					case ImplementationSource.FactoryWithProvider:
						// Register by factory method with provider
						container.Register(serviceType, delegate(IFluentServiceRegistrator context)
						{
							if (replaceExisting)
							{
								context.ReplaceExisting();
							}
							bool hasFactoryMethod = ConfigureContextFromServiceDescriptor(context, descriptor, objectProvider);
							if (!hasFactoryMethod)
							{
								var resolverExpr = Expression.Parameter(typeof(IDependencyResolver));
								var factoryFunc = descriptor.ImplementationFactoryWithProvider;
								var factoryInstanceExpr = Expression.Constant(factoryFunc);
								InvocationExpression factoryCallExpr;

								if (serviceType != typeof(IServiceProvider))
								{
									// Code that will be built using expressions:
									//  {
									//  	var provider = resolver.Resolve<IServiceProvider>(true) ?? objectProvider;
									//  	return descriptor.ImplementationFactoryWithProvider(provider);
									//  });

									var resolverServiceTypeExpr = Expression.Constant(typeof(IServiceProvider), typeof(Type));
									var resolverCallExpr = Expression.Call(resolverExpr, dependencyResolverResolveMethodInfo, resolverServiceTypeExpr, trueExpr, nullObjectArrayExpr);
									var fallbackExpr = Expression.Constant(objectProvider, typeof(IServiceProvider));
									var providerExpr = Expression.TypeAs(resolverCallExpr, typeof(IServiceProvider));
									var guaranteedProviderExpr = Expression.Coalesce(providerExpr, fallbackExpr);
									factoryCallExpr = Expression.Invoke(factoryInstanceExpr, guaranteedProviderExpr);
								}
								else
								{
									// Code that will be built using expressions:
									//  {
									//  	return descriptor.ImplementationFactoryWithProvider(null);
									//  });

									factoryCallExpr = Expression.Invoke(factoryInstanceExpr, nullServiceProviderExpr);
								}
								var lambdaExpr = Expression.Lambda<Func<IDependencyResolver, object>>(factoryCallExpr, resolverExpr);
								context.WithFactory(lambdaExpr.Compile());
							}
						});
						break;

					case ImplementationSource.FactoryInvalid:
						// Register by invalid factory method
						container.Register(serviceType, delegate(IFluentServiceRegistrator context)
						{
							if (replaceExisting)
							{
								context.ReplaceExisting();
							}

							// Use expressions instead of Lambda statement
							var resolverExpr = Expression.Parameter(typeof(IDependencyResolver));
							var newExpr = Expression.New(typeof(NotImplementedException));
							var throwExpr = Expression.Throw(newExpr);
							var lambdaExpr = Expression.Lambda<Func<IDependencyResolver, object>>(throwExpr, resolverExpr);
							context.WithFactory(lambdaExpr.Compile());
						});
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			return true;
		}

		internal static void UnregisterService(IDependencyRegistrator container, IServiceDescriptor descriptor)
		{
			var serviceType = descriptor.ServiceType;
			container.Register(serviceType, delegate (IFluentServiceRegistrator context)
			{
				context.ReplaceExisting();
				context.When(_ => false);
				context.WithInstance(null);
			});
		}

		internal static bool ConfigureContextForService(IFluentServiceRegistrator context, TypeInfo typeInfo, IObjectProvider objectProvider)
		{
			// Check for disposal tracking
			if (typeInfo.GetCustomAttribute<DisableDisposalTrackingAttribute>() != null)
			{
				context.WithoutDisposalTracking();
			}

			// Check for member injection mode
			var memberInjectionAttribute = typeInfo.GetCustomAttributes<MemberInjectionAttribute>().FirstOrDefault();
			if (memberInjectionAttribute != null)
			{
				ApplyMemberInjectionMode(context, memberInjectionAttribute.MemberInjectionMode);
			}

			// Check for lifetime
			var serviceLifetimeAttribute = typeInfo.GetCustomAttributes<ServiceLifetimeAttribute>().FirstOrDefault();
			if (serviceLifetimeAttribute != null)
			{
				ApplyServiceLifetime(context, serviceLifetimeAttribute.ServiceLifetime);
			}

			// Check for implementation type
			return ConfigureContextFromFactoryMethodAttributes(context, typeInfo, objectProvider);
		}

		internal static void ConfigureContextForSingleton(IFluentServiceRegistrator context, PropertyInfo propertyInfo, IObjectProvider objectProvider)
		{
			if (propertyInfo != null)
			{
				var resolverExpr = Expression.Parameter(typeof(IDependencyResolver));
				var memberExpr = Expression.MakeMemberAccess(null, propertyInfo);
				var lambdaExpr = Expression.Lambda<Func<IDependencyResolver, object>>(memberExpr, resolverExpr);

				context
					.WithFactory(lambdaExpr.Compile())
					.WithSingletonLifetime()
					.WithoutDisposalTracking();
			}
			else
			{
				context.AsConstant(null);
			}
		}

		internal static bool ConfigureContextFromServiceDescriptor(IFluentServiceRegistrator context, IServiceDescriptor descriptor, IObjectProvider objectProvider)
		{
			// Process disable disposal tracking
			if (descriptor.DisableDisposalTracking)
			{
				context.WithoutDisposalTracking();
			}

			// Process member injection mode
			ApplyMemberInjectionMode(context, descriptor.MemberInjectionMode);

			// Process service lifetime
			ApplyServiceLifetime(context, descriptor.ServiceLifetime);

			// Check for implementation type
			bool hasFactoryMethod = false;
			if (descriptor.ImplementationSource == ImplementationSource.Type)
			{
				hasFactoryMethod = ConfigureContextFromFactoryMethodAttributes(context, descriptor.ImplementationType.GetTypeInfo(), objectProvider);
			}
			return hasFactoryMethod;
		}

		private static bool ConfigureContextFromFactoryMethodAttributes(IFluentServiceRegistrator context, TypeInfo implementationTypeInfo, IObjectProvider objectProvider)
		{
			// Process factory class attribute
			bool hasFactoryMethods = false;
			var factoryType = implementationTypeInfo.GetCustomAttribute<FactoryTypeAttribute>(true)?.FactoryType;
			if (factoryType != null)
			{
				// Code that will be built using expressions:
				//  context.WithFactory(delegate(IDependencyResolver resolver)
				//  {
				//  	var factory = resolver.Resolve(factoryType) as IObjectFactory;
				//  	var provider = resolver.Resolve<IObjectProvider>(true) ?? objectProvider;
				//  	return factory?.Create(provider);
				//  });

				var resolverExpr = Expression.Parameter(typeof(IDependencyResolver));

				var factoryTargetTypeExpr = Expression.Constant(factoryType, typeof(Type));
				var factoryTargetCallExpr = Expression.Call(resolverExpr, dependencyResolverResolveMethodInfo, factoryTargetTypeExpr, falseExpr, nullObjectArrayExpr);
				var factoryTargetExpr = Expression.TypeAs(factoryTargetCallExpr, typeof(IObjectFactory));

				var resolverServiceTypeExpr = Expression.Constant(typeof(IServiceProvider), typeof(Type));
				var resolverCallExpr = Expression.Call(resolverExpr, dependencyResolverResolveMethodInfo, resolverServiceTypeExpr, trueExpr, nullObjectArrayExpr);
				var fallbackExpr = Expression.Constant(objectProvider, typeof(IServiceProvider));
				var providerExpr = Expression.TypeAs(resolverCallExpr, typeof(IServiceProvider));
				var guaranteedProviderExpr = Expression.Coalesce(providerExpr, fallbackExpr);

				var factoryCallExpr = Expression.Call(factoryTargetExpr, objectFactoryCreateMethodInfo, guaranteedProviderExpr);
				var lambdaExpr = Expression.Lambda<Func<IDependencyResolver, object>>(factoryCallExpr, resolverExpr);
				context.WithFactory(lambdaExpr.Compile());
				hasFactoryMethods = true;
			}
			else
			{
				// Process factory method attribute
				Type baseType = context.ImplementationType;
				while (baseType != null)
				{
					IEnumerable<MethodInfo> declaredMethods = baseType.GetTypeInfo().DeclaredMethods.Where(x => x.IsStatic && x.ReturnType == baseType && x.GetParameters().Length == 0);
					foreach (var methodInfo in declaredMethods)
					{
						var attr = methodInfo.GetCustomAttribute<FactoryMethodAttribute>(false);
						if (attr != null)
						{
							// Use expressions instead of Lambda statement
							var resolverExpr = Expression.Parameter(typeof(IDependencyResolver));
							var factoryCallExpr = Expression.Call(methodInfo);
							var lambdaExpr = Expression.Lambda<Func<IDependencyResolver, object>>(factoryCallExpr, resolverExpr);
							context.WithFactory(lambdaExpr.Compile());

							hasFactoryMethods = true;
							break;
						}
					}

					baseType = baseType.GetTypeInfo().BaseType;
				}
			}

			return hasFactoryMethods;
		}

		private static void ApplyServiceLifetime(IFluentServiceRegistrator context, ServiceLifetime serviceLifetime)
		{
			switch (serviceLifetime)
			{
				case ServiceLifetime.Scoped:
					context.WithScopedLifetime();
					break;
				case ServiceLifetime.PerResolutionRequest:
					context.WithPerResolutionRequestLifetime();
					break;
				case ServiceLifetime.Singleton:
					context.WithSingletonLifetime();
					break;
			}
		}

		private static void ApplyMemberInjectionMode(IFluentServiceRegistrator context, MemberInjectionMode memberInjectionMode)
		{
			switch (memberInjectionMode)
			{
				case MemberInjectionMode.PrivateFields:
					context.WithAutoMemberInjection(Rules.AutoMemberInjectionRules.PrivateFields);
					break;
				case MemberInjectionMode.PropertiesWithLimitedAccess:
					context.WithAutoMemberInjection(Rules.AutoMemberInjectionRules.PropertiesWithLimitedAccess);
					break;
				case MemberInjectionMode.PropertiesWithPublicSetter:
					context.WithAutoMemberInjection(Rules.AutoMemberInjectionRules.PropertiesWithPublicSetter);
					break;
				default:
					context.WithAutoMemberInjection(Rules.AutoMemberInjectionRules.None);
					break;
			}
		}
	}
}