using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrComment : FlickrModelBase
    {
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public FlickrUser Author { get; set; }
    }
}
