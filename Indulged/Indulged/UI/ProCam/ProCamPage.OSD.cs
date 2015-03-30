using Indulged.UI.ProCam.HUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Indulged.UI.ProCam
{
    public partial class ProCamPage
    {
        private EVHUD evHUDView;
        private ISOHUD isoHUDView;

        #region EV OSD

        private void CreateEVHUD()
        {
            if (evHUDView != null)
            {
                return;
            }

            evHUDView = new EVHUD();
            evHUDView.SupportedValues = supportedEVValues;
            evHUDView.HorizontalAlignment = HorizontalAlignment.Right;
            evHUDView.Visibility = Visibility.Collapsed;
            TranslateTransform tf = evHUDView.RenderTransform as TranslateTransform;
            tf.X = evHUDView.Width;

            LayoutRoot.Children.Add(evHUDView);
            evHUDView.SelectedValue = EVDialer.CurrentValue;
        }

        public void ShowEVHUD()
        {
            if (evHUDView == null)
            {
                CreateEVHUD();
            }

            if (currentOrientation == DisplayOrientations.Landscape)
            {
                HideLandscapeShutterButton();

                evHUDView.VerticalAlignment = VerticalAlignment.Bottom;
                evHUDView.Margin = new Thickness(0, 0, 0, 30);
            }
            else
            {
                evHUDView.VerticalAlignment = VerticalAlignment.Bottom;
                evHUDView.Margin = new Thickness(0, 0, 0, CameraSwitchButton.Margin.Bottom + 85);
            }

            var tf = evHUDView.RenderTransform as TranslateTransform;
            tf.X = evHUDView.Width;
            evHUDView.Visibility = Visibility.Visible;

            Storyboard storyboard = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Duration = duration;

            DoubleAnimation panelAnimation = new DoubleAnimation();
            panelAnimation.Duration = duration;
            panelAnimation.To = 0;
            panelAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(panelAnimation, evHUDView);
            Storyboard.SetTargetProperty(panelAnimation, "(UIElement.RenderTransform).(TranslateTransform.X)");
            storyboard.Children.Add(panelAnimation);

            storyboard.Begin();
            storyboard.Completed += (sender, e) =>
            {
            };
        }

        public void DismissEVHUD()
        {
            if (evHUDView == null)
            {
                return;
            }

            Storyboard storyboard = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Duration = duration;

            DoubleAnimation panelAnimation = new DoubleAnimation();
            panelAnimation.Duration = duration;
            panelAnimation.To = evHUDView.Width;
            panelAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(panelAnimation, evHUDView);
            Storyboard.SetTargetProperty(panelAnimation, "(UIElement.RenderTransform).(TranslateTransform.X)");
            storyboard.Children.Add(panelAnimation);

            storyboard.Begin();
            storyboard.Completed += (sender, e) =>
            {
                evHUDView.Visibility = Visibility.Collapsed;

                if (currentOrientation == DisplayOrientations.Landscape)
                {
                    ShowLandscapeShutterButton();
                }

            };
        }

        #endregion

        #region ISO HUD

        private void CreateISOHUD()
        {
            if (isoHUDView != null)
            {
                return;
            }

            isoHUDView = new ISOHUD();
            isoHUDView.SupportedValues = supportedISOValues;
            isoHUDView.HorizontalAlignment = HorizontalAlignment.Left;
            isoHUDView.VerticalAlignment = VerticalAlignment.Center;
            isoHUDView.Visibility = Visibility.Collapsed;

            var tf = isoHUDView.RenderTransform as TranslateTransform;
            tf.X = -isoHUDView.Width;

            LayoutRoot.Children.Add(isoHUDView);
            isoHUDView.SelectedValue = ISODialer.CurrentValue;
        }

        public void ShowISOHUD()
        {
            if (isoHUDView == null)
            {
                CreateISOHUD();
            }

            if (currentOrientation == DisplayOrientations.Landscape)
            {
                HideLandscapeShutterButton();
            }

            var tf = isoHUDView.RenderTransform as TranslateTransform;
            tf.X = -isoHUDView.Width;
            isoHUDView.Visibility = Visibility.Visible;

            Storyboard storyboard = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Duration = duration;

            DoubleAnimation panelAnimation = new DoubleAnimation();
            panelAnimation.Duration = duration;
            panelAnimation.To = 0;
            panelAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(panelAnimation, isoHUDView);
            Storyboard.SetTargetProperty(panelAnimation, "(UIElement.RenderTransform).(TranslateTransform.X)");
            storyboard.Children.Add(panelAnimation);

            storyboard.Begin();
            storyboard.Completed += (sender, e) =>
            {

            };
        }

        public void DismissISOHUD()
        {
            if (isoHUDView == null)
            {
                return;
            }

            if (currentOrientation == DisplayOrientations.Landscape)
            {
                HideLandscapeShutterButton();
            }


            Storyboard storyboard = new Storyboard();
            Duration duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Duration = duration;

            DoubleAnimation panelAnimation = new DoubleAnimation();
            panelAnimation.Duration = duration;
            panelAnimation.To = -isoHUDView.Width;
            panelAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(panelAnimation, isoHUDView);
            Storyboard.SetTargetProperty(panelAnimation, "(UIElement.RenderTransform).(TranslateTransform.X)");
            storyboard.Children.Add(panelAnimation);

            storyboard.Begin();
            storyboard.Completed += (sender, e) =>
            {
                isoHUDView.Visibility = Visibility.Collapsed;

                if (currentOrientation == DisplayOrientations.Landscape)
                {
                    ShowLandscapeShutterButton();
                }

            };
        }

        #endregion

        #region OSD

        public void ShowOSD(FrameworkElement view = null)
        {
            if (currentOrientation == DisplayOrientations.Landscape)
            {
                HideLandscapeShutterButton();
            }

            if (view == null)
            {
                view = OSD.GetMainOSD();
            }

            OSD.ShowOSD(view);
            HUDSwitchButton.IsOn = true;
        }

        public void DismissOSD()
        {
            OSD.DismissOSD(() =>
            {
                if (currentOrientation == DisplayOrientations.Landscape)
                {
                    ShowLandscapeShutterButton();
                }

            });

            HUDSwitchButton.IsOn = false;
        }

        #endregion
    }
}
