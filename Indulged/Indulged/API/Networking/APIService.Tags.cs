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
        public async Task<APIResponse> GetPopularTagListAsync(Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            paramDict["method"] = "flickr.tags.getHotList";

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
                PopularTagListReturned.DispatchEvent(this, evt);
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
                PopularTagListFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }
    }
}
