using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Views;

namespace Core.Helpers
{
	public static class QuoteHelpers
	{
		public static bool IsKLineStart(this Tick tick)
		{
			return (tick.time % 100 ) == 0;
		}

		public static int[] ResolveBeginEndTime(this int time)
		{
			var today = DateTime.Today;
			var endTimes = time.ToString().ToTimes();

			var end = new DateTime(today.Year, today.Month, today.Day, endTimes[0], endTimes[1], endTimes[2]);
			var begin = end.AddMinutes(-1);

			return new int[] { begin.ToTimeNumber(), end.ToTimeNumber() };

		}

		public static int ToKLineTime(this int time)
		{
			var today = DateTime.Today;
			var endTimes = time.ToString().ToTimes();
			int second = endTimes[2];
			var end = new DateTime(today.Year, today.Month, today.Day, endTimes[0], endTimes[1], second);

			if (second == 0) return end.AddMinutes(1).ToTimeNumber();

			return end.AddMinutes(1).AddSeconds(0 - second).ToTimeNumber();


		}

	}
}
