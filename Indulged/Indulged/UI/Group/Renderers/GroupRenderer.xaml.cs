using Indulged.API.Utils;
using Indulged.UI.Common.GroupStream;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Group.Renderers
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

            TitleLabel.Text = Group.Name;
            AlertIcon.Visibility = Group.IsEighteenPlus ? Visibility.Visible : Visibility.Collapsed;
            AdminIcon.Visibility = Group.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            PrivateIcon.Visibility = Group.IsInvitationOnly ? Visibility.Visible : Visibility.Collapsed;

            // Description text
            string desc = "";
            if (Group.MemberCount <= 1)
            {
                desc = Group.MemberCount.ToShortString() + " member, ";
            }
            else
            {
                desc = Group.MemberCount.ToShortString() + " member, ";
            }

            if (Group.PhotoStream.PhotoCount <= 1)
            {
                desc += Group.PhotoStream.PhotoCount.ToShortString() + " photo, ";
            }
            else
            {
                desc += Group.PhotoStream.PhotoCount.ToShortString() + " photos, ";
            }

            if (Group.TopicCount <= 1)
            {
                desc += Group.TopicCount.ToShortString() + " topic";
            }
            else
            {
                desc += Group.TopicCount.ToShortString() + " topics";
            }

            DescriptionLabel.Text = desc;
        }

        private void GroupRendererBase_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Group != null)
            {
                var frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(GroupPage), Group.ResourceId);
            }
        }
    }
}
