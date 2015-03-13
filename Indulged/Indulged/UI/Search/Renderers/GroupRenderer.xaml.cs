using Indulged.API.Storage;
using Indulged.API.Utils;
using Indulged.UI.Common.GroupStream;
using Indulged.UI.Group;
using Indulged.UI.Group.Dialogs;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Search.Renderers
{
    public sealed partial class GroupRenderer : GroupRendererBase
    {


        /// <summary>
        /// Constructor
        /// </summary>
        public GroupRenderer()
        {
            this.InitializeComponent();
        }

        protected override void OnGroupChanged()
        {
            base.OnGroupChanged();

            if (Group == null)
            {
                return;
            }

            // Avatar
            if (Group.AvatarUrl != null){
                IconView.Source = new BitmapImage(new Uri(Group.AvatarUrl));
            }
            else
            {
                IconView.Source = null;
            }

            TitleLabel.Text = Group.Name;
            string desc = "";
            if (Group.MemberCount <= 1){
                desc = Group.MemberCount.ToShortString() + " member, ";
            }
            else
            {
                desc = Group.MemberCount.ToShortString() + " member, ";
            }
                
            if (Group.PhotoStream.PhotoCount <= 1){
                desc += Group.PhotoStream.PhotoCount.ToShortString() + " photo, ";
            }
            else
            {
                desc += Group.PhotoStream.PhotoCount.ToShortString() + " photos, ";
            }                

            if (Group.TopicCount <= 1){
                desc += Group.TopicCount.ToShortString() + " topic";
            }
            else
            {
                desc += Group.TopicCount.ToShortString() + " topics";
            }
                
            DescriptionLabel.Text = desc;
        }

        private void GroupRendererBase_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // Is member of the group? If so, go to the group page directly; Otherwise show the info page
            var currentUser = StorageService.Instance.CurrentUser;
            if (currentUser.GroupIds.Contains(Group.ResourceId))
            {
                var frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(GroupPage), Group.ResourceId);
            }
            else
            {
                var infoView = new GroupInfoDialog();
                infoView.Group = Group;
                infoView.ShowAsModal();
            }
        }

    }
}
