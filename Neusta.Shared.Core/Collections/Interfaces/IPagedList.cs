namespace Neusta.Shared.Core.Collections
{
	using System.Collections.Generic;

	public interface IPagedList<T>
	{
		int TotalCount { get; set; }

		IList<T> Items { get; set; }
	}
}