using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrPhotoActivity : FlickrModelBase
    {
        public string Title { get; set; }
        public FlickrPhoto TargetPhoto { get; set; }
        public List<FlickrPhotoActivityEventBase> Events { get; set; }
        public List<FlickrUser> FavUsers { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public FlickrPhotoActivity()
            : base()
        {
            Events = new List<FlickrPhotoActivityEventBase>();
            FavUsers = new List<FlickrUser>();
        }
    }
}
