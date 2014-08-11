using Indulged.API.Storage;
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
    public sealed partial class StreamSelectorDialog : UserControl
    {
        public FlickrPhotoStream SelectedStream { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamSelectorDialog()
        {
            this.InitializeComponent();

            var streams = new List<FlickrPhotoStream>();
            streams.Add(StorageService.Instance.CurrentUser.PhotoStream);
            streams.Add(StorageService.Instance.DiscoveryStream);
            streams.Add(StorageService.Instance.FavouriteStream);
            streams.Add(StorageService.Instance.ContactStream);

            foreach (var stream in streams)
            {
                var entry = new RadioButton();
                entry.Style = Application.Current.Resources["MainRadioButtonStyle"] as Style;
                entry.Content = stream.Name;
                entry.GroupName = "Stream";
                entry.Checked += (sender, e) =>
                {
                    SelectedStream = stream;
                };

                LayoutRoot.Children.Add(entry);
            }
        }
    }
}
