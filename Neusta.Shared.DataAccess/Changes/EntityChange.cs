namespace Neusta.Shared.DataAccess.Changes
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;

	[DebuggerDisplay("EntityChange='{ChangedEntity}', State={State}", Type = "Change")]
	public class EntityChange : IEntityChange
	{
		private readonly object changedEntity;
		private readonly ChangeState state;
		private readonly IEnumerable<IPropertyChange> changedProperties;

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityChange"/> class.
		/// </summary>
		private EntityChange(object changedEntity, ChangeState state)
			: this(changedEntity, state, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityChange" /> class.
		/// </summary>
		private EntityChange(object changedEntity, ChangeState state, IEnumerable<IPropertyChange> changedProperties)
		{
			if (changedEntity == null)
			{
				throw new ArgumentNullException(nameof(changedEntity));
			}
			if (state == ChangeState.Modified && changedProperties == null)
			{
				throw new ArgumentNullException($"Parameter {nameof(changedProperties)} must be defined if ChangeState is Modified.", nameof(changedProperties));
			}

			this.changedEntity = changedEntity;
			this.changedProperties = changedProperties;
			this.state = state;
		}

		public object ChangedEntity
		{
			[DebuggerStepThrough]
			get { return this.changedEntity; }
		}

		public IEnumerable<IPropertyChange> ChangedProperties
		{
			[DebuggerStepThrough]
			get { return this.changedProperties; }
		}

		public ChangeState State
		{
			[DebuggerStepThrough]
			get { return this.state; }
		}

		public static IEntityChange CreateUpdateChange(object changedEntity, IEnumerable<PropertyChange> changedProperties)
		{
			return new EntityChange(changedEntity, ChangeState.Modified, changedProperties);
		}

		public static IEntityChange CreateDeleteChange(object changedEntity)
		{
			return new EntityChange(changedEntity, ChangeState.Deleted);
		}

		public static IEntityChange CreateAddedChange(object changedEntity)
		{
			return new EntityChange(changedEntity, ChangeState.Added);
		}
	}
}