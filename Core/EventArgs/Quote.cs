using Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
	public class QuoteEventArgs : EventArgs
	{
		public QuoteEventArgs(QuoteViewModel quote)
		{
			this.Quote = quote;
		}
		public QuoteViewModel Quote { get; private set; }
	}

	public class TickEventArgs : EventArgs
	{
		public TickEventArgs(string code, Tick tick, bool realTime)
		{
			this.Code = code;
			this.Tick = tick;
			this.RealTime = realTime;
		}


		public string Code { get; private set; }
		public Tick Tick { get; private set; }
		public bool RealTime { get; private set; }
	}
}
