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
        // Discovery stream returned
        private void OnDiscoveryStreamReturned(object sender, APIEventArgs e)
        {
            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["photos"];
            DiscoveryStream.PhotoCount = int.Parse(rootJson["total"].ToString());
            int page = int.Parse(rootJson["page"].ToString());
            int numPages = int.Parse(rootJson["pages"].ToString());
            int perPage = int.Parse(rootJson["perpage"].ToString());

            List<FlickrPhoto> newPhotos = new List<FlickrPhoto>();
            foreach (var entry in rootJson["photo"])
            {
                JObject json = (JObject)entry;
                FlickrPhoto photo = FlickrPhotoFactory.PhotoWithJObject(json);

                if (!DiscoveryStream.Photos.Contains(photo))
                {
                    DiscoveryStream.Photos.Add(photo);
                    newPhotos.Add(photo);
                }
            }

            // Dispatch event
            var args = new StorageEventArgs();
            args.Page = page;
            args.PageCount = numPages;
            args.PerPage = perPage;
            args.NewPhotos = newPhotos;
            args.UpdatedStream = DiscoveryStream;
            PhotoStreamUpdated.DispatchEvent(this, args);
        }

        // Favourite stream returned
        private void OnFavouriteStreamReturned(object sender, APIEventArgs e)
        {
            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["photos"];
            FavouriteStream.PhotoCount = int.Parse(rootJson["total"].ToString());
            int page = int.Parse(rootJson["page"].ToString());
            int numPages = int.Parse(rootJson["pages"].ToString());
            int perPage = int.Parse(rootJson["perpage"].ToString());

            List<FlickrPhoto> newPhotos = new List<FlickrPhoto>();
            foreach (var entry in rootJson["photo"])
            {
                JObject json = (JObject)entry;
                FlickrPhoto photo = FlickrPhotoFactory.PhotoWithJObject(json);

                if (!FavouriteStream.Photos.Contains(photo))
                {
                    FavouriteStream.Photos.Add(photo);
                    newPhotos.Add(photo);
                }
            }

            // Dispatch event
            var args = new StorageEventArgs();
            args.Page = page;
            args.PageCount = numPages;
            args.PerPage = perPage;
            args.NewPhotos = newPhotos;
            args.UpdatedStream = FavouriteStream;
            PhotoStreamUpdated.DispatchEvent(this, args);
        }

        private void OnAlbumStreamUpdated(object sender, APIEventArgs e)
        {
            if (!AlbumCache.ContainsKey(e.AlbumId))
                return;

            FlickrAlbum photoset = AlbumCache[e.AlbumId];

            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["photoset"];
            int TotalCount = int.Parse(rootJson["total"].ToString());
            int page = int.Parse(rootJson["page"].ToString());
            int numPages = int.Parse(rootJson["pages"].ToString());
            int perPage = int.Parse(rootJson["perpage"].ToString());

            List<FlickrPhoto> newPhotos = new List<FlickrPhoto>();
            foreach (var entry in rootJson["photo"])
            {
                JObject json = (JObject)entry;
                FlickrPhoto photo = FlickrPhotoFactory.PhotoWithJObject(json);

                if (!photoset.PhotoStream.Photos.Contains(photo))
                {
                    photoset.PhotoStream.Photos.Add(photo);
                    newPhotos.Add(photo);
                }
            }

            // Dispatch event
            var evt = new StorageEventArgs();
            evt.AlbumId = photoset.ResourceId;
            evt.Page = page;
            evt.PageCount = numPages;
            evt.PerPage = perPage;
            evt.NewPhotos = newPhotos;
            evt.UpdatedStream = photoset.PhotoStream;
            PhotoStreamUpdated.DispatchEvent(this, evt);
        }

        // Photo stream retrieved for a user
        private void OnUserPhotoStreamReturned(object sender, APIEventArgs e)
        {
            // Find the user
            if (!UserCache.ContainsKey(e.UserId))
                return;

            var user = UserCache[e.UserId];

            JObject rawJson = JObject.Parse(e.Response);
            JObject rootJson = (JObject)rawJson["photos"];
            user.PhotoStream.PhotoCount = int.Parse(rootJson["total"].ToString());
            int page = int.Parse(rootJson["page"].ToString());
            int numPages = int.Parse(rootJson["pages"].ToString());
            int perPage = int.Parse(rootJson["perpage"].ToString());

            var newPhotos = new List<FlickrPhoto>();
            foreach (var entry in rootJson["photo"])
            {
                JObject json = (JObject)entry;
                var photo = FlickrPhotoFactory.PhotoWithJObject(json);

                if (!user.PhotoStream.Photos.Contains(photo))
                {
                    user.PhotoStream.Photos.Add(photo);
                    newPhotos.Add(photo);
                }
            }

            // Dispatch event
            var evt = new StorageEventArgs();
            evt.Page = page;
            evt.PageCount = numPages;
            evt.PerPage = perPage;
            evt.NewPhotos = newPhotos;
            evt.UserId = e.UserId;
            evt.UpdatedStream = user.PhotoStream;
            PhotoStreamUpdated.DispatchEvent(this, evt);
        }
    }
}
