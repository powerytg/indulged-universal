using Indulged.API.Storage.Models;
using Indulged.UI.Models;
using System;
using System.Collections.Generic;

namespace Indulged.UI.Common.PhotoStream.Factories
{
    public class PhotoTileFactory
    {
        private PhotoTileLayoutGeneratorBase layoutGenerator;

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoTileFactory()
        {
            layoutGenerator = new MagazineLayoutGenerator();
        }

        /// <summary>
        /// Random generator
        /// </summary>
        private Random randomGenerator = new Random();

        public List<PhotoTile> GenerateMagazineTiles(List<FlickrPhoto> photos)
        {
            List<PhotoTile> tiles = new List<PhotoTile>();
            foreach (var photo in photos)
            {
                var tile = new PhotoTile(photo, PhotoTile.LayoutStyle.Magazine);
                tiles.Add(tile);
            }

            // Randomly slice the photo into groups
            int min = 2;
            int max = 2;
            int position = 0;

            while (position < photos.Count)
            {
                int ranNum = randomGenerator.Next(min, max);
                List<PhotoTile> group = new List<PhotoTile>();

                if (position + ranNum >= photos.Count)
                {
                    for (int i = position; i < photos.Count; i++)
                    {
                        group.Add(tiles[i]);
                    }

                    layoutGenerator.GenerateLayoutWeightForTiles(group);
                    break;
                }

                for (int i = position; i < position + ranNum; i++)
                {
                    group.Add(tiles[i]);
                }

                layoutGenerator.GenerateLayoutWeightForTiles(group);
                position += ranNum;
            }

            return tiles;
        }
        /*
        public List<PhotoTile> GenerateLinearPhotoTiles(List<FlickrPhoto> photos)
        {
            List<PhotoTile> result = new List<PhotoTile>();
            foreach (var photo in photos)
            {
                var tile = new PhotoTile(new List<FlickrPhoto> { photo });
                result.Add(tile);
            }

            return result;
        }

        public List<PhotoTile> GenerateJournalPhotoTiles(List<FlickrPhoto> photos)
        {
            List<PhotoTile> result = new List<PhotoTile>();
            foreach (var photo in photos)
            {
                var tile = new PhotoTile(new List<FlickrPhoto> { photo });
                tile.ShowAsJournal = true;
                result.Add(tile);
            }

            return result;
        }
         */
    }
}
