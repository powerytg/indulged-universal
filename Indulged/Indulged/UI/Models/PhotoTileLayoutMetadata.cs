using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.UI.Models
{
    public class PhotoTileLayoutMetadata
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int RowSpan { get; set; }
        public int ColSpan { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoTileLayoutMetadata()
        {
            Row = 0;
            Col = 0;
            RowSpan = 1;
            ColSpan = 1;
        }
    }
}
