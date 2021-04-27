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
	public interface IFuturesService
	{
		void SaveTick(Tick tick);
		void SaveTicksToDB();
		IEnumerable<Tick> GetTicks(int begin, int end);
		IEnumerable<Tick> GetTicks(int end);

		QuoteViewModel GetQuote(int begin, int end);
	}

	public class FuturesService : IFuturesService
	{
		private readonly string code = "TX";
		private readonly ITickDBService tickDBService;

		List<Tick> ticks = new List<Tick>();

		

		public FuturesService(ITickDBService tickDBService)
		{
			this.tickDBService = tickDBService;	
		}

		public void SaveTick(Tick tick)
		{
			var exist = ticks.Where(t => t.order == tick.order).FirstOrDefault();
			if (exist == null) ticks.Add(tick);
		}

		public void SaveTicksToDB() => tickDBService.SaveTicksToDB(code, ticks);

		public IEnumerable<Tick> GetTicks(int begin, int end)
		{
			return ticks.Where(t => t.time >= begin && t.time < end).OrderBy(t => t.order);
		}

		public IEnumerable<Tick> GetTicks(int end)
		{
			return ticks.Where(t => t.time < end).OrderBy(t => t.order);
		}


		public QuoteViewModel GetQuote(int begin, int end)
		{
			var tickList = GetTicks(begin, end);

			if (tickList.IsNullOrEmpty()) return null;

			return new QuoteViewModel
			{
				time = end,
				high = tickList.Max(t => (int)t.price),
				low = tickList.Min(t => (int)t.price),
				open = (int)tickList.First().price,
				price = (int)tickList.Last().price
			};
		}

	}



}
