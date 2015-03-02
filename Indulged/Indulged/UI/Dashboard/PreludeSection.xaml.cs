using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using Indulged.UI.Dashboard.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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
    public sealed partial class PreludeSection : UserControl
    {
        private FlickrPhotoStream _currentStream;
        public FlickrPhotoStream CurrentStream
        {
            get
            {
                return _currentStream;
            }

            set
            {
                if (_currentStream != value)
                {
                    _currentStream = value;
                    PhotoListView.Stream = _currentStream;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PreludeSection()
        {
            this.InitializeComponent();

            // Initialize stream
            CurrentStream = StorageService.Instance.DiscoveryStream;

            // Events
            PhotoListView.LoadingStarted += OnLoadingStarted;
            PhotoListView.LoadingComplete += OnLoadingComplete;            
        }

        private async void OnLoadingStarted(object sender, EventArgs e)
        {
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.ShowAsync();
        }

        private async void OnLoadingComplete(object sender, EventArgs e)
        {
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.HideAsync();
        }

        private void OnStreamSelectionChanged(object sender, StreamSelectionChangedEventArgs e)
        {
            if (e.SelectedStream != null)
            {
                CurrentStream = e.SelectedStream;
            }            
        }

        private void SelectorView_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectorView == null)
            {
                SelectorView = sender as StreamSelectorView;
                SelectorView.StreamSelectionChanged -= OnStreamSelectionChanged;
                SelectorView.StreamSelectionChanged += OnStreamSelectionChanged;
            }

        }

    }
}
