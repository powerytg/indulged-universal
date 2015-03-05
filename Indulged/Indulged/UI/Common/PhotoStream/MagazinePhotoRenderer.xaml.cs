using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class MagazinePhotoRenderer : PhotoTileRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagazinePhotoRenderer()
            : base()
        {
            this.InitializeComponent();
        }

        protected override void LayoutCells(double containerWidth)
        {
            if (PhotoTileSource == null)
            {
                return;
            }

            base.LayoutCells(containerWidth);

            if (PhotoTileSource.Photos.Count == 1)
            {
                LayoutOneCell(containerWidth);
            } 
            else if(PhotoTileSource.Photos.Count == 2)
            {
                LayoutTwoCells(containerWidth);
            }
            else if(PhotoTileSource.Photos.Count == 3)
            {
                LayoutThreeCells(containerWidth);
            }
        }

        private void LayoutOneCell(double containerWidth)
        {
            if (PhotoTileSource == null)
            {
                return;
            }

            // Show or hide cells
            PhotoView2.Visibility = Visibility.Collapsed;
            PhotoView3.Visibility = Visibility.Collapsed;

            // Resize self
            var tileLayout = PhotoTileSource.LayoutConfigurations[0];
            Height = tileLayout.RowSpan * cellSize;

            // Position photo
            PhotoView1.Width = containerWidth;
            PhotoView1.Height = Height - cellMargin;

            // Set photos
            PhotoView1.Photo = PhotoTileSource.Photos[0];
        }

        private void LayoutTwoCells(double containerWidth)
        {
            if (PhotoTileSource == null)
            {
                return;
            }

            // Show or hide cells
            PhotoView2.Visibility = Visibility.Visible;
            PhotoView3.Visibility = Visibility.Collapsed;

            // Resize self
            var layout1 = PhotoTileSource.LayoutConfigurations[0];
            var layout2 = PhotoTileSource.LayoutConfigurations[1];

            Width = containerWidth;
            Height = layout1.RowSpan * cellSize;

            // Layout photos
            PhotoView1.Width = layout1.ColSpan * cellSize;
            PhotoView1.Height = Height - cellMargin;

            PhotoView2.Width = containerWidth - PhotoView1.Width - cellMargin;
            PhotoView2.Height = PhotoView1.Height;
            PhotoView2.SetValue(Canvas.LeftProperty, PhotoView1.Width + cellMargin);

            // Set photos
            PhotoView1.Photo = PhotoTileSource.Photos[0];
            PhotoView2.Photo = PhotoTileSource.Photos[1];
        }

        private void LayoutThreeCells(double containerWidth)
        {
            if (PhotoTileSource == null)
            {
                return;
            }

            // Show or hide cells
            PhotoView2.Visibility = Visibility.Visible;
            PhotoView3.Visibility = Visibility.Visible;

            // Resize self
            var layout1 = PhotoTileSource.LayoutConfigurations[0];
            var layout2 = PhotoTileSource.LayoutConfigurations[1];
            var layout3 = PhotoTileSource.LayoutConfigurations[2];

            Width = containerWidth;
            Height = layout1.RowSpan * cellSize;

            // Layout photos
            PhotoView1.Width = layout1.ColSpan * cellSize;
            PhotoView1.Height = Height - cellMargin;

            PhotoView2.Width = layout2.ColSpan * cellSize - cellMargin;
            PhotoView2.Height = layout2.RowSpan * cellSize;
            PhotoView2.SetValue(Canvas.LeftProperty, PhotoView1.Width + cellMargin);

            PhotoView3.Width = PhotoView2.Width;
            PhotoView3.Height = PhotoView1.Height - PhotoView2.Height - cellMargin;
            PhotoView3.SetValue(Canvas.LeftProperty, PhotoView1.Width + cellMargin);
            PhotoView3.SetValue(Canvas.TopProperty, PhotoView2.Height + cellMargin);

            // Set photos
            PhotoView1.Photo = PhotoTileSource.Photos[0];
            PhotoView2.Photo = PhotoTileSource.Photos[1];
            PhotoView3.Photo = PhotoTileSource.Photos[2];
        }
    }
}
