using Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;

namespace Core.Indicators
{
	public class Powers : IIndicator
	{
		private readonly IFuturesService futuresService;

		public Powers(IFuturesService futuresService)
		{
			this.futuresService = futuresService;
		}

		public DataViewModel Calculate(int begin, int end)
		{
			var ticks = futuresService.GetTicks(begin, end);

			double buyQty = 0;
			double sellQty = 0;

			int itemBuyQty = ticks.Where(t => t.type == 1).Sum(t => t.qty);
			int itemSellQty = ticks.Where(t => t.type == -1).Sum(t => t.qty);

			buyQty += itemBuyQty;
			sellQty += itemSellQty;


			var result = new string[] { Convert.ToInt32(Math.Ceiling(buyQty)).ToString(), Convert.ToInt32(Math.Ceiling(sellQty)).ToString() };

			var data = new DataViewModel
			{
				indicator = nameof(Powers),
				text = String.Join(",", result),
				val = Math.Ceiling(buyQty - sellQty).ToString()
			};

			return data;
		}



	}
}
