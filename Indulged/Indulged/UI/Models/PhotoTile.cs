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
        public enum LayoutStyle
        {
            Magazine,
            Journal
        }

        public List<FlickrPhoto> Photos { get; set; }
        public LayoutStyle Style;
        public List<PhotoTileLayoutMetadata> LayoutConfigurations;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="photo"></param>
        public PhotoTile(List<FlickrPhoto> photos, LayoutStyle layoutStyle)
        {
            Photos = photos;
            Style = layoutStyle;
        }
    }
}
