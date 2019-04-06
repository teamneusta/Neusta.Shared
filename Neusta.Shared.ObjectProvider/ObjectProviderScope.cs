namespace Neusta.Shared.ObjectProvider
{
	using JetBrains.Annotations;
	using Neusta.Shared.ObjectProvider.Utils;

	public static class ObjectProviderScope
	{
		/// <summary>
		/// Gets the current <see cref="IObjectProviderScope" />.
		/// </summary>
		[PublicAPI]
		public static IObjectProviderScope Current
		{
			get
			{
				ContextData threadContextData;
				if (CallContextCurrentData.TryGetCurrentData(out threadContextData))
				{
					return threadContextData.CurrentScope as IObjectProviderScope;
				}
				return null;
			}
		}
	}
}