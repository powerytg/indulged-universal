using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Events;
using Indulged.API.Storage.Models;
using Indulged.UI.Common.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Group.Dialogs
{
    public sealed partial class GroupJoinStatusDialog : UserControl, IModalPopupContent
    {
        public FlickrGroup Group { get; set; }

        // Reference to the modal popup container
        public ModalPopup PopupContainer { get; set; }

        private Button doneButton;
        public List<Button> Buttons { get; set; }

        public async void BeginJoinGroup()
        {
            doneButton.IsEnabled = false;

            APIResponse status;
            if (Group.Rules != null && Group.Rules.Length > 0){
                status = await APIService.Instance.JoinGroupAsync(Group.ResourceId, new Dictionary<string, string> { { "accept_rules", "1" } });
            }
            else
            {
                status = await APIService.Instance.JoinGroupAsync(Group.ResourceId);
            }

            if (!status.Success)
            {
                if (status.ErrorMessage.Contains("invitation only"))
                {
                    // Invitation only
                    var requestView = new GroupJoinRequestDialog();
                    requestView.Group = Group;
                    requestView.PopupContainer = PopupContainer;

                    PopupContainer.ReplaceContentWith("Join Request", requestView, requestView.Buttons);
                }
                else
                {
                    LoadingView.ErrorText = "Error joining group: " + status.ErrorMessage;
                    LoadingView.ShowErrorScreen();
                    doneButton.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GroupJoinStatusDialog()
        {
            this.InitializeComponent();

            Buttons = new List<Button>();
            doneButton = new Button();
            doneButton.Style = Application.Current.Resources["MainButtonStyle"] as Style;
            doneButton.Content = "Done";
            doneButton.Click += (sender, e) =>
            {
                PopupContainer.Dismiss();
            };

            Buttons.Add(doneButton);

            // Events
            StorageService.Instance.GroupJoined += OnGroupJoined;
        }

        public void OnPopupRemoved()
        {
            StorageService.Instance.GroupJoined -= OnGroupJoined;
            PopupContainer = null;
            Group = null;
        }

        private void OnGroupJoined(object sender, StorageEventArgs e)
        {
            if (e.GroupId != Group.ResourceId)
            {
                return;
            }

            PopupContainer.DismissWithActionAsync(() =>
            {
                var frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(GroupPage), e.GroupId);
            });
        }
    }
}
