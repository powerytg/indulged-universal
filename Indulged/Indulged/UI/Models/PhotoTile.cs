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

        public FlickrPhoto Photo { get; set; }
        public LayoutStyle Style;
        public int RowSpan { get; set; }
        public int ColSpan { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoTile()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="photo"></param>
        public PhotoTile(FlickrPhoto photo, LayoutStyle layoutStyle)
        {
            Photo = photo;
            Style = layoutStyle;
        }
    }
}
