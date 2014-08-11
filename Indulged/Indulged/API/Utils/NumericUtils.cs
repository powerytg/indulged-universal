using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Utils
{
    public static class NumericUtils
    {
        public static string ToShortString(this int value)
        {
            if (value < 1000)
                return value.ToString();

            if (value >= 1000 && value < 1000000)
                return (value / 1000).ToString() + "k";

            return (value / 1000000).ToString() + "m";
        }
    }
}
