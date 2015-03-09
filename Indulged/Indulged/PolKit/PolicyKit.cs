using Indulged.API.Utils;
using Indulged.API.Storage.Models;
using Indulged.PolKit.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Indulged.UI.Models;

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

        // Blurred background
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
                    SaveSettings();

                    var evt = new PolicyChangedEventArgs();
                    evt.PolicyName = PolicyConstants.UseBlurredBackground;
                    PolicyChanged.DispatchEvent(this, evt);
                }                
            }
        }

        // Use clean, HTML-tag-free content for titles and descriptions
        private bool _useCleanText = true;
        public bool UseCleanText
        {
            get
            {
                return _useCleanText;
            }

            set
            {
                if (_useCleanText != value)
                {
                    _useCleanText = value;
                    SaveSettings();

                    var evt = new PolicyChangedEventArgs();
                    evt.PolicyName = PolicyConstants.UseCleanText;
                    PolicyChanged.DispatchEvent(this, evt);
                }
            }
        }

        // Prelude stream layout style
        private string _preludeLayoutStyle = StreamLayoutStyle.Journal;
        public string PreludeLayoutStyle
        {
            get
            {
                return _preludeLayoutStyle;
            }

            set
            {
                if (_preludeLayoutStyle != value)
                {
                    _preludeLayoutStyle = value;
                    SaveSettings();

                    var evt = new PolicyChangedEventArgs();
                    evt.PolicyName = PolicyConstants.PreludeStreamLayoutStyle;
                    PolicyChanged.DispatchEvent(this, evt);
                }
            }
        }

        // Show extra overlay on Prelude's photo tiles
        private bool _showOverlayOnPreludeTiles = true;
        public bool ShowOverlayOnPreludeTiles
        {
            get
            {
                return _showOverlayOnPreludeTiles;
            }

            set
            {
                if (_showOverlayOnPreludeTiles != value)
                {
                    _showOverlayOnPreludeTiles = value;
                    SaveSettings();

                    var evt = new PolicyChangedEventArgs();
                    evt.PolicyName = PolicyConstants.ShowOverlayOnPreludeTiles;
                    PolicyChanged.DispatchEvent(this, evt);
                }
            }
        }
    }
}
