using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage
{
    public partial class StorageService
    {
        // Singleton
        private static StorageService instance;

        public static StorageService Instance
        {
            get
            {
                if (instance == null)
                    instance = new StorageService();

                return instance;
            }
        }

        // User cache
        public Dictionary<string, FlickrUser> UserCache { get; set; }

        // Current user object
        public FlickrUser CurrentUser { get; set; }

        // Photoset cache
        public int AlbumCount { get; set; }
        public Dictionary<string, FlickrAlbum> AlbumCache { get; set; }
        public List<FlickrAlbum> AlbumList { get; set; }

        // Photo cache
        public Dictionary<string, FlickrPhoto> PhotoCache { get; set; }

        // Special streams
        public FlickrPhotoStream DiscoveryStream { get; set; }
        public FlickrPhotoStream FavouriteStream { get; set; }
        public FlickrPhotoStream ContactStream { get; set; }

        // Group cache
        public Dictionary<string, FlickrGroup> GroupCache { get; set; }

        // Contact list
        public int ContactCount { get; set; }
        public List<FlickrUser> ContactList { get; set; }

        // Activity stream
        public int ActivityItemsCount { get; set; }
        public Dictionary<string, FlickrPhotoActivity> ActivityCache { get; set; }
        public List<FlickrPhotoActivity> ActivityList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StorageService()
        {
            // Initialize special streams
            DiscoveryStream = new FlickrPhotoStream(FlickrPhotoStreamType.DiscoveryStream);
            FavouriteStream = new FlickrPhotoStream(FlickrPhotoStreamType.FavouriteStream);
            ContactStream = new FlickrPhotoStream(FlickrPhotoStreamType.ContactStream);

            // User cache
            UserCache = new Dictionary<string, FlickrUser>();

            // Photoset cache
            AlbumCache = new Dictionary<string, FlickrAlbum>();
            AlbumList = new List<FlickrAlbum>();

            // Photo cache
            PhotoCache = new Dictionary<string, FlickrPhoto>();

            // Group cache
            GroupCache = new Dictionary<string, FlickrGroup>();

            // Contact list
            ContactList = new List<FlickrUser>();

            // Activity stream
            ActivityCache = new Dictionary<string, FlickrPhotoActivity>();
            ActivityList = new List<FlickrPhotoActivity>();

           
        }
    }
}
