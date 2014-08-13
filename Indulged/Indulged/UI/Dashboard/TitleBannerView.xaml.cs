using Indulged.UI.Dashboard.Events;
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

namespace Indulged.UI.Dashboard
{
    public sealed partial class TitleBannerView : UserControl
    {
        private Image VisibleImageView;
        private Image TransitionImageView;

        public void ShowLightTitleBar()
        {
            PerformTransition("/Assets/Dashboard/LightBanner.png");
        }

        public void ShowDarkTitleBar()
        {
            PerformTransition("/Assets/Dashboard/DarkBanner.png");
        }

        private void PerformTransition(string imageUrl)
        {
            TransitionImageView.Source = new BitmapImage(new Uri(TransitionImageView.BaseUri, imageUrl));
            TransitionImageView.Opacity = 1;

            Storyboard animation = new Storyboard();
            Duration dur = new Duration(TimeSpan.FromSeconds(0.3));
            animation.Duration = dur;

            DoubleAnimation fadeOutAnimation = new DoubleAnimation();
            animation.Children.Add(fadeOutAnimation);
            fadeOutAnimation.Duration = dur;
            fadeOutAnimation.To = 0;
            fadeOutAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(fadeOutAnimation, VisibleImageView);
            Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");

            animation.Completed += (s, e) => 
            {
                if (TransitionImageView == ImageView1)
                {
                    TransitionImageView = ImageView2;
                    VisibleImageView = ImageView1;
                }
                else
                {
                    TransitionImageView = ImageView1;
                    VisibleImageView = ImageView2;
                }

                TransitionImageView.Source = null;
            };
            
            animation.Begin();
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public TitleBannerView()
        {
            this.InitializeComponent();

            VisibleImageView = ImageView1;
            TransitionImageView = ImageView2;

            // Events
            DashboardThemeManager.Instance.ThemeChanged += OnThemeChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            DashboardThemeManager.Instance.ThemeChanged -= OnThemeChanged;
        }

        private void OnThemeChanged(object sender, DashboardThemeChangedEventArgs e)
        {
            if (e.SelectedTheme == DashboardThemes.Dark)
            {
                ShowDarkTitleBar();
            }
            else if (e.SelectedTheme == DashboardThemes.Light)
            {
                ShowLightTitleBar();
            }
        }
    }
}
