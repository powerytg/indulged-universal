using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage
{
    public partial class StorageService
    {
        private string containerKey = "user";

        public void SaveCurrentUserInfo()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (!settings.Containers.ContainsKey(containerKey))
            {
                settings.CreateContainer(containerKey, Windows.Storage.ApplicationDataCreateDisposition.Always);
            }

            var values = settings.Containers[containerKey].Values;

            if (values.ContainsKey("currentUserId"))
            {
                values["currentUserId"] = CurrentUser.ResourceId;
            }
            else
            {
                values.Add("currentUserId", CurrentUser.ResourceId);
            }

            Debug.WriteLine("current user id saved: " + CurrentUser.ResourceId);

            if (values.ContainsKey("currentUserName"))
            {
                values["currentUserName"] = CurrentUser.Name;
            }
            else
            {
                values.Add("currentUserName", CurrentUser.Name);
            }

            Debug.WriteLine("current user name saved: " + CurrentUser.Name);

            if (values.ContainsKey("currentUserUserName"))
            {
                values["currentUserUserName"] = CurrentUser.UserName;
            }
            else
            {
                values.Add("currentUserUserName", CurrentUser.UserName);
            }

            Debug.WriteLine("current user username saved: " + CurrentUser.UserName);
        }

        public FlickrUser RetrieveCurrentUserInfo()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (!settings.Containers.ContainsKey(containerKey))
            {
                return null;
            }

            string userId;
            var values = settings.Containers[containerKey].Values;
            if (values.ContainsKey("currentUserId"))
            {
                userId = values["currentUserId"] as string;
                Debug.WriteLine("current user id retrieved: " + userId);
            }
            else
            {
                Debug.WriteLine("current user id not found");
                return null;
            }

            var currentUser = new FlickrUser(userId);

            values = settings.Containers[containerKey].Values;
            if (values.ContainsKey("currentUserName"))
            {
                currentUser.Name = values["currentUserName"] as string;
                Debug.WriteLine("current user name retrieved: " + currentUser.Name);
            }
            else
            {
                Debug.WriteLine("current user name not found");
                return null;
            }

            if (values.ContainsKey("currentUserUserName"))
            {
                currentUser.UserName = values["currentUserUserName"] as string;
                Debug.WriteLine("current user username retrieved: " + currentUser.UserName);
            }
            else
            {
                Debug.WriteLine("current user username not found");
                return null;
            }

            CurrentUser = currentUser;
            UserCache[userId] = currentUser;
            return currentUser;
        }

        public void ClearAcessCredentials()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            settings.DeleteContainer(containerKey);
        }
    }
}
