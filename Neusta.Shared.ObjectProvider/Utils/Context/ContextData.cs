namespace Neusta.Shared.ObjectProvider.Utils
{
	using System.Diagnostics;
	using Neusta.Shared.ObjectProvider.Base;

	internal class ContextData
	{
		private readonly bool asyncFlow;
		private BaseObjectProviderScope currentScope;

		internal ContextData(bool asyncFlow)
		{
			this.asyncFlow = asyncFlow;
		}

		internal bool AsyncFlow
		{
			[DebuggerStepThrough]
			get { return this.asyncFlow; }
		}

		internal BaseObjectProviderScope CurrentScope
		{
			[DebuggerStepThrough]
			get { return this.currentScope; }
			[DebuggerStepThrough]
			set { this.currentScope = value; }
		}
	}
}