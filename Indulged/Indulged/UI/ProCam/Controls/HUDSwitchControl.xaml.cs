using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProCam.Controls
{
    public sealed partial class HUDSwitchControl : UserControl
    {
        // Events
        public EventHandler HUDStateChanged;

        private BitmapImage OnLabelImage = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/OSDOn.png"));
        private BitmapImage OffLabelImage = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/OSDOff.png"));

        private bool _isOn = false;
        public bool IsOn
        {
            get
            {
                return _isOn;
            }

            set
            {
                _isOn = value;
                if (_isOn)
                {
                    Label.Source = OnLabelImage;
                    ArrowTransform.Angle = 0;
                }
                else
                {
                    Label.Source = OffLabelImage;
                    ArrowTransform.Angle = 90;
                }
            }
        }

        // Constructor
        public HUDSwitchControl()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            _isOn = !_isOn;
            double targetArrowAngle;

            if (_isOn)
            {
                Label.Source = OnLabelImage;
                targetArrowAngle = 0;
            }
            else
            {
                Label.Source = OffLabelImage;
                targetArrowAngle = 90;
            }

            if (HUDStateChanged != null)
            {
                HUDStateChanged(this, null);
            }

            Storyboard storyboard = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Duration = duration;

            DoubleAnimation panelAnimation = new DoubleAnimation();
            panelAnimation.Duration = duration;
            panelAnimation.To = targetArrowAngle;
            panelAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(panelAnimation, Arrow);
            Storyboard.SetTargetProperty(panelAnimation, "(UIElement.RenderTransform).(RotateTransform.Angle)");
            storyboard.Children.Add(panelAnimation);

            storyboard.Begin();
        }
    }
}
