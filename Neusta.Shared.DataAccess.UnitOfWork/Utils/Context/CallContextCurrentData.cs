namespace Neusta.Shared.DataAccess.UnitOfWork.Utils
{
	using System.Runtime.CompilerServices;
	using System.Threading;

	internal static class CallContextCurrentData
	{
		private static readonly AsyncLocal<ContextKey> currentScope = new AsyncLocal<ContextKey>();
		private static readonly ConditionalWeakTable<ContextKey, ContextData> contextDataTable = new ConditionalWeakTable<ContextKey, ContextData>();

		public static ContextData CreateOrGetCurrentData(ContextKey contextKey)
		{
			currentScope.Value = contextKey;
			return contextDataTable.GetValue(contextKey, env => new ContextData(true));
		}

		public static void ClearCurrentData(ContextKey contextKey, bool removeContextData)
		{
			// Get the current ambient ContextKey.
			ContextKey key = currentScope.Value;
			if (contextKey != null || key != null)
			{
				// removeContextData flag is used for perf optimization to avoid removing from the table in certain nested scope usage.
				if (removeContextData)
				{
					// if context key is passed in remove that from the contextDataTable, otherwise remove the default context key.
					contextDataTable.Remove(contextKey ?? key);
				}
				if (key != null)
				{
					currentScope.Value = null;
				}
			}
		}

		public static bool TryGetCurrentData(out ContextData currentData)
		{
			currentData = null;

			// Get the current ambient ContextKey.
			ContextKey contextKey = currentScope.Value;
			if (contextKey == null)
			{
				return false;
			}
			else
			{
				return contextDataTable.TryGetValue(contextKey, out currentData);
			}
		}
	}
}