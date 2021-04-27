using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp
{
    public interface ITimeManager
    {
        bool InTime { get; }
        DateTime OpenTime { get; }
        DateTime CloseTime { get; }

        List<int> KLineTimes { get; }
    }


    public class TimeManager : ITimeManager
    {
        private DateTime _openTime;
        private DateTime _closeTime;

        private readonly List<int> _kLineTimes;

        public TimeManager(string open, string close)
        {
            var now = DateTime.Now;
            var openTimes = open.ToTimes();
            _openTime = new DateTime(now.Year, now.Month, now.Day, openTimes[0], openTimes[1], openTimes[2]);

            var closeTimes = close.ToTimes();
            _closeTime = new DateTime(now.Year, now.Month, now.Day, closeTimes[0], closeTimes[1], closeTimes[2]);

            if (_closeTime <= _openTime) _closeTime = _closeTime.AddDays(1);


            var times = new List<int>();
            DateTime time = _openTime.AddMinutes(1);
            while (time <= _closeTime)
            {
                times.Add(time.ToTimeNumber());
                time = time.AddMinutes(1);
            }
            this._kLineTimes = times;
        }

        public DateTime OpenTime => _openTime;
        public DateTime CloseTime => _closeTime;

        public bool InTime => DateTime.Now >= _openTime && DateTime.Now <= _closeTime;

        public List<int> KLineTimes => _kLineTimes;
    }
}
