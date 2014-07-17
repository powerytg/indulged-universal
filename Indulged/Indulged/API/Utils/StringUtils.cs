using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Utils
{
    public static class StringUtils
    {
        public static Dictionary<string, string> ParseQueryString(this string uri)
        {
            var query = (uri.IndexOf('?') > -1) ? uri.Substring(uri.IndexOf('?') + 1) : uri;
            var parts = query.Split('&');

            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var data in parts.Select(s => s.Split('=')))
            {
                result.Add(data[0], data[1]);
            }

            return result;
        }

        public static bool ParseBool(this string str)
        {
            if (str == "1")
                return true;
            else
                return false;
        }
    }
}
