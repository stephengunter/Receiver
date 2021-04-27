using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views
{
	public class IndicatorViewModel
	{
		public int id { get; set; }

		public string name { get; set; }

		public int order { get; set; }

		public string entity { get; set; }

		public string description { get; set; }

		public int begin { get; set; } //盤中產生信號開始時間  例如 90000

		public int end { get; set; } //盤中產生信號結束時間  例如 133000

		public int minParam { get; set; }

		public int maxParam { get; set; }

		public int defaultParam { get; set; }

		public List<int> paramList { get; set; }

		public bool main { get; set; }

		public string source { get; set; }

		public string type { get; set; }

		public bool withAvg { get; set; }

		public int coverId { get; set; }


		
	}

	//public class IndicatorViewModel
	//{
	//	public int id { get; set; }

	//	public string name { get; set; }

	//	public int order { get; set; }

	//	public string entity { get; set; }

	//	public int begin { get; set; } //盤中產生信號開始時間  例如 90000

	//	public int end { get; set; } //盤中產生信號結束時間  例如 133000


	//	public List<int> paramList { get; set; }

	//	public string defaultParam { get; set; }

	//	public bool removed { get; set; }

	//	public bool active { get; set; }

	//	public bool main { get; set; }

	//	public string type { get; set; }
	//}

}
