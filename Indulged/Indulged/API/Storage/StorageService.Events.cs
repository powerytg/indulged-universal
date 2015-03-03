using Indulged.API.Networking;
using Indulged.API.Storage.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Storage
{
    public partial class StorageService
    {
        // Stream events
        public EventHandler<StorageEventArgs> PhotoStreamUpdated;
        public EventHandler<StorageEventArgs> EXIFUpdated;

        // Search events
        public EventHandler<StorageEventArgs> PhotoSearchCompleted;
        public EventHandler<StorageEventArgs> PopularTagsUpdated;
        public EventHandler<StorageEventArgs> GroupSearchCompleted;

        // Upload events
        public EventHandler<StorageEventArgs> UploadedPhotoInfoReturned;

        // Photo events
        public EventHandler<StorageEventArgs> PhotoInfoUpdated;
        public EventHandler<StorageEventArgs> PhotoCommentsUpdated;        

        // Comment events
        public EventHandler<StorageEventArgs> CommentAdded;

        // Group events
        public EventHandler<StorageEventArgs> GroupListUpdated;
        public EventHandler<StorageEventArgs> GroupInfoUpdated;
        public EventHandler<StorageEventArgs> GroupJoined;
        public EventHandler<StorageEventArgs> GroupPhotoListUpdated;
        public EventHandler<StorageEventArgs> GroupTopicsUpdated;
        public EventHandler<StorageEventArgs> GroupTopicAdded;
        public EventHandler<StorageEventArgs> GroupTopicReplyAdded;
        public EventHandler<StorageEventArgs> GroupPhotoAdded;
        public EventHandler<StorageEventArgs> GroupPhotoRemoved;
        public EventHandler<StorageEventArgs> TopicRepliesUpdated;

        // Photo set events
        public EventHandler<StorageEventArgs> AlbumListUpdated;
        public EventHandler<StorageEventArgs> AlbumPhotoAdded;
        public EventHandler<StorageEventArgs> AlbumPhotoRemoved;
        public EventHandler<StorageEventArgs> AlbumUpdated;
        public EventHandler<StorageEventArgs> AlbumPrimaryChanged;
        public EventHandler<StorageEventArgs> AlbumDeleted;

        // Favourite events
        public EventHandler<StorageEventArgs> FavouriteStreamUpdated;
        public EventHandler<StorageEventArgs> PhotoAddedAsFavourite;
        public EventHandler<StorageEventArgs> PhotoRemovedFromFavourite;

        // User info events
        public EventHandler CurrentUserReturned;
        public EventHandler<StorageEventArgs> UserInfoUpdated;
        public EventHandler<StorageEventArgs> ContactListUpdated;
        public EventHandler ContactPhotosUpdated;

        // Activity stream events
        public EventHandler ActivityStreamUpdated;

        public void InitializeEventListeners()
        {
            // Albums
            APIService.Instance.AlbumListReturned += OnAlbumListReturned;

            // Users
            APIService.Instance.GroupListReturned += OnGroupListReturned;
        }
    }
}
