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
        public async Task<APIResponse> SearchGroupsAsync(string query, Dictionary<string, string> parameters = null, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = new Dictionary<string, string>();
            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    paramDict[entry.Key] = entry.Value;
                }
            }

            paramDict["method"] = "flickr.groups.search";
            paramDict["text"] = UrlUtils.Encode(query);

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
                GroupSearchReturned.DispatchEvent(this, evt);
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
                GroupSearchFailedReturn.DispatchEvent(this, evt);
            }

            return retVal;
        }
    }
}
