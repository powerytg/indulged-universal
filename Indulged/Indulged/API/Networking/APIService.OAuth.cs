using Indulged.API.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Indulged.API.Networking
{
    /// <summary>
    /// Special thanks to wonderful Flick.Net library! Some of the source code is modified from it
    /// </summary>
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

        public string APIUrl
        {
            get
            {
                return "https://api.flickr.com/services/rest/";
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
            string encodedUrl = UrlUtils.Encode(apiUrl);
            string encodedParameters = UrlUtils.Encode(parameters);

            //generate the basestring
            string basestring = httpMethod + "&" + encodedUrl + "&" + encodedParameters;

            //hmac-sha1 encryption:
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            //create key (request_token can be an empty string)
            string key = consumerSecret + "&" + secret;
            string signature = Sha1Encrypt(basestring, key);

            //encode the signature to make it url safe and return the encoded url
            return UrlUtils.Encode(signature);
        }

        // Get request token
        public async void GetRequestTokenAsync(Action success, Action<string> failure)
        {
            string timestamp = DateTimeUtils.GetTimestamp();
            string nonce = GenerateNonce();

            // Encode the request string
            string paramString = "oauth_callback=" + UrlUtils.Encode(CallbackUrl);
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

        public async void GetAccessTokenAsync(Action<WwwFormUrlDecoder> success, Action<string> failure)
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

                    // Dispatch a login-success event
                    Debug.WriteLine(result);
                    success(dict);
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

        public Dictionary<string, string> GetBaseRequestParameters()
        {
            string timestamp = DateTimeUtils.GetTimestamp();
            string nonce = GenerateNonce();

            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            paramDict["format"] = "json";
            paramDict["nojsoncallback"] = "1";
            paramDict["oauth_consumer_key"] = consumerKey;
            paramDict["oauth_nonce"] = nonce;
            paramDict["oauth_signature_method"] = "HMAC-SHA1";
            paramDict["oauth_timestamp"] = timestamp;
            paramDict["oauth_token"] = AccessToken;
            paramDict["oauth_version"] = "1.0";

            return paramDict;
        }

        private string EscapeOAuthString(string text)
        {
            string value = text;

            value = Uri.EscapeDataString(value).Replace("+", "%20");

            // UrlEncode escapes with lowercase characters (e.g. %2f) but oAuth needs %2F
            value = Regex.Replace(value, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());

            // these characters are not escaped by UrlEncode() but needed to be escaped
            value = value.Replace("(", "%28").Replace(")", "%29").Replace("$", "%24").Replace("!", "%21").Replace(
                "*", "%2A").Replace("'", "%27");

            // these characters are escaped by UrlEncode() but will fail if unescaped!
            value = value.Replace("%7E", "~");

            return value;
        }

        private string OAuthCalculateSignature(string method, string url, Dictionary<string, string> parameters, string tokenSecret)
        {
            string baseString = "";
            string key = consumerSecret + "&" + tokenSecret;
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            var sorted = parameters.OrderBy(p => p.Key);

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in sorted)
            {
                sb.Append(pair.Key);
                sb.Append("=");
                sb.Append(EscapeOAuthString(pair.Value));
                sb.Append("&");
            }

            sb.Remove(sb.Length - 1, 1);

            baseString = method + "&" + EscapeOAuthString(url) + "&" + EscapeOAuthString(sb.ToString());
            string signature = Sha1Encrypt(baseString, key);
            return signature;
        }
    }
}
