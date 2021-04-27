using Core.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;

namespace Core.Factories
{
	public class IndicatorFactory
	{
		public static IIndicator Create(string entity, IStockService stockService, IFuturesService futuresService)
		{
			if (entity == nameof(BlueChips)) return new BlueChips(stockService);
			else if (entity == nameof(Prices)) return new Prices(futuresService);
			else if (entity == nameof(Powers)) return new Powers(futuresService);


			throw new Exception(String.Format("Can Not Create Indicator Entity Name = {0}", entity));

		}
	}
}
