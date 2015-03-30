using System;
using System.Collections.Generic;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProCam.HUD
{
    public sealed partial class MainHUD : UserControl
    {
        public Button GetSceneButton()
        {
            return SceneButton;
        }

        public Button GetFocusAssistButton()
        {
            return FocusAssistButton;
        }

        private VideoEncodingProperties _currentResolution;
        public VideoEncodingProperties CurrentResolution
        {
            get
            {
                return _currentResolution;
            }

            set
            {
                _currentResolution = value;
                if (_currentResolution != null)
                {
                    int index = _supportedResolutions.IndexOf(_currentResolution);
                    ResLabel.Text = GetResolutionDisplayText(_currentResolution);
                }
            }
        }

        private List<VideoEncodingProperties> _supportedResolutions;
        public List<VideoEncodingProperties> SupportedResolutions
        {
            get
            {
                return _supportedResolutions;
            }

            set
            {
                _supportedResolutions = value;
            }
        }

        // Constructor
        public MainHUD()
        {
            InitializeComponent();
        }

        private string GetResolutionDisplayText(VideoEncodingProperties res)
        {
            double result = (res.Width * res.Height) / 1000000;
            int intValue = (int)Math.Ceiling(result);

            return intValue.ToString() + "MP";
        }    
    }
}
