using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrPhoto : FlickrModelBase
    {
        public enum PhotoSize { Medium, Large };

        public string UserId { get; set; }
        public string Secret { get; set; }
        public string Server { get; set; }
        public string Farm { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ViewCount { get; set; }
        public string LicenseId { get; set; }
        public List<string> Tags { get; set; }
        public bool IsFavourite { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public int MediumWidth { get; set; }
        public int MediumHeight { get; set; }

        // Comments
        public int CommentCount { get; set; }
        private Dictionary<string, FlickrComment> _commentCache = new Dictionary<string, FlickrComment>();
        public Dictionary<string, FlickrComment> CommentCache
        {
            get
            {
                return _commentCache;
            }

            set
            {
                _commentCache = value;
            }
        }

        private List<FlickrComment> _comments = new List<FlickrComment>();
        public List<FlickrComment> Comments
        {
            get
            {
                return _comments;
            }

            set
            {
                _comments = value;
            }
        }

        // EXIF
        public Dictionary<string, string> EXIF { get; set; }

        public string GetImageUrl(PhotoSize size = PhotoSize.Medium)
        {
            string sizeSuffixe = "z";
            if (size == PhotoSize.Medium)
                sizeSuffixe = "z";
            else if (size == PhotoSize.Large)
                sizeSuffixe = "b";

            return "https://farm" + Farm + ".staticflickr.com/" + Server + "/" + ResourceId + "_" + Secret + "_" + sizeSuffixe + ".jpg";
        }

    }
}
