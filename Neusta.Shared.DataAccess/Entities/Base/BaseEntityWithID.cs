namespace Neusta.Shared.DataAccess.Entities
{
	using System;
	using System.Diagnostics;
	using Neusta.Shared.Core;

	public abstract class BaseEntityWithID : BaseEntity, IEntityWithID
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public virtual long ID { get; set; }

		#region Explicit Implementation of IID

		/// <summary>
		/// Gets the unique identifier.
		/// </summary>
		object IID.ID
		{
			[DebuggerStepThrough]
			get { return this.ID; }
		}

		/// <summary>
		/// Determines whether the specified other is equal.
		/// </summary>
		bool IID.IsIDEqual(object other)
		{
			if (other is IEntityWithID<long> vm)
			{
				return Equals(this.ID, vm.ID);
			}
			return ReferenceEquals(other, this);
		}

		#endregion

		#region Explicit Implementation of IEquatable<TID>

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		bool IEquatable<long>.Equals(long other)
		{
			return Equals(this.ID, other);
		}

		#endregion
	}
}