using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Indulged.API.Storage.Factories
{
    public class FlickrPhotoEXIFFactory
    {
        public static Dictionary<string, string> EXIFWithJObject(JObject json)
        {
            if (json == null)
                return null;

            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (JObject entry in json["exif"])
            {
                string label = entry["label"].ToString();
                string content;

                JToken cleanObject;
                if (entry.TryGetValue("clean", out cleanObject))
                {
                    content = entry["clean"]["_content"].ToString();
                }
                else
                {
                    content = entry["raw"]["_content"].ToString();
                }

                result[label] = content;
            }

            return result;
        }
    }
}
