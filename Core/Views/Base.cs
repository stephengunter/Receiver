using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Views
{
	public class BaseOption
	{
		public string value { get; set; }
		public string text { get; set; }

        public override string ToString()
        {
            return text;
        }
    }
}
