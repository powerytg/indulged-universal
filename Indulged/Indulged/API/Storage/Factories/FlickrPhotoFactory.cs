using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Indulged.API.Storage;
using Indulged.API.Storage.Models;

namespace Indulged.API.Storage.Factories
{
    public class FlickrPhotoFactory
    {
        public static FlickrPhoto PhotoWithJObject(JObject json)
        {
            string photoId = json["id"].ToString();
            FlickrPhoto photo = null;
            if (StorageService.Instance.PhotoCache.ContainsKey(photoId))
            {
                photo = StorageService.Instance.PhotoCache[photoId];
            }
            else
            {
                photo = new FlickrPhoto();
                photo.ResourceId = photoId;
                StorageService.Instance.PhotoCache[photoId] = photo;
            }

            JToken ownerValue;
            if (json.TryGetValue("owner", out ownerValue))
            {
                photo.UserId = json["owner"].ToString();

                // Parse user
                FlickrUser user = FlickrUserFactory.UserWithJObject(json);
            }
            else
            {
            }
            
            photo.Secret = json["secret"].ToString();
            photo.Server = json["server"].ToString();
            photo.Farm = json["farm"].ToString();
            photo.ViewCount = int.Parse(json["views"].ToString());

            JToken commentCountValue;
            if(json.TryGetValue("comments", out commentCountValue))
            {
                photo.CommentCount = int.Parse(json["comments"]["_content"].ToString());
            }

            if(json["title"].GetType() == typeof(JValue))
                photo.Title = json["title"].ToString();
            else
                photo.Title = json["title"]["_content"].ToString();                
            
            photo.Description = json["description"]["_content"].ToString();

            JToken licenseValue;
            if (json.TryGetValue("license", out licenseValue))
            {
                photo.LicenseId = json["license"].ToString();
            }

            JToken widthValue;
            if (json.TryGetValue("o_width", out widthValue))
            {
                photo.OriginalWidth = int.Parse(json["o_width"].ToString());
            }

            JToken heightValue;
            if (json.TryGetValue("o_height", out heightValue))
            {
                photo.OriginalHeight = int.Parse(json["o_height"].ToString());
            }

            if (json.TryGetValue("width_l", out widthValue))
            {
                photo.LargeWidth = int.Parse(json["width_l"].ToString());
            }

            if (json.TryGetValue("height_l", out heightValue))
            {
                photo.LargeHeight = int.Parse(json["height_l"].ToString());
            }

            if (json.TryGetValue("width_m", out widthValue))
            {
                photo.MediumWidth = int.Parse(json["width_m"].ToString());
            }

            if (json.TryGetValue("height_m", out heightValue))
            {
                photo.MediumHeight = int.Parse(json["height_m"].ToString());
            }

            // Tags
            photo.Tags = new List<string>();
            JToken tagsValue;
            if (json.TryGetValue("tags", out tagsValue))
            {
                string tagsString = json["tags"].ToString();
                if (tagsString.Length != 0)
                {
                    photo.Tags = new List<string>(tagsString.Split(' '));
                }
                
            }

            // Favourite
            JToken favValue;
            if(json.TryGetValue("isfavorite", out favValue))
            {
                photo.IsFavourite = (json["isfavorite"].ToString() == "1");
            }

            return photo;

        }

        public static FlickrPhoto PhotoWithPhotoActivityJObject(JObject json)
        {
            string photoId = json["id"].ToString();
            FlickrPhoto photo = null;
            if (StorageService.Instance.PhotoCache.ContainsKey(photoId))
            {
                photo = StorageService.Instance.PhotoCache[photoId];
            }
            else
            {
                photo = new FlickrPhoto();
                photo.ResourceId = photoId;
                StorageService.Instance.PhotoCache[photoId] = photo;
            }

            JToken ownerValue;
            if (json.TryGetValue("owner", out ownerValue))
            {
                photo.UserId = json["owner"].ToString();

                // Parse user
                FlickrUser user = FlickrUserFactory.UserWithJObject(json);
            }
            else
            {
            }

            photo.Secret = json["secret"].ToString();
            photo.Server = json["server"].ToString();
            photo.Farm = json["farm"].ToString();
            photo.ViewCount = int.Parse(json["views"].ToString());
            
            if (json["title"].GetType() == typeof(JValue))
                photo.Title = json["title"].ToString();
            else
                photo.Title = json["title"]["_content"].ToString();

            return photo;
        }

        public static FlickrPhoto PhotoWithPhotoInfoJObject(JObject json)
        {
            string photoId = json["id"].ToString();
            FlickrPhoto photo = null;
            if (StorageService.Instance.PhotoCache.ContainsKey(photoId))
            {
                photo = StorageService.Instance.PhotoCache[photoId];
            }
            else
            {
                photo = new FlickrPhoto();
                photo.ResourceId = photoId;
                StorageService.Instance.PhotoCache[photoId] = photo;
            }

            photo.Secret = json["secret"].ToString();
            photo.Server = json["server"].ToString();
            photo.Farm = json["farm"].ToString();
            photo.ViewCount = int.Parse(json["views"].ToString());

            JToken commentCountValue;
            if (json.TryGetValue("comments", out commentCountValue))
            {
                photo.CommentCount = int.Parse(json["comments"]["_content"].ToString());
            }

            photo.Title = json["title"]["_content"].ToString();
            photo.Description = json["description"]["_content"].ToString();

            // Favourite
            JToken favValue;
            if (json.TryGetValue("isfavorite", out favValue))
            {
                photo.IsFavourite = (json["isfavorite"].ToString() == "1");
            }

            return photo;

        }

    }
}
