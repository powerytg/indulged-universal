using Indulged.API.Networking.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Networking
{
    public partial class APIService
    {
        // Load album list
        public EventHandler<APIEventArgs> AlbumListReturned;
        public EventHandler<APIEventArgs> AlbumListFailedReturn;

        // Load group list
        public EventHandler<APIEventArgs> GroupListReturned;
        public EventHandler<APIEventArgs> GroupListFailedReturn;

        // Get EXIF
        public EventHandler<APIEventArgs> PhotoEXIFReturned;
        public EventHandler<APIEventArgs> PhotoEXIFFailedReturn;

        // Favourite requests
        public EventHandler<APIEventArgs> AddToFavouriteReturned;
        public EventHandler<APIEventArgs> AddToFavouriteFailedReturn;
        public EventHandler<APIEventArgs> RemoveFromFavouriteReturned;
        public EventHandler<APIEventArgs> RemoveFromFavouriteFailedReturn;

        // Photo comments
        public EventHandler<APIEventArgs> PhotoCommentsReturned;
        public EventHandler<APIEventArgs> PhotoCommentsFailedReturn;
        public EventHandler<APIEventArgs> AddCommentReturned;
        public EventHandler<APIEventArgs> AddCommentFailedReturn;

        // Get user info
        public EventHandler<APIEventArgs> UserInfoReturned;
        public EventHandler<APIEventArgs> UserInfoFailedReturn;


    }
}
