using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views
{
	public class StockViewModel
	{
		public int id { get; set; }

		public string name { get; set; }

		public string code { get; set; }

		public double price { get; set; }

		public double weight { get; set; }

		public bool isBase { get; set; }

		public bool ignore { get; set; }

		public double ratio { get; set; }
	}
}
