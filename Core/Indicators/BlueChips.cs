using Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;

namespace Core.Indicators
{
	public class BlueChips : IIndicator
	{
		private readonly IStockService stockService;

		public BlueChips(IStockService stockService)
		{
			this.stockService = stockService;
		}

		public DataViewModel Calculate(int begin, int end)
		{
			var stocks = stockService.GetStocks();
			double buyQty = 0;
			double sellQty = 0;

			foreach (var item in stocks)
			{
				var ticks = stockService.GetTicks(item.code, begin, end);

				int itemBuyQty = ticks.Where(t => t.type == 1).Sum(t => t.qty);
				int itemSellQty = ticks.Where(t => t.type == -1).Sum(t => t.qty);

				buyQty += itemBuyQty * item.ratio;
				sellQty += itemSellQty * item.ratio;
			}


			var result = new string[] { Convert.ToInt32(Math.Ceiling(buyQty)).ToString(), Convert.ToInt32(Math.Ceiling(sellQty)).ToString() };

			var data = new DataViewModel
			{
				indicator = nameof(BlueChips),
				text = String.Join(",", result),
				val = Math.Ceiling(buyQty - sellQty).ToString()
			};

			return data;
		}



	}
}
