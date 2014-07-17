using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrAlbum : FlickrModelBase
    {
        public string Primary { get; set; }
        public string Secret { get; set; }
        public string Server { get; set; }
        public string Farm { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
        public bool CanComment { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public FlickrPhoto PrimaryPhoto { get; set; }

        public FlickrPhotoStream PhotoStream { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FlickrAlbum(string albumId)
            : base()
        {
            ResourceId = albumId;
            PhotoStream = new FlickrPhotoStream(FlickrPhotoStreamType.AlbumStream);
            PhotoStream.AlbumId = albumId;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
