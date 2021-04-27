using Core.Helpers;
using Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp
{
    public interface IDateManager
    {
        int DateNum { get;  }
        bool BusinessDay { get; }
    }

    public class DateManager : IDateManager
    {
        private readonly int _dateNum;
        private readonly bool _isBusinessDay;

        public DateManager(List<DayViewModel> holidays)
        {
            
            _dateNum = DateTime.Now.ToDateNumber();

            _isBusinessDay = IsBusinessDay(holidays);
        }

        public int DateNum => _dateNum;

        private bool IsBusinessDay(List<DayViewModel> holidays)
        {
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday) return false;
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday) return false;

            if (holidays.IsNullOrEmpty()) return true;

            var match = holidays.Where(d => d.date == _dateNum).FirstOrDefault();
            return match == null;
        } 

        public bool BusinessDay => _isBusinessDay;

    }

}
