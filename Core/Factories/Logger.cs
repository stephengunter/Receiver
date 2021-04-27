using Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Factories
{
	public class LoggerFactory
	{
		public static ILogger Create(string filePath)
		{
			return new Logger(filePath);
		}
	}
}
