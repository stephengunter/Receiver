using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views
{
	public abstract class BaseAdminRequest
	{
		public string Key { get; set; }

	}

	public class DBAdminRequest : BaseAdminRequest
	{


	}
}
