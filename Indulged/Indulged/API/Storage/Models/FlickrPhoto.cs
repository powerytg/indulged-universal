using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage.Models
{
    public class FlickrPhoto : FlickrModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FlickrPhoto()
            : base()
        {
            OriginalWidth = 0;
            OriginalHeight = 0;

            LargeWidth = 0;
            LargeHeight = 0;

            MediumWidth = 0;
            MediumHeight = 0;
        }

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

        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }

        public int MediumWidth { get; set; }
        public int MediumHeight { get; set; }

        public int LargeWidth { get; set; }
        public int LargeHeight { get; set; }

        public int Width 
        {
            get
            {
                if (OriginalWidth != 0)
                {
                    return OriginalWidth;
                }
                else if (LargeWidth != 0)
                {
                    return LargeWidth;
                }
                else
                {
                    return MediumWidth;
                }
            }
        }

        public int Height
        {
            get
            {
                if (OriginalHeight != 0)
                {
                    return OriginalHeight;
                }
                else if (LargeHeight != 0)
                {
                    return LargeHeight;
                }
                else
                {
                    return MediumHeight;
                }
            }
        }

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

        public string MediumImageUrl
        {
            get
            {
                return "https://farm" + Farm + ".staticflickr.com/" + Server + "/" + ResourceId + "_" + Secret + "_z.jpg";
            }
        }
    }
}
