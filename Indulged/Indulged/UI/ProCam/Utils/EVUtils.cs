using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.UI.ProCam.Utils
{
    public static class EVUtils
    {
        public static string ToEVString(this float ev)
        {
            if (ev == 0)
            {
                return "0 EV";
            }
            else if (ev > 0)
            {
                double displayEV = ev / 3.0f;
                return "+" + displayEV.ToString("F1") + " EV";
            }
            else
            {
                double displayEV = ev / 3.0f;
                return displayEV.ToString("F1") + " EV";
            }
        }
    }
}
