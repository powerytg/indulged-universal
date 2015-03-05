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
            List<PhotoTile> result = new List<PhotoTile>();

            // Randomly slice the photo into groups
            int min = 1;
            int max = 4;
            int position = 0;
            PhotoTile tile;

            while (position < photos.Count)
            {
                int ranNum = randomGenerator.Next(min, max);
                List<FlickrPhoto> group = new List<FlickrPhoto>();

                if (position + ranNum >= photos.Count)
                {
                    for (int i = position; i < photos.Count; i++)
                    {
                        group.Add(photos[i]);
                    }

                    tile = new PhotoTile(group, PhotoTile.LayoutStyle.Magazine);
                    layoutGenerator.GenerateLayout(tile);
                    result.Add(tile);                    
                    break;
                }

                for (int i = position; i < position + ranNum; i++)
                {
                    group.Add(photos[i]);
                }

                tile = new PhotoTile(group, PhotoTile.LayoutStyle.Magazine);
                layoutGenerator.GenerateLayout(tile);
                result.Add(tile);
                
                position += ranNum;
            }

            return result;
        }
        
        public List<PhotoTile> GenerateLinearPhotoTiles(List<FlickrPhoto> photos)
        {
            List<PhotoTile> result = new List<PhotoTile>();
            foreach (var photo in photos)
            {
                var tile = new PhotoTile(new List<FlickrPhoto> { photo }, PhotoTile.LayoutStyle.Magazine);
                layoutGenerator.GenerateLayout(tile);
                result.Add(tile);
            }

            return result;
        }

        public List<PhotoTile> GenerateJournalPhotoTiles(List<FlickrPhoto> photos)
        {
            List<PhotoTile> result = new List<PhotoTile>();
            foreach (var photo in photos)
            {
                var tile = new PhotoTile(new List<FlickrPhoto> { photo }, PhotoTile.LayoutStyle.Journal);
                result.Add(tile);
            }

            return result;
        }
         
    }
}
