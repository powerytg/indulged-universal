using Indulged.API.Utils;
using Indulged.API.Storage.Models;
using Indulged.PolKit.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.PolKit
{
    public partial class PolicyKit
    {
        // Events
        public EventHandler<PolicyChangedEventArgs> PolicyChanged;

        // Licenses
        public Dictionary<string, FlickrLicense> Licenses;

        // Singleton
        private static PolicyKit instance;

        public static PolicyKit Instance
        {
            get
            {
                if (instance == null)
                    instance = new PolicyKit();

                return instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PolicyKit()
        {
            Licenses = new Dictionary<string, FlickrLicense>();
            Licenses["0"] = new FlickrLicense { Name = "All Rights Reserved", Url = null };
            Licenses["1"] = new FlickrLicense { Name = "Non Commercial Share Alike License", Url = "http://creativecommons.org/licenses/by-nc-sa/2.0/" };
            Licenses["2"] = new FlickrLicense { Name = "Non Commercial License", Url = "http://creativecommons.org/licenses/by-nc/2.0/" };
            Licenses["3"] = new FlickrLicense { Name = "Non Commercial No Derivs License", Url = "http://creativecommons.org/licenses/by-nc-nd/2.0/" };
            Licenses["4"] = new FlickrLicense { Name = "Attribution License", Url = "http://creativecommons.org/licenses/by/2.0/" };
            Licenses["5"] = new FlickrLicense { Name = "Share Alike License", Url = "http://creativecommons.org/licenses/by-sa/2.0/" };
            Licenses["6"] = new FlickrLicense { Name = "No Derivs License", Url = "http://creativecommons.org/licenses/by-nd/2.0/" };
            Licenses["7"] = new FlickrLicense { Name = "No known copyright restrictions", Url = "http://www.flickr.com/commons/usage/" };
            Licenses["8"] = new FlickrLicense { Name = "United States Government Work", Url = "http://www.usa.gov/copyright.shtml" };
        }

        private bool _useBlurredBackground = true;
        public bool UseBlurredBackground
        {
            get
            {
                return _useBlurredBackground;
            }

            set
            {
                if (_useBlurredBackground != value)
                {
                    _useBlurredBackground = value;

                    var evt = new PolicyChangedEventArgs();
                    evt.PolicyName = PolicyConstants.UseBlurredBackground;
                    PolicyChanged.DispatchEvent(this, evt);
                }                
            }
        }


    }
}
