namespace Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Conventions
{
	using System;
	using System.Data.Entity.Core.Metadata.Edm;
	using System.Data.Entity.Infrastructure;
	using System.Data.Entity.ModelConfiguration.Conventions;
	using Neusta.Shared.DataAccess.EntityFramework.TenantFilter.Entities;

	public class NoCascadeDeleteOnTenantForeignKeys : IStoreModelConvention<AssociationType>, IConvention
	{
		#region Implementation of IStoreModelConvention<AssociationType>

		/// <summary>
		/// Applies this convention to an item in the model.
		/// </summary>
		public void Apply(AssociationType item, DbModel model)
		{
			if (!item.IsForeignKey)
			{
				return;
			}

			ReferentialConstraint constraint = item.Constraint;
			if (string.Equals(constraint.FromRole.Name, nameof(TenantEntity)))
			{
				constraint.FromRole.DeleteBehavior = OperationAction.None;
				constraint.ToRole.DeleteBehavior = OperationAction.None;
			}
		}

		#endregion
	}
}