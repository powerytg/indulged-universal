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

namespace Indulged.UI.Dashboard
{
    public sealed partial class CatalogAlbumGridView : UserControl
    {
        private List<FlickrAlbum> albums;
        public List<FlickrAlbum> AlbumList
        {
            set
            {
                albums = value;
                UpdateDisplayList();
            }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public CatalogAlbumGridView()
        {
            this.InitializeComponent();
        }

        private void UpdateDisplayList()
        {
            int displayCount = Math.Min(4, albums.Count);
            switch (displayCount)
            {
                case 0:
                    EmptyAlbumLabel.Visibility = Visibility.Visible;
                    ViewAllButton.Visibility = Visibility.Collapsed;
                    Renderer1.Visibility = Visibility.Collapsed;
                    Renderer2.Visibility = Visibility.Collapsed;
                    Renderer3.Visibility = Visibility.Collapsed;
                    Renderer4.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    layoutOneAlbums();
                    break;
                case 2:
                    layoutTwoAlbums();
                    break;
                case 3:
                    layoutThreeAlbums();
                    break;
                case 4:
                    layoutFourAlbums();
                    break;
                default: 
                    break;
            }
            
        }

        private void layoutOneAlbums()
        {
            EmptyAlbumLabel.Visibility = Visibility.Collapsed;
            ViewAllButton.Visibility = Visibility.Visible;
            Renderer1.Visibility = Visibility.Visible;
            Renderer2.Visibility = Visibility.Collapsed;
            Renderer3.Visibility = Visibility.Collapsed;
            Renderer4.Visibility = Visibility.Collapsed;

            Renderer1.Album = albums[0];
        }

        private void layoutTwoAlbums()
        {
            EmptyAlbumLabel.Visibility = Visibility.Collapsed;
            ViewAllButton.Visibility = Visibility.Visible;
            Renderer1.Visibility = Visibility.Visible;
            Renderer2.Visibility = Visibility.Visible;
            Renderer3.Visibility = Visibility.Collapsed;
            Renderer4.Visibility = Visibility.Collapsed;

            Renderer1.Album = albums[0];
            Renderer2.Album = albums[1];
        }

        private void layoutThreeAlbums()
        {
            EmptyAlbumLabel.Visibility = Visibility.Collapsed;
            ViewAllButton.Visibility = Visibility.Visible;
            Renderer1.Visibility = Visibility.Visible;
            Renderer2.Visibility = Visibility.Visible;
            Renderer3.Visibility = Visibility.Visible;
            Renderer4.Visibility = Visibility.Collapsed;

            Renderer1.Album = albums[0];
            Renderer2.Album = albums[1];
            Renderer3.Album = albums[2];

            Grid2.ColumnDefinitions.Clear();
            Grid2.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Grid2.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            Renderer2.SetValue(Grid.ColumnProperty, 0);
            Grid3.SetValue(Grid.ColumnProperty, 1);
        }

        private void layoutFourAlbums()
        {
            EmptyAlbumLabel.Visibility = Visibility.Collapsed;
            ViewAllButton.Visibility = Visibility.Visible;
            Renderer1.Visibility = Visibility.Visible;
            Renderer2.Visibility = Visibility.Visible;
            Renderer3.Visibility = Visibility.Visible;
            Renderer4.Visibility = Visibility.Visible;

            Renderer1.Album = albums[0];
            Renderer2.Album = albums[1];
            Renderer3.Album = albums[2];
            Renderer4.Album = albums[3];

            Grid2.ColumnDefinitions.Clear();
            Grid2.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            Grid2.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            Renderer2.SetValue(Grid.ColumnProperty, 0);
            Grid3.SetValue(Grid.ColumnProperty, 1);

            Grid3.RowDefinitions.Clear();
            Grid3.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star)});
            Grid3.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star)});

            Renderer3.SetValue(Grid.RowProperty, 0);
            Renderer4.SetValue(Grid.RowProperty, 1);
        }

        private void ViewAllButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
