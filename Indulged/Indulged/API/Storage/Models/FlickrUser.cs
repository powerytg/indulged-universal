using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrUser : FlickrModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FlickrUser(string userId)
            : base()
        {
            ResourceId = userId;
            PhotoStream = new FlickrPhotoStream(FlickrPhotoStreamType.UserStream);
            PhotoStream.UserId = userId;
        }

        public string Name { get; set; }
        public string RealName { get; set; }
        public string Description { get; set; }

        public string Location { get; set; }
        public bool IsProUser { get; set; }
        public bool hasFirstDate { get; set; }
        public DateTime FirstDate { get; set; }

        public string Server { get; set; }
        public string Farm { get; set; }

        public string UserName { get; set; }

        public string AvatarUrl
        {
            get
            {
                if (Farm == null || Server == null || ResourceId == null)
                    return "https://www.flickr.com/images/buddyicon.gif";

                if (Farm != null && int.Parse(Farm) == 0)
                    return "https://www.flickr.com/images/buddyicon.gif";

                return "https://farm" + Farm + ".staticflickr.com/" + Server + "/buddyicons/" + ResourceId + ".jpg";
            }
        }

        // Photo stream
        public FlickrPhotoStream PhotoStream { get; set; }

        // Group ids
        public List<string> GroupIds { get; set; }

        // Full user info
        public bool IsFullInfoLoaded { get; set; }

        // Profile url
        public string ProfileUrl { get; set; }

        // Whether can create more albums
        public bool CanCreateAlbum { get; set; }
    }
}
