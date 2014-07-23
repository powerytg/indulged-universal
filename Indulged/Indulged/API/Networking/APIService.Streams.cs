using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using Indulged.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Networking
{
    public partial class APIService
    {
        public async Task<APIResponse> GetPhotoStreamAsync(FlickrPhotoStream stream, Dictionary<string, string> parameters = null, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    paramDict[entry.Key] = entry.Value;
                }
            }

            // Figure out the additional parameters
            if (stream.StreamType == FlickrPhotoStreamType.AlbumStream)
            {
                paramDict["photoset_id"] = stream.AlbumId;
                paramDict["method"] = "flickr.photosets.getPhotos";
            }
            else if (stream.StreamType == FlickrPhotoStreamType.ContactStream)
            {
                paramDict["method"] = "flickr.photos.getContactsPhotos";
            }
            else if (stream.StreamType == FlickrPhotoStreamType.UserStream)
            {
                if (stream.UserId == StorageService.Instance.CurrentUser.ResourceId)
                {
                    paramDict["user_id"] = "me";
                }
                else
                {
                    paramDict["user_id"] = stream.UserId;
                }

                paramDict["method"] = "flickr.people.getPhotos";
            }
            else if (stream.StreamType == FlickrPhotoStreamType.GroupStream)
            {
                paramDict["group_id"] = stream.GroupId;
                paramDict["method"] = "flickr.groups.pools.getPhotos";
            }
            else if (stream.StreamType == FlickrPhotoStreamType.DiscoveryStream)
            {
                paramDict["method"] = "flickr.interestingness.getList";
            }
            else if (stream.StreamType == FlickrPhotoStreamType.FavouriteStream)
            {
                paramDict["method"] = "flickr.favorites.getList";
                paramDict["user_id"] = StorageService.Instance.CurrentUser.ResourceId;
            }

            var retVal = await DispatchRequestAsync("GET", paramDict, true); 
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }
            }

            return retVal;
        }
    }
}
