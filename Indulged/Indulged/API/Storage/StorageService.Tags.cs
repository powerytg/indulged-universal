using Indulged.API.Utils;
using Indulged.API.Networking.Events;
using Indulged.API.Storage.Events;
using Indulged.API.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage
{
    public partial class StorageService
    {
        private void OnPopularTagListReturned(object sender, APIEventArgs e)
        {
            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["hottags"];

            List<FlickrTag> result = new List<FlickrTag>();
            foreach (JObject json in rootJson["tag"])
            {
                FlickrTag tag = new FlickrTag();
                tag.Name = json["_content"].ToString();
                tag.Weight = int.Parse(json["score"].ToString());
                result.Add(tag);
            }

            var evt = new StorageEventArgs();
            evt.NewTags = result;
            PopularTagsUpdated.DispatchEvent(this, evt);
        }
    }
}
