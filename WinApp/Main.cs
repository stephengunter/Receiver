using Core.Helpers;
using Core.Sources;
using Core.Factories;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Core;
using System.Threading;
using Core.Logging;
using Core.Services;
using System.Linq;
using Core.Indicators;
using System.Threading.Tasks;

namespace WinApp
{
	public partial class Main : Form
	{
		private readonly ISettingsManager _settingsManager;
		private readonly ILogger _logger;
		private ITimeManager _timeManager;
		private IDateManager _dateManager;

		private readonly ISource _source;
		private readonly IApiService _apiService;

		IStockService _stockService;
		IFuturesService _futuresService;

		private readonly IHubManager _hubManager;

		List<IIndicator> _indicators = new List<IIndicator>();
		
		private readonly int seconds;
		int rounds = 0;

		#region Helpers
		bool IsDevelopment => _settingsManager.IsDevelopment;
		bool IsBusinessDay => _dateManager.BusinessDay;
		bool InTime => _timeManager.InTime;
		bool HasClosed => DateTime.Now > _timeManager.CloseTime.AddMinutes(15);

		int DateNum => _dateManager.DateNum;
		List<int> KLineTimes => _timeManager.KLineTimes;
		#endregion

		int lastRoundHistoryTicks = 0;
		int historyTicks = 0;
		bool HasNewHistoryTicks => lastRoundHistoryTicks != historyTicks;

		int latestFuturesTickTime = 84500;

		List<int> GetNeedTimes()
		{
			var needTimes = new List<int>() { latestFuturesTickTime.ToKLineTime() };
			if (HasNewHistoryTicks)
			{
				var oldTimes = KLineTimes.Where(t => t < needTimes[0]);
				if (!oldTimes.IsNullOrEmpty()) needTimes.AddRange(oldTimes);
			}

			return needTimes;
		}


		bool initSuccess = false;
		



		public Main()
		{
			_settingsManager = Factories.CreateSettingsManager();
			this.seconds = _settingsManager.GetSettingValue(AppSettingsKey.Seconds).ToInt();

			
			this._timeManager = Factories.CreateTimeManager(_settingsManager.GetSettingValue(AppSettingsKey.Open),
				_settingsManager.GetSettingValue(AppSettingsKey.Close));

			_logger = LoggerFactory.Create(_settingsManager.LogFilePath);


			string siteUrl = _settingsManager.GetSettingValue(AppSettingsKey.SiteUrl);
			string adminUser = _settingsManager.GetSettingValue(AppSettingsKey.Admin);
			string adminPassword = _settingsManager.GetSettingValue(AppSettingsKey.AdminPassword);
			string dbKey = _settingsManager.GetSettingValue(AppSettingsKey.DBKey);
			this._apiService = ServiceFactory.CreateApiService(siteUrl, adminUser, adminPassword, dbKey);

			string hubUrl = _settingsManager.GetSettingValue(AppSettingsKey.HubUrl);
			string quoteKey = _settingsManager.GetSettingValue(AppSettingsKey.QuoteKey);
			_hubManager = Factories.CreateHubManager(hubUrl, quoteKey);


			// init source
			string provider = _settingsManager.GetSettingValue(AppSettingsKey.Provider);
			string sid = _settingsManager.GetSettingValue(AppSettingsKey.SID);
			string password = _settingsManager.GetSettingValue(AppSettingsKey.Password);

			this._source = SourceFactory.Create(provider, sid, password);
			this._source.ExceptionOccured += Source_ExceptionOccured;
			this._source.ActionExecuted += Source_ActionExecuted;
			this._source.NotifyStockTick += Source_NotifyStockTick;
			this._source.NotifyFuturesTick += Source_NotifyFuturesTick;

			Thread.Sleep(1500);


			InitializeComponent();

		}

		private async void Main_Load(object sender, EventArgs e)
		{
			await Init();

			Thread.Sleep(3000);

			this.timer.Interval = this.seconds * 1000;
			this.timer.Enabled = true;

		}


		async Task Init()
		{
			try
			{
				await _apiService.Login();
			}
			catch (Exception ex)
			{
				_logger.LogException(new Exception("apiService.Login Failed"));
				_logger.LogException(ex);
				initSuccess = false;
				return;
			}

			try
			{

				var holidays = await _apiService.GetHolidays();
				this._dateManager = Factories.CreateDateManager(holidays);

				
				if (!IsDevelopment)
				{
					if (!IsBusinessDay)
					{
						this.Close();
						return;
					}
				}
				

				var stocks = await _apiService.GetStocks();
				stocks = stocks.Where(s => !s.ignore).ToList();

				string tickFileFolder = _settingsManager.GetSettingValue(AppSettingsKey.TickFile);
				var tickDBService = ServiceFactory.CreateTickDBService(stocks.Select(s => s.code), tickFileFolder);

				string baseStock = _settingsManager.GetSettingValue(AppSettingsKey.BaseStock);
				_stockService = ServiceFactory.CreateStockService(stocks, baseStock, tickDBService);
				_futuresService = ServiceFactory.CreateFuturesService(tickDBService);

				var indicatorDataList = await _apiService.GetIndicators();
				foreach (var item in indicatorDataList)
				{
					this._indicators.Add(IndicatorFactory.Create(item.entity, _stockService, _futuresService));
				}

				_source.SymbolCodes = _stockService.GetStockCodes();
				_source.Connect();

			}
			catch (Exception ex)
			{
				_logger.LogException(ex);
				initSuccess = false;
				return;
			}

			initSuccess = true;
		}

