﻿using Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Indicators
{
	public interface IIndicator
	{
		DataViewModel Calculate(int begin, int end);
	}

}
