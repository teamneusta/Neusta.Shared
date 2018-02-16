// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;

	// ReSharper disable once InconsistentNaming
	public static class IContainerBuilderExtensions
	{
		/// <summary>
		/// Includes assemblies that are referenced by the main assembly.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder IncludeReferencedAssemblies(this IContainerBuilder builder)
		{
			Guard.ArgumentNotNull(builder, nameof(builder));

			// Use the default scan configuration syntax
			builder.Configure(cfg => cfg
				.Scan(scan => scan
					.FromAssemblyDependencies(builder.Configuration.ApplicationAssembly)
					.AddClasses()
					.AsSelfWithInterfaces()));

			return builder;
		}
	}
}