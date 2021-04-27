using Core.Logging;
using Core.Services;
using Core.Views;
using System.Collections.Generic;

namespace Core.Factories
{
	public class ServiceFactory
	{
		public static IApiService CreateApiService(string baseUrl, string email, string password, string dbKey)
		{
			return new ApiService(baseUrl, email, password, dbKey);
		}

		public static IStockService CreateStockService(List<StockViewModel> stocks, string baseStockCode, ITickDBService tickDBService)
		{
			return new StockService(stocks, baseStockCode, tickDBService);
		}

		public static IFuturesService CreateFuturesService(ITickDBService tickDBService)
		{
			return new FuturesService(tickDBService);
		}

		public static ITickDBService CreateTickDBService(IEnumerable<string> symbolCodes, string tickFolder)
		{
			return new FileTickDBService(symbolCodes, tickFolder);
		}

        
    }
}
