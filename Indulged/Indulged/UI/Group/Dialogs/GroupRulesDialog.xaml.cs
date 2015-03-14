using Indulged.API.Storage.Models;
using Indulged.UI.Common.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Group.Dialogs
{
    public sealed partial class GroupRulesDialog : UserControl
    {
        private FlickrGroup _group;
        public FlickrGroup Group
        {
            get
            {
                return _group;
            }
            set
            {
                if (_group != value)
                {
                    _group = value;
                }

                if (_group != null)
                {
                    OnGroupChanged();
                }
            }
        }

        // Reference to the popup container
        public ModalPopup PopupContainer { get; set; }

        private Button joinButton;
        private Button cancelButton;
        public List<Button> Buttons { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GroupRulesDialog()
        {
            this.InitializeComponent();

            joinButton = new Button();
            joinButton.Style = Application.Current.Resources["MainButtonStyle"] as Style;
            joinButton.Content = "Agree";
            joinButton.Click += (sender, e) =>
            {
                if (Group.IsInvitationOnly)
                {
                    var requestView = new GroupJoinRequestDialog();
                    requestView.Group = Group;
                    requestView.PopupContainer = PopupContainer;

                    PopupContainer.ReplaceContentWith("Join Request", requestView, requestView.Buttons);
                }
                else
                {
                    var statusView = new GroupJoinStatusDialog();
                    statusView.Group = Group;
                    statusView.PopupContainer = PopupContainer;

                    PopupContainer.ReplaceContentWith("Join Group", statusView, statusView.Buttons, () =>
                    {
                        statusView.BeginJoinGroup();
                    });
                }

            };

            cancelButton = new Button();
            cancelButton.Style = Application.Current.Resources["MainButtonStyle"] as Style;
            cancelButton.Content = "Cancel";
            cancelButton.Click += (sender, e) =>
            {
                PopupContainer.Dismiss();
            };

            Buttons = new List<Button>();
            Buttons.Add(joinButton);
            Buttons.Add(cancelButton);
        }

        private void OnGroupChanged()
        {
            if (Group!= null && Group.Rules != null)
            {
                RulesLabel.Text = Group.Rules;
            }            
        }


    }
}
