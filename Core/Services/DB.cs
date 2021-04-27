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
	public interface ITickDBService
	{
		void SaveTicksToDB(string code, IEnumerable<Tick> ticks);
		List<Tick> GetTicksFromDB(string code, int date = 0);
	}


	public class FileTickDBService : ITickDBService
	{
		private readonly string rootFolder;
		List<string> symbolCodes = new List<string> { "TX" };
		

		public FileTickDBService(IEnumerable<string> symbolCodes, string tickFolder)
		{
			this.rootFolder = tickFolder;
			this.symbolCodes.AddRange(symbolCodes);

			Init();
		}

		void Init()
		{
			string tickFolder = TickFolder(DateTime.Today.ToDateNumber());
			if (!Directory.Exists(tickFolder)) Directory.CreateDirectory(tickFolder);

			foreach (var code in symbolCodes)
			{
				string pathString = StockFileName(code);
				if (!File.Exists(pathString)) File.Create(pathString).Dispose();
			}

		}

		string TickFolder(int date = 0)
		{
			if(date > 0) return $"{rootFolder}{date.ToString()}";
		
			return $"{rootFolder}{DateTime.Today.ToDateNumber().ToString()}";

		}
		

		string StockFileName(string code, int date = 0) => Path.Combine(TickFolder(date), $"{code}.json");


		public List<Tick> GetTicksFromDB(string code, int date = 0)
		{
			string content = File.ReadAllText(StockFileName(code, date));

			return JsonConvert.DeserializeObject<List<Tick>>(content);
		}

		public void SaveTicksToDB(string code, IEnumerable<Tick> ticks)
		{
			var ticksInDB = GetTicksFromDB(code);
			if (!ticksInDB.IsNullOrEmpty())
			{
				foreach (var item in ticks)
				{
					var exist = ticksInDB.Where(t => t.order == item.order).FirstOrDefault();
					if (exist == null)
					{
						ticksInDB.Add(item);
					}
				}
			}
			else
			{
				ticksInDB = new List<Tick>();
				ticksInDB.AddRange(ticks);
			}

			if (ticksInDB.IsNullOrEmpty()) File.WriteAllText(StockFileName(code), "");
			else File.WriteAllText(StockFileName(code), JsonConvert.SerializeObject(ticksInDB));

		}
	}
}
