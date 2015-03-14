using Indulged.API.Storage.Models;
using Indulged.UI.Common.Controls;
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

namespace Indulged.UI.Group.Dialogs
{
    public sealed partial class GroupJoinRequestDialog : UserControl, IModalPopupContent
    {
        public FlickrGroup Group { get; set; }

        // Reference to the modal popup container
        public ModalPopup PopupContainer { get; set; }

        private Button confirmButton;
        private Button cancelButton;
        public List<Button> Buttons { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GroupJoinRequestDialog()
        {
            this.InitializeComponent();

            Buttons = new List<Button>();
            confirmButton = new Button();
            confirmButton.Style = Application.Current.Resources["MainButtonStyle"] as Style;
            confirmButton.Content = "Confirm";
            confirmButton.Click += (sender, e) =>
            {
                JoinGroup();
            };


            cancelButton = new Button();
            cancelButton.Style = Application.Current.Resources["MainButtonStyle"] as Style;
            cancelButton.Content = "Cancel";
            cancelButton.Click += (sender, e) =>
            {
                PopupContainer.Dismiss();
            };

            Buttons.Add(confirmButton);
        }

        public void OnPopupRemoved()
        {
            PopupContainer = null;
            Group = null;
        }

        private void JoinGroup()
        {
            string trimmedQueryString = MessageBox.Text.Trim();
            if (trimmedQueryString.Length <= 0)
                return;

            var statusView = new GroupJoinRequestStatusDialog();
            statusView.Group = Group;
            statusView.Message = trimmedQueryString;
            statusView.PopupContainer = PopupContainer;

            PopupContainer.ReplaceContentWith("Join Group", statusView, statusView.Buttons, () =>
            {
                statusView.BeginJoinGroupRequest();
            });

        }

        private void MessageBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            string trimmedQueryString = MessageBox.Text.Trim();
            if (e.Key == Windows.System.VirtualKey.Enter && trimmedQueryString.Length > 0)
            {
                JoinGroup();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            JoinGroup();
        }
    }
}
