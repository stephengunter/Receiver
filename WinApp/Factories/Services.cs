using Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp
{
    public class Factories
    {
        public static ISettingsManager CreateSettingsManager() => new SettingsManager();

        public static IHubManager CreateHubManager(string url, string quoteKey) => new HubManager(url, quoteKey);

        public static ITimeManager CreateTimeManager(string begin, string end) => new TimeManager(begin, end);

        public static IDateManager CreateDateManager(List<DayViewModel> holidays) => new DateManager(holidays);
    }
}
