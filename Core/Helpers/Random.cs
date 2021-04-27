using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class RandomHelpers
    {
        public static bool NextBoolean(this Random random)
        {
            return random.Next() > (Int32.MaxValue / 2);
        }
    }
   

}
