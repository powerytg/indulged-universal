using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Events;
using Indulged.API.Storage.Models;
using Indulged.UI.Common.Controls;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Group.Dialogs
{
    public sealed partial class GroupInfoDialog : UserControl, IModalPopupContent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GroupInfoDialog()
        {
            this.InitializeComponent();

            // Events
            StorageService.Instance.GroupInfoUpdated += OnGroupInfoUpdated;
        }

        private Button joinButton;
        private Button browseButton;
        private Button doneButton;
        private ModalPopup _popupContainer;

        public void ShowAsModal()
        {
            joinButton = new Button();
            joinButton.Style = Application.Current.Resources["MainButtonStyle"] as Style;
            joinButton.Content = "Join";
            joinButton.Click += (sender, e) => {
                JoinGroup();
            };

            browseButton = new Button();
            browseButton.Style = Application.Current.Resources["MainButtonStyle"] as Style;
            browseButton.Content = "Browse";
            browseButton.Click += (sender, e) => {
                BrowseGroup();
            };

            doneButton = new Button();
            doneButton.Style = Application.Current.Resources["MainButtonStyle"] as Style;
            doneButton.Content = "Done";
            doneButton.Click += (sender, e) => {
                _popupContainer.Dismiss();
            };

            if (!_group.IsInfoRetrieved)
            {
                joinButton.IsEnabled = false;
                browseButton.IsEnabled = false;
            }

            _popupContainer = ModalPopup.ShowWithButtons(this, _group.Name, new List<Button> { browseButton, joinButton, doneButton }, false);            
        }

        private FlickrGroup _group;
        public FlickrGroup Group 
        {
            get
            {
                return _group;
            }
            set
            {
                if(_group != value){
                    _group = value;
                }

                if(_group != null){
                    OnGroupChanged();
                }                    
            }
        }

        public void OnPopupRemoved()
        {
            StorageService.Instance.GroupInfoUpdated -= OnGroupInfoUpdated;
        }

        private void OnGroupInfoUpdated(object sender, StorageEventArgs e)
        {
            if (_group == null)
            {
                return;
            }

            if (_group.ResourceId != e.GroupId)
            {
                return;
            }

            joinButton.IsEnabled = true;
            browseButton.IsEnabled = true;

            UpdateDisplayListAndHideLoadingView();
        }

        private async void OnGroupChanged()
        {
            var status = await APIService.Instance.GetGroupInfoAsync(_group.ResourceId);
            if (!status.Success)
            {
                LoadingView.ErrorText = "Cannot retrieve group info: " + status.ErrorMessage;
                LoadingView.ShowErrorScreen();
            }
        }

        private void UpdateDisplayListAndHideLoadingView()
        {
            LoadingView.Visibility = Visibility.Collapsed;
            ContentView.Visibility = Visibility.Visible;

            if (PolKit.PolicyKit.Instance.UseCleanText)
            {
                if (_group.CleanDescription != null)
                {
                    DescriptionLabel.Text = _group.CleanDescription;
                }                
            }
            else
            {
                if (_group.Description != null)
                {
                    DescriptionLabel.Text = _group.Description;
                }                
            }            

            if (_group.ThrottleMode == "none")
            {
                ThrottleIconView.Source = new BitmapImage(new Uri("ms-appx:///Assets/Group/NoThrottleIcon.png"));
                ThrottleDescriptionView.Text = "No upload limit";
            }
            else
            {
                ThrottleIconView.Source = new BitmapImage(new Uri("ms-appx:///Assets/Group/ThrottleIcon.png"));
                ThrottleDescriptionView.Text = "Upload limit: " + _group.ThrottleMaxCount.ToString() + " per " + _group.ThrottleMode;
            }
        }

        private void BrowseGroup()
        {
            _popupContainer.DismissWithActionAsync(() => {
                var frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(GroupPage), _group.ResourceId);
            });
        }

        private void JoinGroup()
        {
            if (Group.Rules != null && Group.Rules.Length > 0)
            {
                var rulesView = new GroupRulesDialog();
                rulesView.Group = Group;
                rulesView.PopupContainer = _popupContainer;

                _popupContainer.ReplaceContentWith("Group Rules", rulesView, rulesView.Buttons);
            }
            else
            {
                if (Group.IsInvitationOnly)
                {
                    var requestView = new GroupJoinRequestDialog();
                    requestView.Group = Group;
                    requestView.PopupContainer = _popupContainer;

                    _popupContainer.ReplaceContentWith("Join Request", requestView, requestView.Buttons);
                }
                else
                {
                    var statusView = new GroupJoinStatusDialog();
                    statusView.Group = Group;
                    statusView.PopupContainer = _popupContainer;

                    _popupContainer.ReplaceContentWith("Join Group", statusView, statusView.Buttons, () =>
                    {
                        statusView.BeginJoinGroup();
                    });
                }
            }
        }
    }
}
