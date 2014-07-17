using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrPhotoActivityCommentEvent : FlickrPhotoActivityEventBase
    {
        public string Message { get; set; }
    }
}
