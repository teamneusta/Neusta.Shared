// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using global::Stashbox;
	using JetBrains.Annotations;
	using Neusta.Shared.ObjectProvider.Stashbox.Adapter;

	// ReSharper disable once InconsistentNaming
	public static class IContainerBuilderExtensions
	{
		/// <summary>
		/// Use Stashbox.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder UseStashbox(this IContainerBuilder builder)
		{
			builder.RegisterAdapterBuilder<StashboxContainerAdapterBuilder>();
			return builder;
		}

		/// <summary>
		/// Use Stashbox.
		/// </summary>
		[PublicAPI]
		public static IContainerBuilder UseStashbox(this IContainerBuilder builder, Action<IContainerConfigurator> configure)
		{
			var stashbox = builder.RegisterAdapterBuilder<StashboxContainerAdapterBuilder>();
			stashbox.Configure = configure;
			return builder;
		}
	}
}