		private void Source_ActionExecuted(object sender, EventArgs e)
		{
			var args = e as ActionEventArgs;
			_logger.LogInfo($"{args.Action} - {args.Code} - {args.Msg}");
		}


		private void Source_ExceptionOccured(object sender, EventArgs e)
		{
			var args = e as ExceptionEventArgs;
			_logger.LogException(args.Exception);

			_source.DisConnect();
			Thread.Sleep(1000);
			_source.Connect();

		}

		private void Source_NotifyStockTick(object sender, EventArgs e)
		{
			var args = e as TickEventArgs;
			if (!args.RealTime) historyTicks++;

			_stockService.SaveTick(args.Code, args.Tick);
		}

	

		private void Source_NotifyFuturesTick(object sender, EventArgs e)
		{
			var args = e as TickEventArgs;
			var tick = args.Tick;

			
			_futuresService.SaveTick(tick);

			if (args.RealTime) latestFuturesTickTime = tick.time;
			else historyTicks++;

			
		}



		private async void timer_Tick(object sender, EventArgs e)
		{
			if (InTime)
			{
				rounds++;

				if (OneMinute)
				{
					if (!initSuccess) await Init();
					else
					{
						CheckConnect();
					}
				}


				await OnNewRound();

			}
			else if (HasClosed)
			{
				if (!IsDevelopment)
				{
					this.timer.Interval = 1000 * 60 * 5;

					_source.DisConnect();

					await _apiService.MoveRealTimeToData();

					this.Close();
				}
				
			}
			
			RenderTime();

		}

		async Task OnNewRound()
		{
			if (!initSuccess) return;

			var needTimes = GetNeedTimes();
			
			
			foreach (var time in needTimes)
			{
				var beginEndTimes = time.ResolveBeginEndTime();
				int begin = beginEndTimes[0];
				int end = beginEndTimes[1];

				var quote = _futuresService.GetQuote(begin, end);
				if (quote != null)
				{
					quote.date = this.DateNum;
					foreach (var indicator in _indicators)
					{
						var data = indicator.Calculate(begin, end);
						if (data != null)
						{
							data.date = quote.date;
							data.time = quote.time;
							quote.dataList.Add(data);
						}
					}

					try
					{
						await _apiService.CreateQuote(quote);
						OnQuoteSaved();
					}
					catch (Exception ex)
					{
						_logger.LogException(ex);
					}

					
				}
			}

			lastRoundHistoryTicks = historyTicks;

		}

		void OnQuoteSaved()
		{
			try
			{
				_hubManager.InvokeQuote();
			}
			catch (Exception ex)
			{
				_logger.LogException(ex);
			}
		}

		void CheckConnect()
		{
			bool connectted = _source.Connectted;

			RenderConnectted(connectted);

			if (!connectted)
			{
				_source.DisConnect();
				Thread.Sleep(1000);
				_source.Connect();

				connectted = _source.Connectted;
				RenderConnectted(connectted);
			}

		}



		#region Render
		void RenderTime()
		{
			labelTime.Text = DateTime.Now.ToString();
			labelOpen.Text = _timeManager.OpenTime.ToString();
			labelClose.Text = _timeManager.CloseTime.ToString();

			if (InTime)
			{
				labelStatus.BackColor = System.Drawing.Color.Green;
				labelStatus.ForeColor = System.Drawing.Color.White;
				labelStatus.Text = "盤中時段";
			}
			else
			{
				labelStatus.BackColor = System.Drawing.Color.Gray;
				labelStatus.ForeColor = System.Drawing.Color.White;
				labelStatus.Text = "已收盤";
			}

		}

		void RenderConnectted(bool connectted)
		{
			if (connectted)
			{
				labelSource.BackColor = System.Drawing.Color.Green;
				labelSource.ForeColor = System.Drawing.Color.White;
				labelSource.Text = "連線中";
			}
			else
			{
				labelSource.BackColor = System.Drawing.Color.Gray;
				labelSource.ForeColor = System.Drawing.Color.White;
				labelStatus.Text = "已斷線";
			}

			_logger.LogInfo("OrderMaker Connectted: " + connectted.ToString());
		}

		#endregion

		#region  Helper

		bool OneMinute => rounds % (60 / seconds) == 0;

		#endregion

		private void Main_FormClosed(object sender, FormClosedEventArgs e)
		{

		}

		private async void Main_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				_source.DisConnect();

				_stockService.SaveTicksToDB();
				_futuresService.SaveTicksToDB();

				//更新權值股
				var stocks = _stockService.GetStocks(updatePrice:true);
				await _apiService.SaveStockPrices(stocks);
				
			}
			catch (Exception ex)
			{

				_logger.LogException(ex);
			}
			
		}

	}
}
