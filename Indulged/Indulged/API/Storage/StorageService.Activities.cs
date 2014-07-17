using Indulged.API.Networking.Events;
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
        // Activity stream retrieved for a user
        private void OnActivityStreamReturned(object sender, APIEventArgs e)
        {
            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["items"];
            ActivityItemsCount = int.Parse(rootJson["total"].ToString());

            ActivityList.Clear();
            foreach (var entry in rootJson["item"])
            {
                JObject json = (JObject)entry;
                FlickrPhotoActivity activity = FlickrActivityFactory.ActivityWithJObject(json);

                if (activity == null)
                    continue;

                if (!ActivityList.Contains(activity))
                {
                    ActivityList.Add(activity);
                }
            }

            // Dispatch event
            if (ActivityStreamUpdated != null)
                ActivityStreamUpdated(this, null);
        }
    }
}
