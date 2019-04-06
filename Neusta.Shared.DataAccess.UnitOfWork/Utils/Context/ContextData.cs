namespace Neusta.Shared.DataAccess.UnitOfWork.Utils
{
	using System.Diagnostics;
	using Neusta.Shared.DataAccess.UnitOfWork.Base;

	internal class ContextData
	{
		private readonly bool asyncFlow;
		private BaseUnitOfWork currentUnitOfWork;

		internal ContextData(bool asyncFlow)
		{
			this.asyncFlow = asyncFlow;
		}

		internal bool AsyncFlow
		{
			[DebuggerStepThrough]
			get { return this.asyncFlow; }
		}

		internal BaseUnitOfWork CurrentUnitOfWork
		{
			[DebuggerStepThrough]
			get { return this.currentUnitOfWork; }
			[DebuggerStepThrough]
			set { this.currentUnitOfWork = value; }
		}
	}
}