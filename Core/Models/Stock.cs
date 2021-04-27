using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	public class Stock
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public double Price { get; set; }

		public double Weight { get; set; }

		public bool Base { get; set; }

		public bool Ignore { get; set; }


	}
}
