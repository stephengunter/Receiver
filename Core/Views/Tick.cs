using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views
{
	public class Tick
	{
		public int order { get; set; }
		public int time { get; set; }

		public double price { get; set; }
		public double bid { get; set; }
		public double offer { get; set; }

		public int qty { get; set; }

		public int type
		{
			get
			{
				if (price == bid) return -1;
				else if (price == offer) return 1;
				return 0;
			}
		}

	}
}
