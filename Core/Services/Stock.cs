using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Views;
using System.IO;
using Core.Helpers;
using Newtonsoft.Json;

namespace Core.Services
{
	public interface IStockService
	{
		IEnumerable<string> GetStockCodes();
		IEnumerable<StockViewModel> GetStocks(bool updatePrice = false);


		IEnumerable<Tick> GetTicks(string code, int begin, int end);

		void SaveTick(string code, Tick tick);
		void SaveTicksToDB();
	}

	public class StockService : IStockService
	{
		private readonly string baseStockCode;
		private readonly ITickDBService tickDBService;

		List<StockViewModel> stocks;

		Dictionary<string, IList<Tick>> symbolTicks = new Dictionary<string, IList<Tick>>();

		IList<Tick> GetTicks(string code) => symbolTicks[code];

		public IEnumerable<Tick> GetTicks(string code, int begin, int end) => GetTicks(code).Where(t => t.time >= begin && t.time < end);

		public StockService(List<StockViewModel> stocks, string baseStockCode, ITickDBService tickDBService)
		{
			this.stocks = stocks;
			this.baseStockCode = baseStockCode;
			this.tickDBService = tickDBService;

			var baseStock = stocks.Where(s => s.code == baseStockCode).FirstOrDefault();
			foreach (var item in stocks)
			{
				item.ratio = (item.price / baseStock.price) * (item.weight / baseStock.weight);
			}

			InitSymbolTicks();

		}

		void InitSymbolTicks()
		{
			var codes = GetStockCodes();
			symbolTicks = new Dictionary<string, IList<Tick>>();
			foreach (var code in codes)
			{
				symbolTicks[code] = new List<Tick>();
			}
		}


		public IEnumerable<string> GetStockCodes() => stocks.Select(s => s.code);

		public IEnumerable<StockViewModel> GetStocks(bool updatePrice = false)
		{
			if(!updatePrice) return stocks;

			foreach (var item in stocks)
			{
				var ticks = GetTicks(item.code);
				if (!ticks.IsNullOrEmpty())
				{
					item.price = ticks.Last().price;
				} 
			}

			return stocks;
		}

		public void SaveTick(string code, Tick tick)
		{
			var ticks = GetTicks(code);
			var exist = ticks.Where(t => t.order == tick.order).FirstOrDefault();
			if (exist == null) ticks.Add(tick);
		}

		

		public void SaveTicksToDB()
		{
			foreach (var code in symbolTicks.Keys)
			{
				var ticks = GetTicks(code);
				tickDBService.SaveTicksToDB(code, ticks);

			}

		}

	}



}
