using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Networking.Events
{
    public class APIEventArgs : EventArgs
    {
        public string Response { get; set; }
        public string ErrorMessage { get; set; }

        public string SessionId { get; set; }
        public string PhotoId { get; set; }
        public string AlbumId { get; set; }
        public string GroupId { get; set; }
        public string TopicId { get; set; }
        public string UserId { get; set; }

        public bool IsUploadedPhoto { get; set; }

        public string Message { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
