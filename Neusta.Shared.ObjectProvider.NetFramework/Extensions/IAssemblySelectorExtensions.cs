// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyModel;
	using Neusta.Shared.Core.Utils;

	// ReSharper disable once InconsistentNaming
	public static class IAssemblySelectorExtensions
	{
		/// <summary>
		/// Will scan for types from the calling assembly.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromCallingAssembly(this IAssemblySelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			Assembly callingAssembly = Assembly.GetCallingAssembly();
			if (callingAssembly == null)
			{
				throw new InvalidOperationException("Assembly.GetCallingAssembly() returned null.");
			}
			return selector.FromAssemblies(callingAssembly);
		}

		/// <summary>
		/// Will scan for types from the currently executing assembly.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromExecutingAssembly(this IAssemblySelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (executingAssembly == null)
			{
				throw new InvalidOperationException("Assembly.GetExecutingAssembly() returned null.");
			}
			return selector.FromAssemblies(executingAssembly);
		}

		/// <summary>
		/// Will scan for types from the entry assembly.
		/// </summary>

		[PublicAPI]
		public static IImplementationTypeSelector FromEntryAssembly(this IAssemblySelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly == null)
			{
				throw new InvalidOperationException("Assembly.GetEntryAssembly() returned null.");
			}
			return selector.FromAssemblies(entryAssembly);
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently executing application.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromApplicationDependencies(this IAssemblySelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.FromApplicationDependencies(_ => true);
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently executing application.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromApplicationDependencies(this IAssemblySelector selector, Func<Assembly, bool> predicate)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(predicate, nameof(predicate));

			try
			{
				var context = DependencyContext.Default;
				if (context != null)
				{
					return selector.FromDependencyContext(context, predicate);
				}
				else
				{
					return selector.FromAssemblyDependencies(selector.InternalGetApplicationAssembly() ?? Assembly.GetEntryAssembly(), predicate);
				}
			}
			catch
			{
				// Something went wrong when loading the DependencyContext, fall
				// back to loading all referenced assemblies of the entry assembly...
				return selector.FromAssemblyDependencies(selector.InternalGetApplicationAssembly() ?? Assembly.GetEntryAssembly(), predicate);
			}
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently specified type.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssemblyDependenciesOf<T>(this IAssemblySelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.FromAssemblyDependencies(typeof(T).Assembly, _ => true);
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently specified type.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssemblyDependenciesOf<T>(this IAssemblySelector selector, Func<Assembly, bool> predicate)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.FromAssemblyDependencies(typeof(T).Assembly, predicate);
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently specified <paramref name="assembly"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssemblyDependencies(this IAssemblySelector selector, Assembly assembly)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(assembly, nameof(assembly));

			return selector.FromAssemblyDependencies(assembly, _ => true);
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently specified <paramref name="assembly"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAssemblyDependencies(this IAssemblySelector selector, Assembly assembly, Func<Assembly, bool> predicate)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(assembly, nameof(assembly));
			Guard.ArgumentNotNull(predicate, nameof(predicate));

			var assemblies = new List<Assembly> { assembly };
			try
			{
				AssemblyName[] dependencyNames = assembly.GetReferencedAssemblies();
				foreach (AssemblyName dependencyName in dependencyNames)
				{
					try
					{
						// Try to load the referenced assembly...
						Assembly dependencyAssembly = Assembly.Load(dependencyName);
						if (predicate(dependencyAssembly))
						{
							assemblies.Add(dependencyAssembly);
						}
					}
					catch
					{
						// Failed to load assembly. Skip it.
					}
				}
				return selector.InternalFromAssemblies(assemblies);
			}
			catch
			{
				return selector.InternalFromAssemblies(assemblies);
			}
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently specified type and
		/// recursively all of their dependencies.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAllAssemblyDependenciesOf<T>(this IAssemblySelector selector)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.FromAllAssemblyDependencies(typeof(T).Assembly, _ => true);
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently specified type and
		/// recursively all of their dependencies.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAllAssemblyDependenciesOf<T>(this IAssemblySelector selector, Func<Assembly, bool> predicate)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.FromAllAssemblyDependencies(typeof(T).Assembly, predicate);
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently specified <paramref name="assembly"/> and
		/// recursively all of their dependencies.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAllAssemblyDependencies(this IAssemblySelector selector, Assembly assembly)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.FromAllAssemblyDependencies(assembly, _ => true);
		}

		/// <summary>
		/// Will load and scan all runtime libraries referenced by the currently specified <paramref name="assembly"/> and
		/// recursively all of their dependencies.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromAllAssemblyDependencies(this IAssemblySelector selector, Assembly assembly, Func<Assembly, bool> predicate)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(assembly, nameof(assembly));
			Guard.ArgumentNotNull(predicate, nameof(predicate));

			var assemblies = new List<Assembly> { assembly };
			try
			{
				var dependencyNames = new List<AssemblyName>(assembly.GetReferencedAssemblies());
				int idx = 0;
				while (idx < dependencyNames.Count)
				{
					AssemblyName dependencyName = dependencyNames[idx];
					try
					{
						// Try to load the referenced assembly...
						Assembly dependencyAssembly = Assembly.Load(dependencyName);
						if (predicate(dependencyAssembly))
						{
							assemblies.Add(dependencyAssembly);

							// Add additional assembly dependencies
							foreach (AssemblyName subDependencyName in dependencyAssembly.GetReferencedAssemblies())
							{
								if (dependencyNames.Any(match => match.FullName != subDependencyName.FullName))
								{
									dependencyNames.Add(subDependencyName);
								}
							}
						}
					}
					catch
					{
						// Failed to load assembly. Skip it.
					}
					idx++;
				}
				return selector.InternalFromAssemblies(assemblies);
			}
			catch
			{
				return selector.InternalFromAssemblies(assemblies);
			}
		}

		/// <summary>
		/// Will load and scan all runtime libraries in the given <paramref name="context"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromDependencyContext(this IAssemblySelector selector, DependencyContext context)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));

			return selector.FromDependencyContext(context, _ => true);
		}

		/// <summary>
		/// Will load and scan all runtime libraries in the given <paramref name="context"/>.
		/// </summary>
		[PublicAPI]
		public static IImplementationTypeSelector FromDependencyContext(this IAssemblySelector selector, DependencyContext context, Func<Assembly, bool> predicate)
		{
			Guard.ArgumentNotNull(selector, nameof(selector));
			Guard.ArgumentNotNull(context, nameof(context));
			Guard.ArgumentNotNull(predicate, nameof(predicate));

			var assemblies = context.RuntimeLibraries
				.SelectMany(library => library.GetDefaultAssemblyNames(context))
				.Select(Assembly.Load)
				.Where(predicate)
				.ToArray();

			return selector.InternalFromAssemblies(assemblies);
		}
	}
}