namespace Neusta.Shared.DataAccess.Entities
{
	using System;
	using System.Diagnostics;
	using Neusta.Shared.Core;

	public abstract class BaseEntityWithID<TID> : BaseEntity, IEntityWithID<TID>
		where TID : IEquatable<TID>
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		public virtual TID ID { get; set; }

		#region Explicit Implementation of IID

		/// <summary>
		/// Gets the unique identifier.
		/// </summary>
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
			if (other is IEntityWithID<TID> vm)
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
		bool IEquatable<TID>.Equals(TID other)
		{
			return Equals(this.ID, other);
		}

		#endregion
	}
}