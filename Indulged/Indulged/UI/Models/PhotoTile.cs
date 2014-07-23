using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.UI.Models
{
    public class PhotoTile
    {
        public List<FlickrPhoto> Photos { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoTile()
        {
            Photos = new List<FlickrPhoto>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="photos"></param>
        public PhotoTile(List<FlickrPhoto> photos)
        {
            Photos = new List<FlickrPhoto>();
            Photos.AddRange(photos);
        }
    }
}
