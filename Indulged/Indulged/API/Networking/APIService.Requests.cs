using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Indulged.API.Networking
{
    public partial class APIService
    {
        public async Task<APIResponse> DispatchRequestAsync(string method, Dictionary<string, string> parameters = null, bool shouldEnsureStatOK = true, Action<string> success = null, Action<string> failure = null)
        {
            var paramDict = GetBaseRequestParameters();

            if (parameters != null)
            {
                foreach (var entry in parameters)
                {
                    paramDict[entry.Key] = entry.Value;
                }
            }

            // Create the http request
            var retVal = new APIResponse();

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = null;
                if (method.ToUpper() == "GET")
                {
                    string paramString = GenerateParamString(paramDict);
                    string signature = GenerateSignature(method, AccessTokenSecret, APIUrl, paramString);
                    var requestUrl = APIUrl + "?" + paramString + "&oauth_signature=" + signature;
                    resp = await client.GetAsync(requestUrl);
                }
                else if (method.ToUpper() == "POST")
                {
                    // Add oauth signature to paramDict
                    string signature = OAuthCalculateSignature("POST", APIUrl, paramDict, AccessTokenSecret);
                    paramDict["oauth_signature"] = signature;
                    resp = await client.PostAsync(APIUrl, new FormUrlEncodedContent(paramDict));
                }
                
                // Process results
                resp.EnsureSuccessStatusCode();

                var result = await resp.Content.ReadAsStringAsync();
                if (shouldEnsureStatOK)
                {
                    JObject json = JObject.Parse(result);
                    string status = json["stat"].ToString();
                    if (status == "ok")
                    {
                        retVal.Success = true;
                        retVal.Result = result;
                    }
                    else 
                    {
                        retVal.Success = false;
                        retVal.ErrorMessage = json["message"].ToString();

                        if (failure != null)
                        {
                            failure(retVal.ErrorMessage);
                        }
                    }

                }
                else
                {
                    retVal.Success = true;
                    retVal.Result = result;

                    if (success != null)
                    {
                        success(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                retVal.Success = false;
                retVal.ErrorMessage = ex.Message;

                if (failure != null)
                {
                    failure(ex.Message);
                }
            }

            return retVal;
        }

    }
}
