using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp
{
    public interface IHubManager
    {
        void InvokeQuote();
    }


    public class HubManager : IHubManager
    {
        private readonly HubConnection _connection;
        private readonly string _quoteKey;

        public HubManager(string url, string quoteKey)
        {
            _quoteKey = quoteKey;

            _connection = new HubConnectionBuilder()
               .WithUrl(url)
               .Build();

            _connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };

            Task.Run(() => _connection.StartAsync()).Wait();
        }

        public void InvokeQuote()
        {
            Task.Run(() => _connection.InvokeAsync("Quote", _quoteKey)).Wait();
        }
    }
}
