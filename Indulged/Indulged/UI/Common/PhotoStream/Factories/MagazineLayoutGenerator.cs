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

        private Random randomGenerator = new Random();

        public override void GenerateLayoutWeightForTiles(List<PhotoTile> tiles)
        {
            if (tiles.Count == 1)
            {
                LayoutOneMagazineTile(tiles);
            }
            else if(tiles.Count == 2)
            {
                LayoutTwoMagazineTiles(tiles);
            }
            else if(tiles.Count == 3)
            {
                LayoutThreeMagazineTiles(tiles);
            }
        }

        private void LayoutOneMagazineTile(List<PhotoTile> tiles)
        {
            // If there's only one tile, then the tile will use all the width and as much as heights needed
            var tile = tiles[0];
            tile.ColSpan = MAX_COL_COUNT;
            if (tile.Photo.Height != 0)
            {
                tile.RowSpan = Math.Min(MAX_ROW_COUNT, (int)(tile.Photo.Height / cellSize));
            }
            else
            {
                tile.RowSpan = defaultRowSpanForSingleImage;
            }
        }

        private void LayoutTwoMagazineTiles(List<PhotoTile> tiles)
        {
            // For two tiles, use the minimal number of rows, and then distribute the cols
            var tile1 = tiles[0];
            var tile2 = tiles[1];

            // Row span
            int tile1RowSpan;
            int tile2RowSpan;
            if (tile1.Photo.Height != 0)
            {
                tile1RowSpan = Math.Min(MAX_ROW_COUNT, (int)(tile1.Photo.Height / cellSize));
            }
            else
            {
                tile1RowSpan = defaultRowSpanForTwoImages;
            }

            if (tile2.Photo.Height != 0)
            {
                tile2RowSpan = Math.Min(MAX_ROW_COUNT, (int)(tile2.Photo.Height / cellSize));
            }
            else
            {
                tile2RowSpan = defaultRowSpanForTwoImages;
            }

            var rowSpan = Math.Min(tile1RowSpan, tile2RowSpan);
            rowSpan = Math.Min(maxRowSpanForTwoImages, rowSpan);
            tile1.RowSpan = rowSpan;
            tile2.RowSpan = rowSpan;
            
            // Col span
            if (tile1.Photo.Width != 0 && tile2.Photo.Width != 0)
            {
                float ratio = (float)tile1.Photo.Width / (float)(tile1.Photo.Width + tile2.Photo.Width);

                // Jitter and constraint
                if (ratio == 0.5)
                {
                    ratio = randomGenerator.NextFloat(0.35f, 0.65f);
                }

                ratio = Math.Min(ratio, maxRatio);

                tile1.ColSpan = (int)Math.Ceiling(ratio * MAX_COL_COUNT);
                tile2.ColSpan = MAX_COL_COUNT - tile1.ColSpan + 1;
            }
        }

        private void LayoutThreeMagazineTiles(List<PhotoTile> tiles)
        {
            // For three photos, one will be on the left, and two on right
            var tile1 = tiles[0];
            var tile2 = tiles[1];
            var tile3 = tiles[2];

            // Col span
            if (tile1.Photo.Width != 0 && tile2.Photo.Width != 0 && tile3.Photo.Width != 0)
            {
                float rightSizeWidth = (float)Math.Max(tile2.Photo.Width, tile3.Photo.Width);
                float ratio = (float)tile1.Photo.Width / (float)(tile1.Photo.Width + rightSizeWidth);
                tile1.ColSpan = (int)Math.Ceiling(ratio * MAX_COL_COUNT);
                tile2.ColSpan = MAX_COL_COUNT - tile1.ColSpan;
                tile3.ColSpan = tile2.ColSpan;
            }

            // Row span
            var maxHeight = 0;
            maxHeight = Math.Max(maxHeight, tile1.Photo.Height);
            maxHeight = Math.Max(maxHeight, tile2.Photo.Height);
            maxHeight = Math.Max(maxHeight, tile3.Photo.Height);
            if (maxHeight == 0)
            {
                tile1.RowSpan = defaultRowSpanForThreeImages;
            }
            else
            {
                tile1.RowSpan = (int)Math.Ceiling(maxHeight / cellSize);
                tile1.RowSpan = Math.Min(tile1.RowSpan, MAX_ROW_COUNT);
            }

            if (tile2.Photo.Height != 0 && tile3.Photo.Height != 0)
            {
                float ratio = (float)tile2.Photo.Height / (float)(tile2.Photo.Width + tile3.Photo.Height);
                tile2.RowSpan = (int) Math.Ceiling(tile1.RowSpan * ratio);
            }
            else
            {
                tile2.RowSpan = (int)Math.Ceiling(tile1.RowSpan * 0.5f);
            }

            tile3.RowSpan = tile1.RowSpan - tile2.RowSpan;
        }

        
    }
}
