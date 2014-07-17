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
        private void OnPhotoSearchResult(object sender, APIEventArgs e)
        {
            JObject json = JObject.Parse(e.Response);
            JObject photosetJson = (JObject)json["photos"];

            int page = int.Parse(photosetJson["page"].ToString());
            int perPage = int.Parse(photosetJson["perpage"].ToString());
            int numTotal = int.Parse(photosetJson["total"].ToString());

            List<FlickrPhoto> photos = new List<FlickrPhoto>();
            foreach (var photoJson in photosetJson["photo"])
            {
                FlickrPhoto photo = FlickrPhotoFactory.PhotoWithJObject((JObject)photoJson);
                photos.Add(photo);
            }

            var evt = new StorageEventArgs();
            evt.SessionId = e.SessionId;
            evt.Page = page;
            evt.PerPage = perPage;
            evt.TotalCount = numTotal;
            evt.NewPhotos = photos;
            PhotoSearchCompleted.DispatchEvent(this, evt);
        }

        private void OnGroupSearchResult(object sender, APIEventArgs e)
        {
            JObject json = JObject.Parse(e.Response);
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
            evt.SessionId = e.SessionId;
            evt.Page = page;
            evt.PerPage = perPage;
            evt.TotalCount = numTotal;
            evt.NewGroups = groups;
            GroupSearchCompleted.DispatchEvent(this, evt);
        }
    }
}
