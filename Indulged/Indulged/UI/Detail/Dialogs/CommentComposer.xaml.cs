using Indulged.API.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace Indulged.UI.Detail.Dialogs
{
    public sealed partial class CommentComposer : UserControl
    {
        public string PhotoId { get; set; }

        public RoutedEventHandler PostButtonClicked;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentComposer()
        {
            this.InitializeComponent();
        }

        public async Task<bool> PostCommentAsync()
        {
            if (MessageTextBox.Text.Trim().Length == 0)
            {
                StatusView.ErrorText = "Content cannot be empty";
                StatusView.Opacity = 1;
                StatusView.ShowErrorScreen();
                return false;
            }

            // Disable UI elements
            MessageTextBox.IsEnabled = false;
            SendButton.IsEnabled = false;

            // Show loading screen
            StatusView.Opacity = 1;
            StatusView.ShowLoadingScreen();

            var status = await APIService.Instance.AddCommentAsync(PhotoId, MessageTextBox.Text);
            if (!status.Success)
            {
                StatusView.ErrorText = status.ErrorMessage;
                StatusView.ShowErrorScreen();
            }

            return status.Success;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (PostButtonClicked != null)
            {
                PostButtonClicked(this, null);
            }
        }

    }
}
