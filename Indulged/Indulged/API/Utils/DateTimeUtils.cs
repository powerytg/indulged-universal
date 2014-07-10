using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Utils
{
    public static class DateTimeUtils
    {
        public static string ToTimestampString(this DateTime date)
        {
            double seconds = (DateTime.Now - date).TotalSeconds;
            if (seconds < 60)
                return "1m";
            else if (seconds >= 60 && seconds < 3600)
                return Math.Floor(seconds / 60).ToString() + "m";
            else if (seconds >= 3600 && seconds < 86400)
                return Math.Floor(seconds / 3600).ToString() + "h";
            else if (seconds >= 86400 && seconds < 86400 * 30)
                return Math.Floor(seconds / 86400).ToString() + "d";
            else
            {
                return date.Month.ToString() + "/" + date.Day.ToString();
            }

        }

        public static String GetTimestamp()
        {
            int epoch = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            return epoch.ToString();
        }

        public static DateTime ToDateTime(this string timestamp)
        {
            double tp = double.Parse(timestamp);
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(tp);
        }

    }
}
