using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Networking
{
    public class APIService
    {

        private string containerKey = "oauth";

        public void SaveAccessCredentials()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (!settings.Containers.ContainsKey(containerKey))
            {
                settings.CreateContainer(containerKey, Windows.Storage.ApplicationDataCreateDisposition.Always);
            }

            var values = settings.Containers[containerKey].Values;

            if (values.ContainsKey("accessToken"))
            {
                values["accessToken"] = AccessToken;
            }
            else
            {
                values.Add("accessToken", AccessToken);
            }

            Debug.WriteLine("access token saved: " + AccessToken);

            if (values.ContainsKey("accessTokenSecret"))
            {
                values["accessTokenSecret"] = AccessTokenSecret;
            }
            else
            {
                values.Add("accessTokenSecret", AccessTokenSecret);
            }

            Debug.WriteLine("access token secret saved: " + AccessTokenSecret);
        }

        public bool RetrieveAcessCredentials()
        {
            bool result = true;
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (!settings.Containers.ContainsKey(containerKey))
            {
                return false;
            }

            var values = settings.Containers[containerKey].Values;
            if (values.ContainsKey("accessToken"))
            {
                AccessToken = values["accessToken"] as string;
                Debug.WriteLine("access token retrieved: " + AccessToken);
            }
            else
            {
                Debug.WriteLine("access token not found");
                result = false;
            }

            if (values.ContainsKey("accessTokenSecret"))
            {
                AccessTokenSecret = values["accessTokenSecret"] as string;
                Debug.WriteLine("access token secret retrieved: " + AccessTokenSecret);
            }
            else
            {
                Debug.WriteLine("access token secret not found");
                result = false;
            }

            return result;
        }

        public void ClearAcessCredentials()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            settings.DeleteContainer(containerKey);
        }
    
    }
}
