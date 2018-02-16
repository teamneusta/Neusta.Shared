// ReSharper disable once CheckNamespace
namespace Neusta.Shared.ObjectProvider
{
	using System;
	using System.Diagnostics;
	using JetBrains.Annotations;

	public static class ContainerAdapter
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static IContainerAdapter defaultAdapter;
		private static bool isRegistered;

		/// <summary>
		/// Gets the default adapter.
		/// </summary>
		[PublicAPI]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static IContainerAdapter Default
		{
			[DebuggerStepThrough]
			get { return defaultAdapter; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether a logging adapter is set.
		/// </summary>
		[PublicAPI]
		public static bool IsRegistered
		{
			[DebuggerStepThrough]
			get { return isRegistered; }
		}

		/// <summary>
		/// Registers the given adapter as default.
		/// </summary>
		[PublicAPI]
		public static IContainerAdapter Register(IContainerAdapter adapter)
		{
			if (adapter != null)
			{
				ContainerAdapter.defaultAdapter = adapter;
				isRegistered = true;
			}
			return ContainerAdapter.defaultAdapter;
		}
	}
}