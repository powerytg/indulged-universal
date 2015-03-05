using Indulged.UI.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace Indulged.UI.Common.PhotoStream.Factories
{
    public abstract class PhotoTileLayoutGeneratorBase
    {
        public static int MAX_COL_COUNT = 10;
        public static int MAX_ROW_COUNT = 8;
        protected double cellSize;

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoTileLayoutGeneratorBase()
        {
            // Calculate the grid cell size based on screen size and max number of columns
            var screenWidth = Window.Current.Bounds.Width;
            cellSize = Math.Floor(screenWidth / MAX_COL_COUNT);
        }

        /// <summary>
        /// Create RowSpan and ColSpan for photo tiles
        /// </summary>
        /// <param name="tiles"></param>
        public abstract void GenerateLayout(PhotoTile tile);
    }
}
