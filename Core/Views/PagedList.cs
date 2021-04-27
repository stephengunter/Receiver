using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views
{
	public class PagedList<T, V>
	{
		public List<T> List { get; set; }
		public List<V> ViewList { get; set; }

		public int TotalItems { get; set; }
		public int PageNumber { get; }
		public int PageSize { get; }
		public int TotalPages { get; }
		public bool HasPreviousPage { get; }
		public bool HasNextPage { get; }

		public int NextPageNumber { get; }

		public int PreviousPageNumber { get; }
	}
}
