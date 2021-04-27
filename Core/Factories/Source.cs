using Core.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Factories
{
	public class SourceFactory
	{
		public static ISource Create(string name, string sid, string password)
		{
			return new Capital(sid, password);
		}
	}
}
