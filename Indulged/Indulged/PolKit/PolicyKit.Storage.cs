using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.PolKit
{
    public partial class PolicyKit
    {
        private string containerKey = "settings";

        public void SaveSettings()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (!settings.Containers.ContainsKey(containerKey))
            {
                settings.CreateContainer(containerKey, Windows.Storage.ApplicationDataCreateDisposition.Always);
            }

            var values = settings.Containers[containerKey].Values;

            // Blurred background
            if (values.ContainsKey(PolicyConstants.UseBlurredBackground))
            {
                values[PolicyConstants.UseBlurredBackground] = UseBlurredBackground;
            }
            else
            {
                values.Add(PolicyConstants.UseBlurredBackground, UseBlurredBackground);
            }
        }

        public void RetrieveSettings()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (!settings.Containers.ContainsKey(containerKey))
            {
                return;
            }

            var values = settings.Containers[containerKey].Values;
            if (values.ContainsKey(PolicyConstants.UseBlurredBackground))
            {
                UseBlurredBackground = bool.Parse(values[PolicyConstants.UseBlurredBackground].ToString());
            }
        }

        public void ClearAcessCredentials()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            settings.DeleteContainer(containerKey);
        }
    }
}
