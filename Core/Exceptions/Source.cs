using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
	public class SourceException : Exception
	{
		public SourceException(string msg = "") : base(msg)
		{

		}
	}
}
