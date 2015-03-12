using Indulged.API.Utils;
using Indulged.API.Networking.Events;
using Indulged.API.Storage.Events;
using Indulged.API.Storage.Factories;
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
        public StorageEventArgs OnGroupSearchResult(string response)
        {
            JObject json = JObject.Parse(response);
            JObject groupJson = (JObject)json["groups"];

            int page = int.Parse(groupJson["page"].ToString());
            int perPage = int.Parse(groupJson["perpage"].ToString());
            int numTotal = int.Parse(groupJson["total"].ToString());

            List<FlickrGroup> groups = new List<FlickrGroup>();
            foreach (var js in groupJson["group"])
            {
                FlickrGroup group = FlickrGroupFactory.GroupWithJObject((JObject)js);
                groups.Add(group);
            }

            var evt = new StorageEventArgs();
            evt.Page = page;
            evt.PerPage = perPage;
            evt.TotalCount = numTotal;
            evt.NewGroups = groups;
            return evt;
        }
    }
}
