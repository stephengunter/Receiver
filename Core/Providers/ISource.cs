using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Sources
{
	public interface ISource
	{
		void Login();
		bool Connectted { get; }
		void Connect();
		void DisConnect();

		IEnumerable<string> SymbolCodes { get; set; }

		event EventHandler ExceptionOccured;

		event EventHandler ActionExecuted;

		event EventHandler NotifyStockTick;
		event EventHandler NotifyFuturesTick;
	}
}
