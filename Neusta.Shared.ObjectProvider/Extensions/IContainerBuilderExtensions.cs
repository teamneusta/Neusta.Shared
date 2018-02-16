// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using JetBrains.Annotations;
	using Neusta.Shared.Core.Utils;
	using Neusta.Shared.ObjectProvider.Configuration.Helper;

	// ReSharper disable once InconsistentNaming
	public static class IContainerBuilderExtensions
	{
		/// <summary>
		/// Configure the logging builder.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder Configure(this IContainerBuilder builder, Action<IContainerConfigurationSyntax> configure)
		{
			Guard.ArgumentNotNull(builder, nameof(builder));

			if (configure != null)
			{
				var syntax = new RootSyntaxHelper(builder.Configuration);
				configure(syntax);
			}
			return builder;
		}

		/// <summary>
		/// Configure the logging builder.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder Configure(this IContainerBuilder builder, IContainerConfiguration configuration)
		{
			Guard.ArgumentNotNull(builder, nameof(builder));

			if (configuration != null)
			{
				builder.UpdateConfiguration(configuration);
			}
			return builder;
		}
	}
}