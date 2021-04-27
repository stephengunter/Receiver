using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views
{
	public class QuoteViewModel
	{
		public int date { get; set; }

		public int time { get; set; }

		public int price { get; set; }

		public int open { get; set; }

		public int high { get; set; }

		public int low { get; set; }

		public List<DataViewModel> dataList { get; set; } = new List<DataViewModel>();

	}
}
