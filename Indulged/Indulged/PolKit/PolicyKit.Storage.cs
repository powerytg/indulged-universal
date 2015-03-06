using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

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
            SaveValue(values, PolicyConstants.UseBlurredBackground, UseBlurredBackground);

            // Prelude stream layout style
            SaveValue(values, PolicyConstants.PreludeStreamLayoutStyle, PreludeLayoutStyle);
        }

        public void RetrieveSettings()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (!settings.Containers.ContainsKey(containerKey))
            {
                return;
            }

            var values = settings.Containers[containerKey].Values;

            // Blurred background
            if (values.ContainsKey(PolicyConstants.UseBlurredBackground))
            {
                UseBlurredBackground = bool.Parse(values[PolicyConstants.UseBlurredBackground].ToString());
            }

            // Prelude stream layout style
            if (values.ContainsKey(PolicyConstants.PreludeStreamLayoutStyle))
            {
                PreludeLayoutStyle = values[PolicyConstants.PreludeStreamLayoutStyle].ToString();
            }

        }

        public void ClearAcessCredentials()
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            settings.DeleteContainer(containerKey);
        }

        private void SaveValue(IPropertySet dict, string key, object value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

    }
}
