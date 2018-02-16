namespace Neusta.Shared.Core.Collections
{
	using System.Collections.Generic;

	public class PagedList<T> : IPagedList<T>
	{
		public PagedList()
		{
			this.Items = new List<T>();
		}

		public int TotalCount { get; set; }

		public IList<T> Items { get; set; }
	}
}