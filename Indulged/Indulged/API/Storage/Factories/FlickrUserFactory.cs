using Indulged.API.Storage.Models;
using Indulged.API.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Factories
{
    public class FlickrUserFactory
    {
        public static FlickrUser UserWithJObject(JObject json)
        {
            FlickrUser user = null;
            string userId = json["owner"].ToString();
            if (StorageService.Instance.UserCache.ContainsKey(userId))
                user = StorageService.Instance.UserCache[userId];
            else
            {
                user = new FlickrUser(userId);
                StorageService.Instance.UserCache[user.ResourceId] = user;
            }

            JToken nameValue;
            if (json.TryGetValue("ownername", out nameValue))
            {
                user.Name = json["ownername"].ToString();
                user.PhotoStream.Name = user.Name + "'s Photos";
            }

            JToken farmValue;
            if (json.TryGetValue("iconfarm", out farmValue))
            {
                user.Farm = json["iconfarm"].ToString();
            }

            JToken serverValue;
            if (json.TryGetValue("iconserver", out serverValue))
            {
                user.Server = json["iconserver"].ToString();
            }


            return user;
        }

        public static FlickrUser UserWithTopicJObject(JObject json)
        {
            FlickrUser user = null;
            string userId = json["author"].ToString();
            if (StorageService.Instance.UserCache.ContainsKey(userId))
                user = StorageService.Instance.UserCache[userId];
            else
            {
                user = new FlickrUser(userId);
                StorageService.Instance.UserCache[user.ResourceId] = user;
            }

            JToken nameValue;
            if (json.TryGetValue("authorname", out nameValue))
            {
                user.Name = json["authorname"].ToString();
                user.PhotoStream.Name = user.Name + "'s Photos";
            }

            JToken farmValue;
            if (json.TryGetValue("iconfarm", out farmValue))
            {
                user.Farm = json["iconfarm"].ToString();
            }

            JToken serverValue;
            if (json.TryGetValue("iconserver", out serverValue))
            {
                user.Server = json["iconserver"].ToString();
            }


            return user;
        }

        public static FlickrUser UserWithPhotoCommentJObject(JObject json)
        {
            FlickrUser user = null;
            string userId = json["author"].ToString();
            if (StorageService.Instance.UserCache.ContainsKey(userId))
                user = StorageService.Instance.UserCache[userId];
            else
            {
                user = new FlickrUser(userId);
                StorageService.Instance.UserCache[user.ResourceId] = user;
            }

            JToken nameValue;
            if (json.TryGetValue("authorname", out nameValue))
            {
                user.Name = json["authorname"].ToString();
                user.PhotoStream.Name = user.Name + "'s Photos";
            }

            JToken farmValue;
            if (json.TryGetValue("iconfarm", out farmValue))
            {
                user.Farm = json["iconfarm"].ToString();
            }

            JToken serverValue;
            if (json.TryGetValue("iconserver", out serverValue))
            {
                user.Server = json["iconserver"].ToString();
            }


            return user;
        }

        public static FlickrUser UserWithUserInfoJObject(JObject rootJson)
        {
            JObject json = (JObject)rootJson["person"];

            FlickrUser user = null;
            string userId = json["id"].ToString();
            if (StorageService.Instance.UserCache.ContainsKey(userId))
                user = StorageService.Instance.UserCache[userId];
            else
            {
                user = new FlickrUser(userId);
                StorageService.Instance.UserCache[user.ResourceId] = user;
            }

            user.IsFullInfoLoaded = true;

            // Is pro user
            user.IsProUser = (json["ispro"].ToString() == "1");

            // User name
            JToken nameValue;
            if (json.TryGetValue("username", out nameValue))
            {
                user.Name = json["username"]["_content"].ToString();
                user.PhotoStream.Name = user.Name + "'s Photos";
            }

            JToken realNameValue;
            if (json.TryGetValue("realname", out realNameValue))
            {
                user.RealName = json["realname"]["_content"].ToString();
            }

            // Location
            JToken locationValue;
            if (json.TryGetValue("location", out locationValue))
            {
                user.Location = json["location"]["_content"].ToString();
            }

            // Description
            JToken descValue;
            if (json.TryGetValue("description", out descValue))
            {
                user.Description = json["description"]["_content"].ToString();
            }

            JToken farmValue;
            if (json.TryGetValue("iconfarm", out farmValue))
            {
                user.Farm = json["iconfarm"].ToString();
            }

            JToken serverValue;
            if (json.TryGetValue("iconserver", out serverValue))
            {
                user.Server = json["iconserver"].ToString();
            }

            // Profile url
            JToken profileUrlValue;
            if (json.TryGetValue("profileurl", out profileUrlValue))
            {
                user.ProfileUrl = json["profileurl"]["_content"].ToString();
            }

            // Photos section
            JObject photoJson = (JObject)json["photos"];

            JToken photoCountValue;
            if (photoJson.TryGetValue("count", out photoCountValue))
            {
                user.PhotoStream.PhotoCount = int.Parse(photoJson["count"]["_content"].ToString());
            }

            JToken firstDateValue;
            if (photoJson.TryGetValue("firstdate", out firstDateValue))
            {
                string dateString = photoJson["firstdate"]["_content"].ToString();
                if (dateString.Length > 0)
                {
                    user.FirstDate = dateString.ToDateTime();
                    user.hasFirstDate = true;
                }
                else
                    user.hasFirstDate = false;
            }

            return user;
        }

        public static FlickrUser UserWithActivityEventJObject(JObject json)
        {
            FlickrUser user = null;
            string userId = json["user"].ToString();
            if (StorageService.Instance.UserCache.ContainsKey(userId))
                user = StorageService.Instance.UserCache[userId];
            else
            {
                user = new FlickrUser(userId);
                StorageService.Instance.UserCache[user.ResourceId] = user;
            }

            JToken nameValue;
            if (json.TryGetValue("username", out nameValue))
            {
                user.Name = json["username"].ToString();
                user.PhotoStream.Name = user.Name + "'s Photos";
            }

            JToken farmValue;
            if (json.TryGetValue("iconfarm", out farmValue))
            {
                user.Farm = json["iconfarm"].ToString();
            }

            JToken serverValue;
            if (json.TryGetValue("iconserver", out serverValue))
            {
                user.Server = json["iconserver"].ToString();
            }


            return user;
        }

        public static FlickrUser ContactWithJObject(JObject json)
        {
            FlickrUser user = null;
            string userId = json["nsid"].ToString();
            if (StorageService.Instance.UserCache.ContainsKey(userId))
                user = StorageService.Instance.UserCache[userId];
            else
            {
                user = new FlickrUser(userId);
                StorageService.Instance.UserCache[user.ResourceId] = user;
            }

            JToken nameValue;
            if (json.TryGetValue("username", out nameValue))
            {
                user.Name = json["username"].ToString();
                user.PhotoStream.Name = user.Name + "'s Photos";
            }

            JToken farmValue;
            if (json.TryGetValue("iconfarm", out farmValue))
            {
                user.Farm = json["iconfarm"].ToString();
            }

            JToken serverValue;
            if (json.TryGetValue("iconserver", out serverValue))
            {
                user.Server = json["iconserver"].ToString();
            }


            return user;
        }
    }
}
