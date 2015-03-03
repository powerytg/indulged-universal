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
        public async Task<APIResponse> GetGroupListAsync(string userId, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            paramDict["method"] = "flickr.people.getGroups";
            paramDict["primary_photo_extras"] = UrlUtils.Encode(CommonPhotoExtraParameters);
            paramDict["user_id"] = UrlUtils.Encode(userId);
            paramDict["extras"] = UrlUtils.Encode("privacy,throttle,restrictions");

            var retVal = await DispatchRequestAsync("GET", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.UserId = userId;
                evt.Response = retVal.Result;
                GroupListReturned.DispatchEvent(this, evt);
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
                GroupListFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }
    }
}
