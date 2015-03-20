using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private void InitializeChrome()
        {
            if (!evSupported)
            {
                EVDialer.Opacity = 0.3;
                EVDialer.IsEnabled = false;
            }
            else
            {
                EVDialer.SupportedValues = supportedEVValues;
            }

            if (!isoSupported)
            {
                ISODialer.Opacity = 0.3;
                ISODialer.IsEnabled = false;
            }
            else
            {
                ISODialer.SupportedValues = supportedISOValues;
            }
        }

        private void HideLandscapeShutterButton()
        {
            CameraSwitchButton.Visibility = Visibility.Collapsed;
            LandscapeShutterButton.Visibility = Visibility.Collapsed;
        }

        private void ShowLandscapeShutterButton()
        {
            Storyboard storyboard = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.5));
            storyboard.Duration = duration;

            if (LandscapeShutterButton.Visibility != Visibility.Visible)
            {
                TranslateTransform tf = (TranslateTransform)LandscapeShutterButton.RenderTransform;
                tf.X = 200;
                LandscapeShutterButton.Visibility = Visibility.Visible;

                DoubleAnimation shutterAnimation = new DoubleAnimation();
                shutterAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                shutterAnimation.To = 0;
                shutterAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
                Storyboard.SetTarget(shutterAnimation, LandscapeShutterButton);
                Storyboard.SetTargetProperty(shutterAnimation, "(UIElement.RenderTransform).(TranslateTransform.X)");
                storyboard.Children.Add(shutterAnimation);
            }


            if (availableCameras.Count > 1)
            {
                if (CameraSwitchButton.Visibility != Visibility.Visible)
                {
                    TranslateTransform switchTF = (TranslateTransform)CameraSwitchButton.RenderTransform;
                    switchTF.X = 200;
                    CameraSwitchButton.Visibility = Visibility.Visible;

                    DoubleAnimation switchAnimation = new DoubleAnimation();
                    switchAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
                    switchAnimation.To = 0;
                    switchAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
                    Storyboard.SetTarget(switchAnimation, CameraSwitchButton);
                    Storyboard.SetTargetProperty(switchAnimation, "(UIElement.RenderTransform).(TranslateTransform.X)");
                    storyboard.Children.Add(switchAnimation);
                }
            }


            if (storyboard.Children.Count != 0)
            {
                storyboard.Begin();
            }
        }

        private void OnShutterButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
