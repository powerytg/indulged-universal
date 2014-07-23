using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Events
{
    public class StorageEventArgs : EventArgs
    {
        public string SessionId { get; set; }
        public string PhotoId { get; set; }
        public string AlbumId { get; set; }
        public string GroupId { get; set; }
        public string TopicId { get; set; }
        public string UserId { get; set; }

        public int Page { get; set; }
        public int PageCount { get; set; }
        public int PerPage { get; set; }
        public int TotalCount { get; set; }

        public List<FlickrTopic> NewTopics { get; set; }
        public List<FlickrReply> NewReplies { get; set; }
        public List<FlickrPhoto> NewPhotos { get; set; }
        public List<FlickrComment> NewComments { get; set; }
        public List<FlickrGroup> NewGroups { get; set; }
        public List<FlickrTag> NewTags { get; set; }
        public List<FlickrUser> NewUsers { get; set; }

        public FlickrPhotoStream UpdatedStream { get; set; }
    }
}
