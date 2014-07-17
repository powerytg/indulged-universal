using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Indulged.API.Utils;
using Indulged.API.Storage.Models;

namespace Indulged.API.Storage.Factories
{
    public class FlickrAlbumFactory
    {
        public static FlickrAlbum PhotoSetWithJObject(JObject json)
        {
            string setId = json["id"].ToString();
            FlickrAlbum photoset;
            if (StorageService.Instance.AlbumCache.ContainsKey(setId))
            {
                photoset = StorageService.Instance.AlbumCache[setId];
            }
            else
            {
                photoset = new FlickrAlbum(setId);
                StorageService.Instance.AlbumCache[setId] = photoset;
            }

            photoset.Primary = json["primary"].ToString();
            photoset.Secret = json["secret"].ToString();
            photoset.Server = json["server"].ToString();
            photoset.Farm = json["farm"].ToString();
            photoset.PhotoStream.PhotoCount = int.Parse(json["photos"].ToString());
            photoset.PhotoStream.VideoCount = int.Parse(json["videos"].ToString());
            photoset.Title = json["title"]["_content"].ToString();
            photoset.Description = json["description"]["_content"].ToString();
            photoset.IsVisible = json["visibility_can_see_set"].ToString().ParseBool();
            photoset.ViewCount = int.Parse(json["count_views"].ToString());
            photoset.CommentCount = int.Parse(json["count_comments"].ToString());
            photoset.CanComment = json["can_comment"].ToString().ParseBool();
            photoset.CreationDate = json["date_create"].ToString().ToDateTime();
            photoset.UpdatedDate = json["date_update"].ToString().ToDateTime();

            // Create the primary photo
            if (StorageService.Instance.PhotoCache.ContainsKey(photoset.Primary))
            {
                photoset.PrimaryPhoto = StorageService.Instance.PhotoCache[photoset.Primary];
            }
            else
            {
                var primaryPhoto = new FlickrPhoto();
                primaryPhoto.ResourceId = photoset.Primary;
                primaryPhoto.Secret = photoset.Secret;
                primaryPhoto.Server = photoset.Server;
                primaryPhoto.Farm = photoset.Farm;

                photoset.PrimaryPhoto = primaryPhoto;
                StorageService.Instance.PhotoCache[photoset.Primary] = primaryPhoto;
            }

            return photoset;

        }
    }
}
