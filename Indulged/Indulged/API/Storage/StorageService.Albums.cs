using Indulged.API.Utils;
using Indulged.API.Networking.Events;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indulged.API.Storage.Models;
using Indulged.API.Storage.Factories;
using Indulged.API.Storage.Events;

namespace Indulged.API.Storage
{
    public partial class StorageService
    {
        public void OnAlbumListReturned(object sender, APIEventArgs e)
        {
            JObject json = JObject.Parse(e.Response);
            JObject photosetJson = (JObject)json["photosets"];

            bool canCreate = photosetJson["cancreate"].ToString().ParseBool();
            int page = int.Parse(photosetJson["page"].ToString());
            int perPage = int.Parse(photosetJson["perpage"].ToString());
            int numTotal = int.Parse(photosetJson["total"].ToString());

            CurrentUser.CanCreateAlbum = canCreate;

            AlbumList.Clear();
            foreach (var ps in photosetJson["photoset"])
            {
                FlickrAlbum photoset = FlickrAlbumFactory.PhotoSetWithJObject((JObject)ps);
                AlbumList.Add(photoset);
            }

            AlbumCount = numTotal;

            // Dispatch event
            var args = new StorageEventArgs();
            args.Page = page;
            args.PerPage = perPage;
            AlbumListUpdated.DispatchEvent(this, args);
        }
        
        private void OnPhotoAddedToAlbum(object sender, APIEventArgs e)
        {
            FlickrAlbum photoSet = AlbumCache[e.AlbumId];
            FlickrPhoto photo = PhotoCache[e.PhotoId];

            if (!photoSet.PhotoStream.Photos.Contains(photo))
            {
                photoSet.PhotoStream.Photos.Insert(0, photo);
                photoSet.PhotoStream.PhotoCount++;

                // Dispatch event
                var ae = new StorageEventArgs();
                ae.PhotoId = photo.ResourceId;
                ae.AlbumId = photoSet.ResourceId;
                AlbumPhotoAdded.DispatchEvent(this, ae);
            }
        }

        private void OnPhotoRemovedFromAlbum(object sender, APIEventArgs e)
        {
            FlickrAlbum photoSet = AlbumCache[e.AlbumId];
            FlickrPhoto photo = PhotoCache[e.PhotoId];

            if (photoSet.PhotoStream.Photos.Contains(photo))
            {
                photoSet.PhotoStream.Photos.Remove(photo);
                photoSet.PhotoStream.PhotoCount--;

                // Dispatch event
                var evt = new StorageEventArgs();
                evt.PhotoId = photo.ResourceId;
                evt.AlbumId = photoSet.ResourceId;
                AlbumPhotoRemoved.DispatchEvent(this, evt);
            }
        }

        private void OnAlbumUpdated(object sender, APIEventArgs e)
        {
            FlickrAlbum photoSet = AlbumCache[e.AlbumId];
            photoSet.Title = e.Title;
            photoSet.Description = e.Description;

            var evt = new StorageEventArgs();
            evt.AlbumId = e.AlbumId;
            AlbumUpdated.DispatchEvent(this, evt);
        }

        private void OnAlbumPrimaryChanged(object sender, APIEventArgs e)
        {
            FlickrAlbum photoSet = AlbumCache[e.AlbumId];
            photoSet.Primary = e.PhotoId;
            photoSet.PrimaryPhoto = PhotoCache[photoSet.Primary];

            var evt = new StorageEventArgs();
            evt.AlbumId = e.AlbumId;
            AlbumPrimaryChanged.DispatchEvent(this, evt);
        }

        private void OnAlbumDeleted(object sender, APIEventArgs e)
        {
            FlickrAlbum photoSet = AlbumCache[e.AlbumId];
            AlbumCache.Remove(e.AlbumId);
            AlbumList.Remove(photoSet);

            photoSet = null;

            // Dispatch event
            var args = new StorageEventArgs();
            args.AlbumId = e.AlbumId;
            AlbumDeleted.DispatchEvent(this, args);
        }
    }
}
