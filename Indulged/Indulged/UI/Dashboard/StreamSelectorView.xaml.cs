using Indulged.API.Storage;
using Indulged.API.Utils;
using Indulged.UI.Common.Controls;
using Indulged.UI.Dashboard.Events;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Dashboard
{
    public sealed partial class StreamSelectorView : UserControl
    {
        // Events
        public EventHandler<StreamSelectionChangedEventArgs> StreamSelectionChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamSelectorView()
        {
            this.InitializeComponent();

            // Default stream set to "Discovery"
            StreamNameLabel.Text = StorageService.Instance.DiscoveryStream.Name;
        }

        private void SelectorButton_Click(object sender, RoutedEventArgs e)
        {
            var contentView = new StreamSelectorDialog();
            var dialog = ModalPopup.Show(contentView, "Choose Stream", new List<string> { "Confirm", "Cancel" });
            dialog.DismissWithButtonClick += (s, evt) =>
            {
                if (evt.ButtonIndex == 0)
                {
                    var streamEvent = new StreamSelectionChangedEventArgs();
                    streamEvent.SelectedStream = contentView.SelectedStream;

                    // Update title
                    StreamNameLabel.Text = contentView.SelectedStream.Name;

                    StreamSelectionChanged.DispatchEvent(this, streamEvent);
                }
            };
        }
    }
}
