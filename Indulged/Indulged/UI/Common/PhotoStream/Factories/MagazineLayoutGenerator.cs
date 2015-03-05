using Indulged.UI.Models;
using System;
using System.Collections.Generic;
using Indulged.API.Utils;

namespace Indulged.UI.Common.PhotoStream.Factories
{
    public class MagazineLayoutGenerator : PhotoTileLayoutGeneratorBase
    {
        private int defaultRowSpanForSingleImage = 6;
        private int defaultRowSpanForTwoImages = 3;
        private int maxRowSpanForTwoImages = 5;
        private int defaultRowSpanForThreeImages = 4;
        private float maxRatio = 0.65f;
        private float minRatio = 0.33f;

        private Random randomGenerator = new Random();

        public override void GenerateLayout(PhotoTile tile)
        {
            if (tile.Photos.Count == 1)
            {
                LayoutOnePhoto(tile);
            }
            else if (tile.Photos.Count == 2)
            {
                LayoutTwoPhotos(tile);
            }
            else if (tile.Photos.Count == 3)
            {
                LayoutThreePhotos(tile);
            }
        }

        private void LayoutOnePhoto(PhotoTile tile)
        {
            // If there's only one tile, then the tile will use all the width and as much as heights needed
            PhotoTileLayoutMetadata layout = new PhotoTileLayoutMetadata();
            var photo = tile.Photos[0];
            layout.ColSpan = MAX_COL_COUNT;
            if (photo.Height != 0)
            {
                layout.RowSpan = Math.Min(MAX_ROW_COUNT, (int)(photo.Height / cellSize));
            }
            else
            {
                layout.RowSpan = defaultRowSpanForSingleImage;
            }

            tile.LayoutConfigurations = new List<PhotoTileLayoutMetadata> { layout };
        }

        private void LayoutTwoPhotos(PhotoTile tile)
        {
            // For two tiles, use the minimal number of rows, and then distribute the cols
            var photo1 = tile.Photos[0];
            var photo2 = tile.Photos[1];
            var layout1 = new PhotoTileLayoutMetadata();
            var layout2 = new PhotoTileLayoutMetadata();

            // Row span
            int tile1RowSpan;
            int tile2RowSpan;
            if (photo1.Height != 0)
            {
                tile1RowSpan = Math.Min(MAX_ROW_COUNT, (int)(photo1.Height / cellSize));
            }
            else
            {
                tile1RowSpan = defaultRowSpanForTwoImages;
            }

            if (photo2.Height != 0)
            {
                tile2RowSpan = Math.Min(MAX_ROW_COUNT, (int)(photo2.Height / cellSize));
            }
            else
            {
                tile2RowSpan = defaultRowSpanForTwoImages;
            }

            var rowSpan = Math.Min(tile1RowSpan, tile2RowSpan);
            rowSpan = Math.Min(maxRowSpanForTwoImages, rowSpan);
            layout1.RowSpan = rowSpan;
            layout2.RowSpan = rowSpan;
            
            // Col span
            if (photo1.Width != 0 && photo2.Width != 0)
            {
                float ratio = (float)photo1.Width / (float)(photo1.Width + photo2.Width);

                // Jitter and constraint
                if (ratio == 0.5)
                {
                    ratio = randomGenerator.NextFloat(0.35f, 0.65f);
                }

                ratio = Math.Min(ratio, maxRatio);
                ratio = Math.Max(ratio, minRatio);

                layout1.ColSpan = (int)Math.Ceiling(ratio * MAX_COL_COUNT);
                layout2.ColSpan = MAX_COL_COUNT - layout1.ColSpan;
                layout2.Col = layout1.ColSpan;
            }

            tile.LayoutConfigurations = new List<PhotoTileLayoutMetadata> { layout1, layout2 };
        }

        private void LayoutThreePhotos(PhotoTile tile)
        {
            // For three photos, one will be on the left, and two on right
            var photo1 = tile.Photos[0];
            var photo2 = tile.Photos[1];
            var photo3 = tile.Photos[2];

            var layout1 = new PhotoTileLayoutMetadata();
            var layout2 = new PhotoTileLayoutMetadata();
            var layout3 = new PhotoTileLayoutMetadata();

            // Col span
            if (photo1.Width != 0 && photo2.Width != 0 && photo3.Width != 0)
            {
                float rightSizeWidth = (float)Math.Max(photo2.Width, photo3.Width);
                float ratio = (float)photo1.Width / (float)(photo1.Width + rightSizeWidth);

                // Restrain and jitter the ratio
                ratio = Math.Min(ratio, maxRatio);
                ratio = Math.Max(ratio, minRatio);
                if (ratio == 0.5)
                {
                    ratio = randomGenerator.NextFloat(0.35f, 0.65f);
                }

                layout1.ColSpan = (int)Math.Ceiling(ratio * MAX_COL_COUNT);
                layout2.ColSpan = MAX_COL_COUNT - layout1.ColSpan + 1;
                layout3.ColSpan = layout2.ColSpan;

                layout2.Col = layout1.ColSpan;
                layout3.Col = layout2.Col;
            }

            // Row span
            var maxHeight = 0;
            maxHeight = Math.Max(maxHeight, photo1.Height);
            maxHeight = Math.Max(maxHeight, photo2.Height);
            maxHeight = Math.Max(maxHeight, photo3.Height);
            if (maxHeight == 0)
            {
                layout1.RowSpan = defaultRowSpanForThreeImages;
            }
            else
            {
                layout1.RowSpan = (int)Math.Ceiling(maxHeight / cellSize);
                layout1.RowSpan = Math.Min(layout1.RowSpan, MAX_ROW_COUNT);
            }

            if (photo2.Height != 0 && photo3.Height != 0)
            {
                float ratio = (float)photo2.Height / (float)(photo2.Width + photo3.Height);
                ratio = Math.Min(ratio, maxRatio);
                ratio = Math.Max(ratio, minRatio);
                layout2.RowSpan = (int)Math.Ceiling(layout1.RowSpan * ratio);
            }
            else
            {
                layout2.RowSpan = (int)Math.Ceiling(layout1.RowSpan * 0.5f);
            }

            layout3.RowSpan = layout1.RowSpan - layout2.RowSpan;
            layout3.Row = layout2.RowSpan;

            tile.LayoutConfigurations = new List<PhotoTileLayoutMetadata> { layout1, layout2, layout3 };
        }

        
    }
}
