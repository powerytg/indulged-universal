using Indulged.API.Storage.Models;
using Indulged.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.UI.Common.PhotoStream
{
    public class PhotoTileFactory
    {
        // Random generator
        private Random randomGenerator = new Random();

        public List<PhotoTile> GeneratePhotoTiles(List<FlickrPhoto> photos)
        {
            List<PhotoTile> result = new List<PhotoTile>();

            // Randomly slice the photo into groups
            int min = 1;
            int max = 3;
            int position = 0;

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

                    result.Add(new PhotoTile(group));
                    break;
                }

                for (int i = position; i < position + ranNum; i++)
                {
                    group.Add(photos[i]);
                }

                result.Add(new PhotoTile(group));
                position += ranNum;
            }

            return result;
        }

    }
}
