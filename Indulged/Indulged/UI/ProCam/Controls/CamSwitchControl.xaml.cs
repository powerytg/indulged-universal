using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProCam.Controls
{
    public sealed partial class CamSwitchControl : UserControl
    {
        // Events
        public EventHandler CameraChanged;

        private BitmapImage backCamIcon = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/MainCamera.png"));
        private BitmapImage frontCamIcon = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/FrontCamera.png"));

        private bool _shouldUseShortNames = false;
        public bool ShouldUserShortNames
        {
            get
            {
                return _shouldUseShortNames;
            }

            set
            {
                _shouldUseShortNames = value;
                UpdateLabel();
            }
        }

        private Windows.Devices.Enumeration.Panel _currentCamera = Windows.Devices.Enumeration.Panel.Back;
        public Windows.Devices.Enumeration.Panel CurrentCamera
        {
            get
            {
                return _currentCamera;
            }

            set
            {
                _currentCamera = value;

                if (_currentCamera == Windows.Devices.Enumeration.Panel.Back)
                {
                    Icon.Source = backCamIcon;
                }
                else if (_currentCamera == Windows.Devices.Enumeration.Panel.Front)
                {
                    Icon.Source = frontCamIcon;
                }

                UpdateLabel();
            }
        }

        // Constructor
        public CamSwitchControl()
        {
            InitializeComponent();
        }

        private void UpdateLabel()
        {
            if (_currentCamera == Windows.Devices.Enumeration.Panel.Back)
            {
                Label.Text = _shouldUseShortNames ? "BACK" : "BACK CAMERA";
            }
            else if (_currentCamera == Windows.Devices.Enumeration.Panel.Front)
            {
                Label.Text = _shouldUseShortNames ? "FRONT" : "FRONT CAMERA";
            }
        }

        public void LayoutInLandscapeMode()
        {
            ShouldUserShortNames = true;
        }

        public void LayoutInPortraitMode()
        {
            ShouldUserShortNames = false;
        }

        private void LayoutRoot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_currentCamera == Windows.Devices.Enumeration.Panel.Front)
            {
                CurrentCamera = Windows.Devices.Enumeration.Panel.Back;
            }
            else if (_currentCamera == Windows.Devices.Enumeration.Panel.Back)
            {
                CurrentCamera = Windows.Devices.Enumeration.Panel.Front;
            }

            if (CameraChanged != null)
            {
                CameraChanged(this, null);
            }
        }
    }
}
