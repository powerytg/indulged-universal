using Indulged.UI.Common.PhotoStream.Factories;
using Indulged.UI.Models;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.PhotoStream
{
    public class PhotoTileRendererBase : UserControl
    {
        protected double cellMargin = 2;
        protected double cellSize = 0;

        private bool needLayout = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoTileRendererBase()
            : base()
        {
            // Events
            this.SizeChanged += OnSizeChanged;
        }

        public static readonly DependencyProperty PhotoTileSourceProperty = DependencyProperty.Register(
        "PhotoTileSource",
        typeof(PhotoTile),
        typeof(PhotoTileRendererBase),
        new PropertyMetadata(null, OnPhotoTilePropertyChanged));

        public PhotoTile PhotoTileSource
        {
            get { return (PhotoTile)GetValue(PhotoTileSourceProperty); }
            set { SetValue(PhotoTileSourceProperty, value); }
        }

        private static void OnPhotoTilePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (PhotoTileRendererBase)sender;
            target.OnPhotoTileChanged();
        }

        protected virtual void OnPhotoTileChanged()
        {
            if (cellSize == 0)
            {
                needLayout = true;         
            }
            else
            {
                LayoutCells(Width);
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Calculate cell size
            if (e.NewSize.Width != 0 && !double.IsNaN(e.NewSize.Width))
            {
                cellSize = Math.Floor(e.NewSize.Width / PhotoTileLayoutGeneratorBase.MAX_COL_COUNT);

                if (PhotoTileSource != null && needLayout)
                {
                    LayoutCells(e.NewSize.Width);
                }
            }            
        }

        protected virtual void LayoutCells(double containerWidth)
        {
            needLayout = false;
        }

    }
}
