using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private static BitmapImage flashIconAuto = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/FlashAuto.png"));
        private static BitmapImage flashIconOn = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/FlashOn.png"));
        private static BitmapImage flashIconOff = new BitmapImage(new Uri("ms-appx:///Assets/ProCam/FlashOff.png"));

        private void ShowLoadingView()
        {
            HideUIChrome();
            LoadingView.Visibility = Visibility.Visible;
        }

        private void HideLoadingView()
        {
            LoadingView.Visibility = Visibility.Collapsed;
            ShowUIChrome();
        }

        private void ShowUIChrome()
        {
            LayoutRoot.IsHitTestVisible = true;
            Chrome.Visibility = Visibility.Visible;
        }

        private void HideUIChrome()
        {
            LayoutRoot.IsHitTestVisible = false;
            Chrome.Visibility = Visibility.Collapsed;
        }

        private void InitializeChrome()
        {
            // EV
            if (!evSupported)
            {
                EVDialer.Opacity = 0.3;
                EVDialer.IsEnabled = false;
            }
            else
            {
                EVDialer.SupportedValues = supportedEVValues;
            }

            // ISO
            if (!isoSupported)
            {
                ISODialer.Opacity = 0.3;
                ISODialer.IsEnabled = false;
            }
            else
            {
                ISODialer.SupportedValues = supportedISOValues;
            }

            // White balance
            if (wbSupported)
            {                
                OSD.GetWhiteBalanceOSD().SupportedWhiteBalances = supportWhiteBalances;
                WBButton.IsEnabled = true;
            }
            else
            {
                OSD.GetWhiteBalanceOSD().SupportedWhiteBalances = new System.Collections.Generic.List<Windows.Media.Devices.ColorTemperaturePreset>();
                WBButton.IsEnabled = false;
            }

            // Resolution
            OSD.GetMainOSD().SupportedResolutions = supportedResolutions;
            OSD.GetMainOSD().CurrentResolution = currentResolution;

            // Scene modes
            OSD.GetSceneOSD().SupportedSceneModes = availableSceneModes;

            // Focus assist
            if (focusAssistSupported)
            {
                OSD.GetFocusAssistOSD().SupportedModes = supportedFocusAssistModes;
                OSD.GetFocusAssistOSD().CurrentIndex = 0;
            }

            // Hide loading view
            HideLoadingView();
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

        private void LayoutInLandscapeMode()
        {
            LandscapeShutterButton.Visibility = Visibility.Visible;
            PortraitShutterButton.Visibility = Visibility.Collapsed;

            EVDialer.HorizontalAlignment = HorizontalAlignment.Left;
            EVDialer.Margin = new Thickness(140, 0, 0, 20);

            CameraSwitchButton.Margin = new Thickness(0, 0, 155, 120);
        }

        private void LayoutInPortraitMode()
        {
            LandscapeShutterButton.Visibility = Visibility.Collapsed;
            PortraitShutterButton.Visibility = Visibility.Visible;

            EVDialer.HorizontalAlignment = HorizontalAlignment.Right;
            EVDialer.Margin = new Thickness(0, 0, 20, 20);

            CameraSwitchButton.Margin = new Thickness(20, 0, 15, 180);
        }

    }
}
