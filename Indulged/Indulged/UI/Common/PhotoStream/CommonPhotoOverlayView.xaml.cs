using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class CommonPhotoOverlayView : UserControl
    {
        public enum PhotoOverlayLayoutMode
        {
            Mini, Overlay, Full
        }

        private static SolidColorBrush transparentBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        private static SolidColorBrush solidBrush = new SolidColorBrush(Color.FromArgb(255, 25, 29, 36));
        private static SolidColorBrush semiTransparentBrush = new SolidColorBrush(Color.FromArgb(215, 25, 29, 36));

        public static readonly DependencyProperty PhotoSourceProperty = DependencyProperty.Register(
        "PhotoSource",
        typeof(FlickrPhoto),
        typeof(CommonPhotoOverlayView),
        new PropertyMetadata(null, OPhotoSourcePropertyChanged));

        public FlickrPhoto PhotoSource
        {
            get { return (FlickrPhoto)GetValue(PhotoSourceProperty); }
            set { SetValue(PhotoSourceProperty, value); }
        }

        private static void OPhotoSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CommonPhotoOverlayView)sender;
            target.OnPhotoSourceChanged();
        }

        private void OnPhotoSourceChanged()
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommonPhotoOverlayView()
        {
            this.InitializeComponent();
        }

        private PhotoOverlayLayoutMode _layoutMode;
        public PhotoOverlayLayoutMode LayoutMode
        {
            get
            {
                return _layoutMode;
            }

            set
            {
                _layoutMode = value;
                PerformLayout();
            }
        }

        private void PerformLayout()
        {
            switch (_layoutMode)
            {
                case PhotoOverlayLayoutMode.Mini:
                    LayoutInMiniMode();
                    break;
                case PhotoOverlayLayoutMode.Full:
                    LayoutInFullMode();
                    break;
                case PhotoOverlayLayoutMode.Overlay:
                    LayoutInOverlayMode();
                    break;
            }
        }

        private void LayoutInMiniMode()
        {
            LayoutRoot.Background = transparentBrush;

            if (ShouldShowTitle())
            {
                TitleLabel.Text = PhotoSource.Title;
                TitleLabel.Visibility = Visibility.Visible;
            }
            else
            {
                TitleLabel.Visibility = Visibility.Collapsed;
            }

            DescLabel.Visibility = Visibility.Collapsed;
            if (PhotoSource.CommentCount > 0 || PhotoSource.ViewCount > 0)
            {
                StatView.PhotoSource = PhotoSource;
                StatView.Visibility = Visibility.Visible;
            }
            else
            {
                StatView.Visibility = Visibility.Collapsed;
            }
        }

        private void LayoutInFullMode()
        {
            LayoutRoot.Background = solidBrush;

            if (ShouldShowTitle())
            {
                TitleLabel.Text = PhotoSource.Title;
                TitleLabel.Visibility = Visibility.Visible;
            }
            else
            {
                TitleLabel.Visibility = Visibility.Collapsed;
            }

            if (ShouldShowDescription())
            {
                DescLabel.Text = PhotoSource.Description;
                DescLabel.Visibility = Visibility.Visible;
            }
            else
            {
                DescLabel.Visibility = Visibility.Collapsed;
            }

            if (PhotoSource.CommentCount > 0 || PhotoSource.ViewCount > 0)
            {
                StatView.PhotoSource = PhotoSource;
                StatView.Visibility = Visibility.Visible;
            }
            else
            {
                StatView.Visibility = Visibility.Collapsed;
            }
        }

        private void LayoutInOverlayMode()
        {
            LayoutRoot.Background = semiTransparentBrush;

            if (ShouldShowTitle())
            {
                TitleLabel.Text = PhotoSource.Title;
                TitleLabel.Visibility = Visibility.Visible;
            }
            else
            {
                TitleLabel.Visibility = Visibility.Collapsed;
            }

            if (ShouldShowDescription())
            {
                DescLabel.Text = PhotoSource.Description;
                DescLabel.Visibility = Visibility.Visible;
            }
            else
            {
                DescLabel.Visibility = Visibility.Collapsed;
            }

            if (PhotoSource.CommentCount > 0 || PhotoSource.ViewCount > 0)
            {
                StatView.PhotoSource = PhotoSource;
                StatView.Visibility = Visibility.Visible;
            }
            else
            {
                StatView.Visibility = Visibility.Collapsed;
            }
        }

        private bool ShouldShowTitle()
        {
            if (PhotoSource.Title.Length == 0)
            {
                return false;
            }

            // Filter out blacklist
            if (IsTextInBlackList(PhotoSource.Title))
            {
                return false;
            }

            return true;
        }

        private bool ShouldShowDescription()
        {
            if (PhotoSource.Description.Length == 0)
            {
                return false;
            }

            // Filter out blacklist
            if (IsTextInBlackList(PhotoSource.Description))
            {
                return false;
            }

            return true;
        }

        public static bool IsTextInBlackList(string text)
        {
            string lowcaseText = text.ToLower();
            if (lowcaseText.StartsWith("untitled")
                || lowcaseText.StartsWith("img")
                || lowcaseText.StartsWith("http")
                || lowcaseText.StartsWith("<"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
