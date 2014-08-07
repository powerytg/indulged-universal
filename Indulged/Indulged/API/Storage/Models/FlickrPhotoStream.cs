using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public enum FlickrPhotoStreamType { DiscoveryStream, FavouriteStream, UserStream, ContactStream, GroupStream, AlbumStream };

    public class FlickrPhotoStream
    {
        public string Name { get; set; }

        public List<FlickrPhoto> Photos { get; set; }
        public int PhotoCount { get; set; }
        public int VideoCount { get; set; }
        public FlickrPhotoStreamType StreamType { get; set; }

        public string AlbumId { get; set; }
        public string GroupId { get; set; }
        public string UserId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FlickrPhotoStream(FlickrPhotoStreamType type)
        {
            Photos = new List<FlickrPhoto>();
            StreamType = type;
        }
    }
}
