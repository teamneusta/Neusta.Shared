namespace Neusta.Shared.ObjectProvider.Configuration.Helper
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;

	internal class ImplementationTypeFilter : IImplementationTypeFilter
	{
		private IEnumerable<Type> types;

		/// <summary>
		/// Initializes a new instance of the <see cref="ImplementationTypeFilter"/> class.
		/// </summary>
		public ImplementationTypeFilter(IEnumerable<Type> types)
		{
			this.types = types;
		}

		/// <summary>
		/// Gets the types.
		/// </summary>
		internal IEnumerable<Type> Types
		{
			[DebuggerStepThrough]
			get { return this.types; }
		}

		#region Explicit Implementation of IImplementationTypeFilter

		IImplementationTypeFilter IImplementationTypeFilter.InternalWhere(Func<Type, bool> predicate)
		{
			this.types = this.types.Where(predicate);
			return this;
		}

		#endregion
	}
}