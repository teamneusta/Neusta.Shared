namespace Neusta.Shared.DataAccess
{
	using System.Collections.Generic;
	using JetBrains.Annotations;

	public interface IEntityChange
	{
		[PublicAPI]
		object ChangedEntity{get;}

		[PublicAPI]
		IEnumerable<IPropertyChange> ChangedProperties{get;}

		[PublicAPI]
		ChangeState State {get;}
	}
}