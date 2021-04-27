using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Views;
using SKCOMLib;
using Core.Exceptions;
using System.Threading;
using Core.Helpers;

namespace Core.Sources
{
	public class Capital : ISource
	{
		string txSymbolKey = "TX00";
		private readonly string sid;
		private readonly string password;

		int m_nCode;
		string strMsg;

		SKCenterLib m_pSKCenter;
		SKQuoteLib m_SKQuoteLib;

		SKReplyLib m_pSKReply;

		int date = DateTime.Today.ToDateNumber();
		private readonly int futuresBegin = 84500;
		private readonly int futuresEnd = 134500;
		private readonly int stockBegin = 90000;
		private readonly int stockEnd = 133000;

		public Capital(string sid, string password)
		{
			this.sid = sid;
			this.password = password;

			m_pSKCenter = new SKCenterLib();
			m_pSKReply = new SKReplyLib();
			m_pSKReply.OnReplyMessage += new _ISKReplyLibEvents_OnReplyMessageEventHandler(this.OnAnnouncement);

			Login();

			m_SKQuoteLib = new SKCOMLib.SKQuoteLib();
			m_SKQuoteLib.OnConnection += new _ISKQuoteLibEvents_OnConnectionEventHandler(m_SKQuoteLib_OnConnection);
			m_SKQuoteLib.OnNotifyHistoryTicks += new _ISKQuoteLibEvents_OnNotifyHistoryTicksEventHandler(m_SKQuoteLib_OnNotifyHistoryTicks);
			m_SKQuoteLib.OnNotifyTicks += new _ISKQuoteLibEvents_OnNotifyTicksEventHandler(m_SKQuoteLib_OnNotifyTicks);
			
		}

		public IEnumerable<string> SymbolCodes { get; set; }

		Dictionary<short, string> symbolIndexCode = new Dictionary<short, string>();
		
		Dictionary<short, double> symbolIndexPoints = new Dictionary<short, double>();

		void InitSymbolIndexCode()
		{
			symbolIndexCode = new Dictionary<short, string>();
			symbolIndexPoints = new Dictionary<short, double>();

			var tx = GetSKSTOCKByCode(txSymbolKey);
			symbolIndexCode[tx.sStockIdx] = txSymbolKey;

			double txPoints = 1;
			for (int i = 0; i < tx.sDecimal; i++)
			{
				txPoints *= 10;
			}
			symbolIndexPoints[tx.sStockIdx] = txPoints;

			foreach (var code in SymbolCodes)
			{
				var pSKStock = GetSKSTOCKByCode(code);
				symbolIndexCode[pSKStock.sStockIdx] = code;

				double symbolPoints = 1;
				for (int i = 0; i < pSKStock.sDecimal; i++)
				{
					symbolPoints *= 10;
				}
				symbolIndexPoints[pSKStock.sStockIdx] = symbolPoints;
			}
		}

		public event EventHandler ExceptionOccured;
		void OnExceptionOccured(string msg)
		{
			ExceptionOccured?.Invoke(this, new ExceptionEventArgs(new SourceException(msg)));
		}


		public event EventHandler ActionExecuted;
		void OnActionExecuted(ActionEventArgs e)
		{
			ActionExecuted?.Invoke(this, e);
		}
		void OnActionExecuted(string action)
		{
			this.strMsg = m_pSKCenter.SKCenterLib_GetReturnCodeMessage(this.m_nCode);
			OnActionExecuted(new ActionEventArgs(action, this.m_nCode.ToString(), this.strMsg));

		}

		public event EventHandler NotifyStockTick;
		public event EventHandler NotifyFuturesTick;

		public bool Connectted
		{
			get
			{
				CheckConnect();
				return m_nCode == 1;
			}
		}
		

		public void Connect()
		{
			m_nCode = m_SKQuoteLib.SKQuoteLib_EnterMonitor();
			OnActionExecuted("EnterMonitor");
		}


