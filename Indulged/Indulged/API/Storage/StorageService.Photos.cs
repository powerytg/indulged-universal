using Indulged.API.Utils;
using Indulged.API.Networking.Events;
using Indulged.API.Storage.Events;
using Indulged.API.Storage.Factories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indulged.API.Storage.Models;

namespace Indulged.API.Storage
{
    public partial class StorageService
    {
        private void OnEXIFReturned(object sender, APIEventArgs e)
        {
            if (!PhotoCache.ContainsKey(e.PhotoId))
                return;

            var photo = PhotoCache[e.PhotoId];
            JObject json = JObject.Parse(e.Response);
            photo.EXIF = FlickrPhotoEXIFFactory.EXIFWithJObject((JObject)json["photo"]);

            var evt = new StorageEventArgs();
            evt.PhotoId = photo.ResourceId;
            EXIFUpdated.DispatchEvent(this, evt);
        }

        private void OnPhotoInfoReturned(object sender, APIEventArgs e)
        {
            JObject json = JObject.Parse(e.Response);
            FlickrPhoto photo = FlickrPhotoFactory.PhotoWithPhotoInfoJObject((JObject)json["photo"]);

            // Should add this photo to stream?
            if (e.IsUploadedPhoto)
            {
                // Dispatch event
                var evt = new StorageEventArgs();
                evt.PhotoId = e.PhotoId;
                UploadedPhotoInfoReturned.DispatchEvent(this, evt);
            }

            var updateEvt = new StorageEventArgs();
            updateEvt.PhotoId = photo.ResourceId;
            PhotoInfoUpdated.DispatchEvent(this, updateEvt);
        }

        private void OnPhotoCommentsReturned(object sender, APIEventArgs e)
        {
            if (!PhotoCache.ContainsKey(e.PhotoId))
                return;

            FlickrPhoto photo = PhotoCache[e.PhotoId];

            // Hack prevent evulation func timeout
            if (e.Response.Contains("_content"))
            {
                JObject rawJson = JObject.Parse(e.Response);
                JObject rootJson = (JObject)rawJson["comments"];

                photo.Comments.Clear();

                foreach (var entry in rootJson["comment"])
                {
                    JObject commentJObject = (JObject)entry;
                    FlickrComment comment = FlickrCommentFactory.PhotoCommentWithJObject(commentJObject, photo);
                    photo.Comments.Add(comment);
                }

            }


            var evt = new StorageEventArgs();
            evt.PhotoId = photo.ResourceId;
            PhotoCommentsUpdated.DispatchEvent(this, evt);
        }

        private void OnPhotoCommentAdded(object sender, APIEventArgs e)
        {
            FlickrPhoto photo = PhotoCache[e.PhotoId];

            JObject rawJson = JObject.Parse(e.Response);
            string newCommentId = rawJson["comment"]["id"].ToString();

            var newComment = new FlickrComment();
            newComment.ResourceId = newCommentId;
            newComment.Message = e.Message;
            newComment.Author = CurrentUser;
            newComment.CreationDate = DateTime.Now;

            photo.CommentCache[newCommentId] = newComment;
            photo.Comments.Insert(0, newComment);
            photo.CommentCount++;

            var evt = new StorageEventArgs();
            evt.SessionId = e.SessionId;
            evt.PhotoId = photo.ResourceId;
            evt.NewComments = new List<FlickrComment> { newComment };
            CommentAdded.DispatchEvent(this, evt);
        }

        private void OnAddPhotoAsFavourite(object sender, APIEventArgs e)
        {
            FlickrPhoto photo = PhotoCache[e.PhotoId];
            photo.IsFavourite = true;

            if (!FavouriteStream.Photos.Contains(photo))
            {
                FavouriteStream.Photos.Insert(0, photo);
            }

            FavouriteStream.PhotoCount++;

            var evt = new StorageEventArgs();
            evt.PhotoId = photo.ResourceId;
            PhotoAddedAsFavourite.DispatchEvent(this, evt);
        }

        private void OnRemovePhotoFromFavourite(object sender, APIEventArgs e)
        {
            FlickrPhoto photo = PhotoCache[e.PhotoId];
            photo.IsFavourite = false;

            if (FavouriteStream.Photos.Contains(photo))
            {
                FavouriteStream.Photos.Remove(photo);
            }

            FavouriteStream.PhotoCount--;

            var evt = new StorageEventArgs();
            evt.PhotoId = photo.ResourceId;
            PhotoRemovedFromFavourite.DispatchEvent(this, evt);
        }
    }
}
