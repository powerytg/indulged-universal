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
using Indulged.API.Storage.Factories;

namespace Indulged.API.Storage
{
    public partial class StorageService
    {
        // Group list retrieved for a user
        public void OnCurrentUserGroupListReturned(string response)
        {            
            JObject rawJson = JObject.Parse(response);
            JObject rootJson = (JObject)rawJson["groups"];

            List<FlickrGroup> result = new List<FlickrGroup>();
            CurrentUser.GroupIds = new List<string>();
            foreach (var entry in rootJson["group"])
            {
                JObject json = (JObject)entry;
                FlickrGroup group = FlickrGroupFactory.GroupWithJObject(json);
                CurrentUser.GroupIds.Add(group.ResourceId);
                result.Add(group);
            }
        }

        // User info returned
        private void OnUserInfoReturned(object sender, APIEventArgs e)
        {
            JObject json = JObject.Parse(e.Response);
            var user = FlickrUserFactory.UserWithUserInfoJObject(json);

            // Dispatch event
            var evt = new StorageEventArgs();
            evt.UserId = e.UserId;
            UserInfoUpdated.DispatchEvent(this, evt);
        }

        // Contact list returned
        private void OnContactListReturned(object sender, APIEventArgs e)
        {
            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["contacts"];
            ContactCount = int.Parse(rootJson["total"].ToString());
            int page = int.Parse(rootJson["page"].ToString());
            int numPages = int.Parse(rootJson["pages"].ToString());
            int perPage = int.Parse(rootJson["perpage"].ToString());

            var newUsers = new List<FlickrUser>();
            if (ContactCount > 0)
            {
                foreach (var entry in rootJson["contact"])
                {
                    JObject json = (JObject)entry;
                    var contact = FlickrUserFactory.ContactWithJObject(json);

                    if (!ContactList.Contains(contact))
                    {
                        ContactList.Add(contact);
                        newUsers.Add(contact);
                    }
                }
            }

            // Dispatch event
            var evt = new StorageEventArgs();
            evt.Page = page;
            evt.PageCount = numPages;
            evt.PerPage = perPage;
            evt.NewUsers = newUsers;
            ContactListUpdated.DispatchEvent(this, evt);
        }

        private void OnContactPhotosReturned(object sender, APIEventArgs e)
        {
            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["photos"];

            ContactStream.Photos.Clear();
            foreach (var entry in rootJson["photo"])
            {
                JObject json = (JObject)entry;
                var photo = FlickrPhotoFactory.PhotoWithJObject(json);
                ContactStream.Photos.Add(photo);
            }

            // Dispatch event
            if (ContactPhotosUpdated != null)
                ContactPhotosUpdated(this, null);
        }

    }
}
