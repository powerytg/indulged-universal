using Indulged.API.Utils;
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
        public async Task<APIResponse> GetAlbumListAsync(Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            paramDict["primary_photo_extras"] = CommonPhotoExtraParameters;
            paramDict["method"] = "flickr.photosets.getList";

            var retVal = await DispatchRequestAsync("GET", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.Response = retVal.Result;
                AlbumListReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.ErrorMessage = retVal.ErrorMessage;
                AlbumListFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }
    }
}