		public void DisConnect()
		{
			m_nCode = m_SKQuoteLib.SKQuoteLib_LeaveMonitor();
			OnActionExecuted("LeaveMonitor");
		}

		

		public void Login()
		{
			m_nCode = m_pSKCenter.SKCenterLib_Login(sid.ToUpper(), password);			
			OnActionExecuted("Login");			
		}

		void CheckConnect()
		{
			m_nCode = m_SKQuoteLib.SKQuoteLib_IsConnected();
			OnActionExecuted("SKQuoteLib_IsConnected");
		}

		void RegisterQuote()
		{
			short sPage = 1;
			var symbolCodes = symbolIndexCode.Values;

			foreach (var code in symbolCodes)
			{
				m_nCode = m_SKQuoteLib.SKQuoteLib_RequestTicks(sPage, code);
				if (m_nCode != 0)
				{
					throw new SourceException();
				}
				sPage++;
			}


		}

		SKSTOCK GetSKSTOCKByCode(string code)
		{
			SKSTOCK pSKStock = new SKSTOCK();
			int nCode = m_SKQuoteLib.SKQuoteLib_GetStockByNo(code, ref pSKStock);
			return pSKStock;
		}

		bool IsStock(string code) => code != txSymbolKey;

		bool InTime(string code, int time)
		{
			if (IsStock(code)) return time >= stockBegin && time <= stockEnd;
			return time >= futuresBegin && time <= futuresEnd;
		}

		void HandleTickNotify(bool realTime ,short sStockIdx, int nPtr, int lTimehms, int nBid, int nAsk, int nClose, int nQty)
		{
			string code = symbolIndexCode[sStockIdx];
			if (InTime(code, lTimehms))
			{
				double symbolPoints = symbolIndexPoints[sStockIdx];

				var tick = new Tick
				{
					order = nPtr,
					time = lTimehms,
					bid = nBid / symbolPoints,
					offer = nAsk / symbolPoints,
					price = nClose / symbolPoints,
					qty = nQty
				};

				var e = new TickEventArgs(code, tick, realTime);
				if (IsStock(code)) NotifyStockTick?.Invoke(this, e);
				else NotifyFuturesTick?.Invoke(this, e);
			}
		}


		#region COM EVENT

		void m_SKQuoteLib_OnConnection(int nKind, int nCode)
		{
			if (nKind == 3001 && nCode == 0)
			{
				//報價連線OK
			}
			else if (nKind == 3003 && nCode == 0)
			{
				//報價商品載入完成
				if (symbolIndexCode.Count < 1)
				{
					InitSymbolIndexCode();
				}

				try
				{
					RegisterQuote();
				}
				catch (Exception)
				{
					OnExceptionOccured("RegisterQuote Exception");
				}
				
			}

		}


		void m_SKQuoteLib_OnNotifyTicks(short sMarketNo, short sStockIdx, int nPtr, int nDate, int lTimehms, int lTimemillismicros, int nBid, int nAsk, int nClose, int nQty, int nSimulate)
		{
			if (nSimulate > 0) return;

			if (nDate == this.date)
			{
				bool realTime = true;
				HandleTickNotify(realTime, sStockIdx, nPtr, lTimehms, nBid, nAsk, nClose, nQty);
			}			

		}
		
		void m_SKQuoteLib_OnNotifyHistoryTicks(short sMarketNo, short sStockIdx, int nPtr, int nDate, int lTimehms, int lTimemillismicros, int nBid, int nAsk, int nClose, int nQty, int nSimulate)
		{
			if (nSimulate > 0) return;

			if (nDate == this.date)
			{
				bool realTime = false;
				HandleTickNotify(realTime, sStockIdx, nPtr, lTimehms, nBid, nAsk, nClose, nQty);
			}
			
		}

		void OnAnnouncement(string strUserID, string bstrMessage, out short nConfirmCode)
		{
			OnActionExecuted(new ActionEventArgs("Announcement", this.m_nCode.ToString(), this.strMsg));
			nConfirmCode = -1;
		}
		#endregion

	}
}
