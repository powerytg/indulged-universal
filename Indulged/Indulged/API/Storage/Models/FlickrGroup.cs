using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public enum FlickrGroupPrivicy { Private = 1, InviteOnlyPublic = 2, Public = 3 };

    public class FlickrGroup : FlickrModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Server { get; set; }
        public string Farm { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsInvitationOnly { get; set; }
        public bool IsEighteenPlus { get; set; }
        public int MemberCount { get; set; }
        public int TopicCount { get; set; }
        public FlickrGroupPrivicy Privacy { get; set; }
        public string Rules { get; set; }

        public string ThrottleMode { get; set; }
        public int ThrottleMaxCount { get; set; }
        public int ThrottleRemainingCount { get; set; }

        public bool IsInfoRetrieved { get; set; }

        public FlickrGroup(string groupId)
            : base()
        {
            ResourceId = groupId;
            PhotoStream = new FlickrPhotoStream(FlickrPhotoStreamType.GroupStream);
            PhotoStream.GroupId = groupId;
        }

        public override string ToString()
        {
            return Name;
        }

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

        // Topics
        public Dictionary<string, FlickrTopic> TopicCache = new Dictionary<string, FlickrTopic>();
        private List<FlickrTopic> _topics = new List<FlickrTopic>();
        public List<FlickrTopic> Topics
        {
            get
            {
                return _topics;
            }

            set
            {
                _topics = value;

            }
        }

    }
}
