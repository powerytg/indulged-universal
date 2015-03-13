using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indulged.API.Utils;
using Indulged.API.Networking.Events;

namespace Indulged.API.Networking
{
    public partial class APIService
    {        
        public async Task<APIResponse> GetGroupInfoAsync(string groupId, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            paramDict["method"] = "flickr.groups.getInfo";
            paramDict["group_id"] = UrlUtils.Encode(groupId);

            var retVal = await DispatchRequestAsync("GET", paramDict, true);
            if (retVal.Success)
            {
                if (success != null)
                {
                    success(retVal.Result);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.Response = retVal.Result;
                GroupInfoReturned.DispatchEvent(this, evt);
            }
            else
            {
                if (failure != null)
                {
                    failure(retVal.ErrorMessage);
                }

                // Dispatch event
                var evt = new APIEventArgs();
                evt.GroupId = groupId;
                evt.ErrorMessage = retVal.ErrorMessage;
                GroupInfoFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }
    }
}
