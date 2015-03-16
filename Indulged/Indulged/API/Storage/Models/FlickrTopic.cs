using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrTopic : FlickrModelBase
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }

        public FlickrUser Author { get; set; }
        public bool IsAdmin { get; set; }

        public bool CanReply { get; set; }
        public DateTime LastReplyDate { get; set; }
        public int ReplyCount { get; set; }

        private Dictionary<string, FlickrReply> _replyCache = new Dictionary<string, FlickrReply>();
        public Dictionary<string, FlickrReply> ReplyCache
        {
            get
            {
                return _replyCache;
            }

            set
            {
                _replyCache = value;
            }
        }

        private List<FlickrReply> replies = new List<FlickrReply>();
        public List<FlickrReply> Replies
        {
            get
            {
                return replies;
            }

            set
            {
                replies = value;
            }
        }

        public string CleanSubject { get; set; }
        public string CleanMessage { get; set; }

    }
}
