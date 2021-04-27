using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
	public class OrderError : Exception
	{
		public OrderError()
		{

		}

		public OrderError(string code = "", string msg = "") : base($"errCode: {code} , errMsg: {msg}")
		{

		}
	}


}
