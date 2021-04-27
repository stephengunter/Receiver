using Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;

namespace Core.Indicators
{
	public class Prices : IIndicator
	{
		private readonly IFuturesService futuresService;

		public Prices(IFuturesService futuresService)
		{
			this.futuresService = futuresService;
		}

		public DataViewModel Calculate(int begin, int end)
		{
			var ticks = futuresService.GetTicks(begin, end);

			var result = ticks.GroupBy(t => t.price, t => t.qty, (price, qty) => new
			{
				Price = price,
				Qty = qty.Sum()
			});

			double totalPrices = result.Sum(r => r.Price * r.Qty);
			int totalQty = result.Sum(r => r.Qty);

			var avg = Math.Round((totalPrices / totalQty) , 2);

			var data = new DataViewModel
			{
				indicator = nameof(Prices),
				text = avg.ToString(),
				val = avg.ToString()
			};

			return data;
		}



	}
}
