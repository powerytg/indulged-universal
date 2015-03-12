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
        public List<FlickrPhoto> OnPhotoStreamReturned(FlickrPhotoStream stream, string json)
        {
            if (stream.StreamType == FlickrPhotoStreamType.AlbumStream)
            {
                return OnAlbumStreamUpdated(stream.AlbumId, json);
            }
            else if (stream.StreamType == FlickrPhotoStreamType.ContactStream)
            {
                return null;
            }
            else if (stream.StreamType == FlickrPhotoStreamType.UserStream)
            {
                return OnUserPhotoStreamReturned(stream.UserId, json);
            }
            else if (stream.StreamType == FlickrPhotoStreamType.GroupStream)
            {
                return OnGroupPhotoStreamReturned(stream.GroupId, json);
            }
            else if (stream.StreamType == FlickrPhotoStreamType.DiscoveryStream)
            {
                return OnDiscoveryStreamReturned(json);
            }
            else if (stream.StreamType == FlickrPhotoStreamType.FavouriteStream)
            {
                return OnFavouriteStreamReturned(json);
            }
            else if (stream.StreamType == FlickrPhotoStreamType.SearchStream)
            {
                return OnSearchStreamResult(stream, json);
            }

            // Unsupported
            return null;
        }

        // Discovery stream returned
        private List<FlickrPhoto> OnDiscoveryStreamReturned(string response)
        {
            JObject rawJson = JObject.Parse(response);
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

            return newPhotos;
        }

        // Favourite stream returned
        private List<FlickrPhoto> OnFavouriteStreamReturned(string response)
        {
            JObject rawJson = JObject.Parse(response);
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

            return newPhotos;
        }

        private List<FlickrPhoto> OnAlbumStreamUpdated(string albumId, string response)
        {
            if (!AlbumCache.ContainsKey(albumId))
            {
                return new List<FlickrPhoto>();
            }
                
            FlickrAlbum photoset = AlbumCache[albumId];

            JObject rawJson = JObject.Parse(response);
            JObject rootJson = (JObject)rawJson["photoset"];
            int TotalCount = int.Parse(rootJson["total"].ToString());
            int page = int.Parse(rootJson["page"].ToString());
            int numPages = int.Parse(rootJson["pages"].ToString());
            int perPage = int.Parse(rootJson["perpage"].ToString());

            photoset.PhotoStream.PhotoCount = TotalCount;

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

            return newPhotos;
        }

        // Photo stream retrieved for a user
        private List<FlickrPhoto> OnUserPhotoStreamReturned(string userId, string response)
        {
            // Find the user
            if (!UserCache.ContainsKey(userId))
                return new List<FlickrPhoto>();

            var user = UserCache[userId];

            JObject rawJson = JObject.Parse(response);
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
            evt.UserId = userId;
            evt.UpdatedStream = user.PhotoStream;
            PhotoStreamUpdated.DispatchEvent(this, evt);

            return newPhotos;
        }

        private List<FlickrPhoto> OnGroupPhotoStreamReturned(string groupId, string response)
        {
            if (!GroupCache.ContainsKey(groupId))
            {
                return new List<FlickrPhoto>();
            }
                
            FlickrGroup group = GroupCache[groupId];

            JObject rawJson = JObject.Parse(response);
            JObject rootJson = (JObject)rawJson["photos"];
            int TotalCount = int.Parse(rootJson["total"].ToString());
            int page = int.Parse(rootJson["page"].ToString());
            int numPages = int.Parse(rootJson["pages"].ToString());
            int perPage = int.Parse(rootJson["perpage"].ToString());

            group.PhotoStream.PhotoCount = TotalCount;

            List<FlickrPhoto> newPhotos = new List<FlickrPhoto>();
            foreach (var entry in rootJson["photo"])
            {
                JObject json = (JObject)entry;
                FlickrPhoto photo = FlickrPhotoFactory.PhotoWithJObject(json);

                if (!group.PhotoStream.Photos.Contains(photo))
                {
                    group.PhotoStream.Photos.Add(photo);
                    newPhotos.Add(photo);
                }
            }

            // Dispatch event
            var evt = new StorageEventArgs();
            evt.GroupId = group.ResourceId;
            evt.Page = page;
            evt.PageCount = numPages;
            evt.PerPage = perPage;
            evt.NewPhotos = newPhotos;
            evt.UpdatedStream = group.PhotoStream;
            PhotoStreamUpdated.DispatchEvent(this, evt);

            return newPhotos;
        }

        private List<FlickrPhoto> OnSearchStreamResult(FlickrPhotoStream stream, string response)
        {
            JObject json = JObject.Parse(response);
            JObject photosetJson = (JObject)json["photos"];

            int page = int.Parse(photosetJson["page"].ToString());
            int perPage = int.Parse(photosetJson["perpage"].ToString());
            int numTotal = int.Parse(photosetJson["total"].ToString());

            stream.PhotoCount = numTotal;

            List<FlickrPhoto> newPhotos = new List<FlickrPhoto>();
            foreach (var photoJson in photosetJson["photo"])
            {
                FlickrPhoto photo = FlickrPhotoFactory.PhotoWithJObject((JObject)photoJson);
                if (!stream.Photos.Contains(photo))
                {
                    stream.Photos.Add(photo);
                    newPhotos.Add(photo);
                }
            }

            var evt = new StorageEventArgs();
            evt.Page = page;
            evt.PerPage = perPage;
            evt.TotalCount = numTotal;
            evt.NewPhotos = newPhotos;
            PhotoSearchCompleted.DispatchEvent(this, evt);

            return newPhotos;
        }
    }
}
