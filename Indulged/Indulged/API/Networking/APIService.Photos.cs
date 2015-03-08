using Indulged.API.Networking.Events;
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
        public async Task<APIResponse> GetEXIFAsync(string photoId, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            paramDict["method"] = "flickr.photos.getExif";
            paramDict["photo_id"] = photoId;

            var retVal = await DispatchRequestAsync("GET", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.PhotoId = photoId;
                evt.Response = retVal.Result;
                PhotoEXIFReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.PhotoId = photoId;
                evt.ErrorMessage = retVal.ErrorMessage;
                PhotoEXIFFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> AddToFavouriteAsync(string photoId, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            paramDict["method"] = "flickr.favorites.add";
            paramDict["photo_id"] = photoId;

            var retVal = await DispatchRequestAsync("POST", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.PhotoId = photoId;
                evt.Response = retVal.Result;
                AddToFavouriteReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.PhotoId = photoId;
                evt.ErrorMessage = retVal.ErrorMessage;
                AddToFavouriteFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }

        public async Task<APIResponse> RemoveFromFavouriteAsync(string photoId, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            paramDict["method"] = "flickr.favorites.remove";
            paramDict["photo_id"] = photoId;

            var retVal = await DispatchRequestAsync("POST", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.PhotoId = photoId;
                evt.Response = retVal.Result;
                RemoveFromFavouriteReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.PhotoId = photoId;
                evt.ErrorMessage = retVal.ErrorMessage;
                RemoveFromFavouriteFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }
    }
}
