using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrPhotoActivityFavouriteEvent : FlickrPhotoActivityEventBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FlickrPhotoActivityFavouriteEvent()
            : base()
        {
            FavouriteUsers = new List<FlickrUser>();
        }

        public List<FlickrUser> FavouriteUsers { get; set; }
    }
}
