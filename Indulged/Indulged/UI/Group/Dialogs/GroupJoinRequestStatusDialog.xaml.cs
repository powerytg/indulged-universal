using Indulged.API.Networking;
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
    public sealed partial class GroupJoinRequestStatusDialog : UserControl, IModalPopupContent
    {
        public FlickrGroup Group { get; set; }
        public string Message { get; set; }

        // Reference to the modal popup container
        public ModalPopup PopupContainer { get; set; }

        private Button doneButton;
        public List<Button> Buttons { get; set; }

        public async void BeginJoinGroupRequest()
        {
            doneButton.IsEnabled = false;

            APIResponse status;
            if (Group.Rules != null && Group.Rules.Length > 0){
                status = await APIService.Instance.SendJoinGroupRequestAsync(Group.ResourceId, Message, new Dictionary<string, string> { { "accept_rules", "1" } });
            } else {
                status = await APIService.Instance.SendJoinGroupRequestAsync(Group.ResourceId, Message);
            }

            if (status.Success)
            {
                LoadingView.LoadingText = "Your request has been sent";
                LoadingView.ShowLoadingCompleteScreen();
                doneButton.IsEnabled = true;
            }
            else
            {
                LoadingView.ErrorText = "Cannot send request: " + status.ErrorMessage;
                LoadingView.ShowErrorScreen();
                doneButton.IsEnabled = true;
            }
        }

        public void OnPopupRemoved()
        {
            Group = null;
            Message = null;
            PopupContainer = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GroupJoinRequestStatusDialog()
        {
            this.InitializeComponent();

            Buttons = new List<Button>();
            doneButton = new Button();
            doneButton.Content = "Done";
            doneButton.Click += (sender, e) =>
            {
                PopupContainer.Dismiss();
            };

            Buttons.Add(doneButton);
        }
    }
}
