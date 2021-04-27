using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Views;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Core.Models;


namespace Core.Services
{
	public interface IApiService
	{
		Task<List<DayViewModel>> GetHolidays();

		Task Login();
		Task RefreshToken();
		Task<List<StockViewModel>> GetStocks();
		Task SaveStockPrices(IEnumerable<StockViewModel> models);

		Task<List<IndicatorViewModel>> GetIndicators();
		Task CreateQuote(QuoteViewModel model);

		Task MoveRealTimeToData();
	}

	public class ApiService : IApiService
	{
		private readonly string baseUrl;
		private readonly string email;
		private readonly string password;
		private readonly string dbKey;

		string token;
		string refreshToken;

		public ApiService(string baseUrl, string email, string password, string dbKey)
		{
			this.baseUrl = baseUrl;
			this.email = email;
			this.password = password;
			this.dbKey = dbKey;
		}

		public async Task<List<DayViewModel>> GetHolidays()
		{
			string year = DateTime.Today.Year.ToString();
			var url = $"api/days?year={year}";
			var result = await GetResponseStringAsync(url);

			return JsonConvert.DeserializeObject<List<DayViewModel>>(result);
		}

		public async Task Login()
		{
			var url = "api/auth/login";

			var result = await PostResponseStringAsync(url, new LoginRequest {
				email = this.email, password = this.password
			});

			var model = JsonConvert.DeserializeObject<AuthResponse>(result);

			this.token = model.accessToken.token;
			this.refreshToken = model.refreshToken;

		}

		public async Task RefreshToken()
		{
			var url = "api/auth/refreshtoken";

			var result = await PostResponseStringAsync(url, new Core.Views.RefreshTokenRequest
			{
				accessToken = this.token,
				refreshToken = this.refreshToken
			});

			var model = JsonConvert.DeserializeObject<AuthResponse>(result);

			this.token = model.accessToken.token;
			this.refreshToken = model.refreshToken;

		}

		public async Task<List<StockViewModel>> GetStocks()
		{
			var url = "admin/stocks";
			var result = await GetResponseStringAsync(url);

			var pageList = JsonConvert.DeserializeObject<PagedList<Stock,StockViewModel>>(result);

			return pageList.ViewList;
		}

		public async Task SaveStockPrices(IEnumerable<StockViewModel> models)
		{
			var url = $"admin/stocks/update";
			var result = await PostResponseStringAsync(url, models);
		}

		public async Task<List<IndicatorViewModel>> GetIndicators()
		{
			var url = "admin/indicators/get";
			var result = await GetResponseStringAsync(url);

			return JsonConvert.DeserializeObject<List<IndicatorViewModel>>(result);
		}

		public async Task CreateQuote(QuoteViewModel model)
		{
			var url = "admin/realtime";

			var result = await PostResponseStringAsync(url, model);

		}

		public async Task MoveRealTimeToData()
		{
			var url = "admin/db/realtime";
			var model = new DBAdminRequest { Key = dbKey };
			var result = await PostResponseStringAsync(url, model);
		}


		HttpClient CreateHttpClient()
		{
			var client = new HttpClient
			{
				BaseAddress = new Uri(baseUrl)
			};

			if (!String.IsNullOrEmpty(token)) client.SetBearerToken(token);

			return client;
		}

		async Task<string> GetResponseStringAsync(string action)
		{
			var client = CreateHttpClient();

			var response = await client.GetAsync(action);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
		}

		async Task<string> PostResponseStringAsync(string action, object model)
		{
			var client = CreateHttpClient();
			string json = JsonConvert.SerializeObject(model);

			HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");			


			var response = await client.PostAsync(action, contentPost);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();

		}

		async Task<string> PutResponseStringAsync(string action, object model)
		{
			var client = CreateHttpClient();
			string json = JsonConvert.SerializeObject(model);

			HttpContent contentPost = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync(action, contentPost);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();

		}

		
	}
}
