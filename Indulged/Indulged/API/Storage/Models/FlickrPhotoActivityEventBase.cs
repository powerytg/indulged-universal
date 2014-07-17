using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrPhotoActivityEventBase : FlickrModelBase
    {
        public string EventType { get; set; }
        public FlickrUser EventUser { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
