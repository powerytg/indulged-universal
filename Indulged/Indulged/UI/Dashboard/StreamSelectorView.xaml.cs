using Indulged.API.Utils;
using Indulged.API.Storage;
using Indulged.API.Storage.Models;
using Indulged.UI.Common.Controls;
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
using Windows.UI.Xaml.Navigation;

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
                    SelectorButton.Content = contentView.SelectedStream.Name.ToUpper();

                    StreamSelectionChanged.DispatchEvent(this, streamEvent);
                }
            };
        }
    }
}
