using Indulged.API.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Indulged.API.Networking
{
    public partial class APIService
    {
        // OAuth2 client id. This is actually the iOS app id
        private string consumerKey = "eba49551fb292408a090ce891260cfca";

        // OAuth2 client secret
        private string consumerSecret = "94a739276ccbe2f6";

        // OAuth token
        public string RequestToken { get; set; }

        // OAuth token secret
        public string RequestTokenSecret { get; set; }

        // OAuth token verifier
        public string RequestTokenVerifier { get; set; }

        // OAuth access token
        public string AccessToken { get; set; }

        // OAuth access token secret
        public string AccessTokenSecret { get; set; }

        // Redirect Url
        public string CallbackUrl 
        {
            get
            {
                return "http://indulged.com/auth";
            }           
        }

        private string authUrl = "https://www.flickr.com/services";

        public string AuthorizeUrl
        {
            get
            {
                return authUrl + "/oauth/authorize?oauth_token=" + RequestToken + "&perms=write";
            }
        }

        private string GenerateNonce()
        {
            return Guid.NewGuid().ToString().Replace("-", null);
        }

        private string GenerateParamString(Dictionary<string, string> parameters)
        {
            string paramString = null;
            if (parameters != null)
            {
                var sortedParams = from key in parameters.Keys
                                   orderby key ascending
                                   select key;

                List<string> paramList = new List<string>();
                foreach (string key in sortedParams)
                {
                    string part = key + "=" + parameters[key];
                    paramList.Add(part);
                }

                paramString = string.Join("&", paramList);
                return paramString;
            }
            else
            {
                return null;
            }
        }

        private string GenerateParamString(List<KeyValuePair<string, string>> parameters)
        {
            parameters.Sort(KeyValuePairCompare);
            if (parameters != null)
            {
                parameters.Sort(KeyValuePairCompare);

                List<string> paramList = new List<string>();
                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    string part = pair.Key + "=" + pair.Value;
                    paramList.Add(part);
                }

                var paramString = string.Join("&", paramList);
                return paramString;
            }
            else
            {
                return null;
            }
        }

        static int KeyValuePairCompare(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        private string Sha1Encrypt(string baseString, string keyString)
        {
            var crypt = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
            var buffer = CryptographicBuffer.ConvertStringToBinary(baseString, BinaryStringEncoding.Utf8);
            var keyBuffer = CryptographicBuffer.ConvertStringToBinary(keyString, BinaryStringEncoding.Utf8);
            var key = crypt.CreateKey(keyBuffer);

            var sigBuffer = CryptographicEngine.Sign(key, buffer);
            string signature = CryptographicBuffer.EncodeToBase64String(sigBuffer);
            return signature;
        }

        private string GenerateSignature(string httpMethod, string secret, string apiUrl, string parameters)
        {
            string encodedUrl = UrlHelper.Encode(apiUrl);
            string encodedParameters = UrlHelper.Encode(parameters);

            //generate the basestring
            string basestring = httpMethod + "&" + encodedUrl + "&" + encodedParameters;

            //hmac-sha1 encryption:
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            //create key (request_token can be an empty string)
            string key = consumerSecret + "&" + secret;
            string signature = Sha1Encrypt(basestring, key);

            //encode the signature to make it url safe and return the encoded url
            return UrlHelper.Encode(signature);
        }

        // Get request token
        public async void GetRequestTokenAsync(Action success, Action<string> failure)
        {
            string timestamp = DateTimeUtils.GetTimestamp();
            string nonce = GenerateNonce();

            // Encode the request string
            string paramString = "oauth_callback=" + UrlHelper.Encode(CallbackUrl);
            paramString += "&oauth_consumer_key=" + consumerKey;
            paramString += "&oauth_nonce=" + nonce;
            paramString += "&oauth_signature_method=HMAC-SHA1";
            paramString += "&oauth_timestamp=" + timestamp;
            paramString += "&oauth_version=1.0";

            string signature = GenerateSignature("GET", null, authUrl + "/oauth/request_token", paramString);

            // Create the http request
            string requestUrl = authUrl + "/oauth/request_token?" + paramString + "&oauth_signature=" + signature;

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.GetAsync(requestUrl);
                resp.EnsureSuccessStatusCode();

                var result = await resp.Content.ReadAsStringAsync();

                if (result.Contains("oauth_callback_confirmed=true"))
                {
                    // Parse out the request token and secret
                    string[] parts = result.Split('&');
                    string tokenString = parts[1];
                    RequestToken = tokenString.Split('=')[1];

                    string secretString = parts[2];
                    RequestTokenSecret = secretString.Split('=')[1];

                    // Return success
                    Debug.WriteLine(result);
                    success();
                }
                else
                {
                    // Failed
                    Debug.WriteLine(result);
                    failure(result);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                failure(ex.Message);
            }
        }

        public async void GetAccessTokenAsync(Action success, Action<string> failure)
        {
            string timestamp = DateTimeUtils.GetTimestamp();
            string nonce = GenerateNonce();

            // Encode the request string
            string paramString = "oauth_consumer_key=" + consumerKey;
            paramString += "&oauth_nonce=" + nonce;
            paramString += "&oauth_signature_method=HMAC-SHA1";
            paramString += "&oauth_timestamp=" + timestamp;
            paramString += "&oauth_token=" + RequestToken;
            paramString += "&oauth_verifier=" + RequestTokenVerifier;
            paramString += "&oauth_version=1.0";

            string signature = GenerateSignature("GET", RequestTokenSecret, authUrl + "/oauth/access_token", paramString);

            // Create the http request
            string requestUrl = authUrl + "/oauth/access_token?" + paramString + "&oauth_signature=" + signature;

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.GetAsync(requestUrl);
                resp.EnsureSuccessStatusCode();

                var result = await resp.Content.ReadAsStringAsync();
                if (result.StartsWith("fullname="))
                {
                    var dict = new WwwFormUrlDecoder(result);
                    AccessToken = dict.GetFirstValueByName("oauth_token");
                    AccessTokenSecret = dict.GetFirstValueByName("oauth_token_secret");

                    // Construct current user object
                    /*
                    User currentUser = new User();
                    currentUser.ResourceId = Uri.UnescapeDataString(dict["user_nsid"]);
                    currentUser.Name = dict["fullname"];
                    currentUser.UserName = dict["username"];

                    Cinderella.Cinderella.CinderellaCore.UserCache[currentUser.ResourceId] = currentUser;
                    Cinderella.Cinderella.CinderellaCore.CurrentUser = currentUser;
                    Cinderella.Cinderella.CinderellaCore.SaveCurrentUserInfo();
                     * */

                    // Dispatch a login-success event
                    Debug.WriteLine(result);
                    success();
                }
                else
                {
                    Debug.WriteLine(result);
                    failure(result);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                failure(ex.Message);
            }

        }

    }
}
