using Indulged.API.Utils;
using Indulged.API.Storage.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Factories
{
    public class FlickrCommentFactory
    {
        public static FlickrComment PhotoCommentWithJObject(JObject json, FlickrPhoto photo)
        {
            // Get comment id
            string commentId = json["id"].ToString();
            FlickrComment comment = null;
            if (photo.CommentCache.ContainsKey(commentId))
            {
                comment = photo.CommentCache[commentId];
            }
            else
            {
                comment = new FlickrComment();
                comment.ResourceId = commentId;
                photo.CommentCache[commentId] = comment;
            }


            // Parse user
            comment.Author = FlickrUserFactory.UserWithPhotoCommentJObject(json);
            comment.CreationDate = json["datecreate"].ToString().ToDateTime();

            // Content
            comment.Message = json["_content"].ToString();

            return comment;
        }
    }
}
