using Indulged.API.Storage.Models;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.PhotoStream
{
    public sealed partial class PhotoTileRenderer3 : PhotoTileRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoTileRenderer3()
        {
            this.InitializeComponent();
        }

        protected override void OnPhotoTileChanged()
        {
            base.OnPhotoTileChanged();
            Renderer1.PhotoSource = PhotoTileSource.Photos[0];
            Renderer2.PhotoSource = PhotoTileSource.Photos[1];
            Renderer3.PhotoSource = PhotoTileSource.Photos[2];

            LayoutPhotos();
        }

        private void LayoutPhotos()
        {
            LayoutRoot.RowDefinitions.Clear();
            LayoutRoot.ColumnDefinitions.Clear();

            Renderer1.ClearValue(Grid.ColumnSpanProperty);
            Renderer1.ClearValue(Grid.RowSpanProperty);
            Renderer2.ClearValue(Grid.ColumnSpanProperty);
            Renderer2.ClearValue(Grid.RowSpanProperty);
            Renderer3.ClearValue(Grid.ColumnSpanProperty);
            Renderer3.ClearValue(Grid.RowSpanProperty);

            if (IsPortraitAspectRatio(Renderer1.PhotoSource))
            {
                // Large photo on left
                LayoutFirstPhotoOnLeft();
            }
            else if (IsPortraitAspectRatio(Renderer3.PhotoSource))
            {
                // Large photo on right
                LayoutLastPhotoOnRight();
            }
            else
            {
                // Large photo on top
                LayoutFirstPhotoOnTop();
            }

        }

        private void LayoutFirstPhotoOnLeft()
        {
            this.MaxHeight = 340;

            float hRatio = GetHorizontalRatio(Renderer1.PhotoSource, Renderer2.PhotoSource);
            float vRatio = GetVerticalRatio(Renderer2.PhotoSource, Renderer3.PhotoSource);

            int left = (int)Math.Floor(hRatio * 100);
            int right = 100 - left;
            int top = (int)Math.Floor(vRatio * 100);
            int bottom = 100 - top;

            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(left, GridUnitType.Star) });
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(right, GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(top, GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(bottom, GridUnitType.Star) });

            Renderer1.SetValue(Grid.ColumnProperty, 0);
            Renderer1.SetValue(Grid.RowSpanProperty, 2);

            Renderer2.SetValue(Grid.ColumnProperty, 1);
            Renderer2.SetValue(Grid.RowProperty, 0);

            Renderer3.SetValue(Grid.ColumnProperty, 1);
            Renderer3.SetValue(Grid.RowProperty, 1);

        }

        private void LayoutLastPhotoOnRight()
        {
            this.MaxHeight = 340;

            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(240, GridUnitType.Pixel) });
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            Renderer3.SetValue(Grid.ColumnProperty, 1);
            Renderer3.SetValue(Grid.RowSpanProperty, 2);

            Renderer1.SetValue(Grid.ColumnProperty, 0);
            Renderer1.SetValue(Grid.ColumnProperty, 0);

            Renderer2.SetValue(Grid.ColumnProperty, 0);
            Renderer2.SetValue(Grid.RowProperty, 1);

        }

        private void LayoutFirstPhotoOnTop()
        {
            this.MaxHeight = 480;

            float hRatio = GetHorizontalRatio(Renderer2.PhotoSource, Renderer3.PhotoSource);
            float vRatio = GetVerticalRatio(Renderer1.PhotoSource, Renderer2.PhotoSource);

            int left = (int)Math.Floor(hRatio * 100);
            int right = 100 - left;
            int top = (int)Math.Floor(vRatio * 100);
            top = Math.Max(top, 60);

            int bottom = 100 - top;

            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(left, GridUnitType.Star) });
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(right, GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(top, GridUnitType.Star) });
            LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(bottom, GridUnitType.Star) });

            Renderer1.SetValue(Grid.ColumnSpanProperty, 2);
            Renderer1.SetValue(Grid.RowProperty, 0);

            Renderer2.SetValue(Grid.ColumnProperty, 0);
            Renderer2.SetValue(Grid.RowProperty, 1);

            Renderer3.SetValue(Grid.ColumnProperty, 1);
            Renderer3.SetValue(Grid.RowProperty, 1);
        }

        private float GetHorizontalRatio(FlickrPhoto leftPhoto, FlickrPhoto rightPhoto)
        {
            float hRatio;
            if (leftPhoto.Width > 0 && rightPhoto.Width > 0)
            {
                hRatio = (float)leftPhoto.Width / (float)(leftPhoto.Width + rightPhoto.Width);
            }
            else
            {
                int f1 = Math.Min(leftPhoto.Width, leftPhoto.Height);
                int f2 = Math.Min(rightPhoto.Width, rightPhoto.Height);
                if (f1 != 0 && f2 != 0)
                {
                    hRatio = (float)f1 / (float)(f1 + f2);
                }
                else
                {
                    hRatio = 0.6f;
                }
            }

            if (hRatio < 0.4f)
            {
                hRatio = 0.4f;
            }

            if (hRatio > 0.75f)
            {
                hRatio = 0.75f;
            }

            return hRatio;
        }

        private float GetVerticalRatio(FlickrPhoto topPhoto, FlickrPhoto bottomPhoto)
        {
            float vRatio;
            if (topPhoto.Height > 0 && bottomPhoto.Height > 0)
            {
                vRatio = (float)topPhoto.Height / (float)(topPhoto.Height + bottomPhoto.Height);
            }
            else
            {
                int f1 = Math.Min(topPhoto.Width, topPhoto.Height);
                int f2 = Math.Min(bottomPhoto.Width, bottomPhoto.Height);
                if (f1 != 0 && f2 != 0)
                {
                    vRatio = (float)f1 / (float)(f1 + f2);
                }
                else
                {
                    vRatio = 0.6f;
                }
            }

            if (vRatio < 0.3f)
            {
                vRatio = 0.3f;
            }

            if (vRatio > 0.75f)
            {
                vRatio = 0.75f;
            }

            return vRatio;
        }

        private bool IsPortraitAspectRatio(FlickrPhoto photo)
        {
            int w = (photo.Width == 0) ? photo.Width : photo.Height;
            int h = (photo.Height == 0) ? photo.Height : photo.Height;

            if (w == 0 || h == 0)
            {
                return false;
            }
            else
            {
                return (w < h);
            }
        }

    }
}
