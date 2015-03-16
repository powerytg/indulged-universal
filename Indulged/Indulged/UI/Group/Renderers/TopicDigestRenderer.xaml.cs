
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

using System;
using Windows.UI.Xaml.Media.Imaging;
using Indulged.API.Utils;

namespace Indulged.UI.Group.Renderers
{
    public sealed partial class TopicDigestRenderer : TopicRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TopicDigestRenderer()
        {
            this.InitializeComponent();
        }

        protected override void OnTopicChanged()
        {
            base.OnTopicChanged();

            if (Topic.Author.AvatarUrl != null){
                IconView.Source = new BitmapImage(new Uri(Topic.Author.AvatarUrl));
            }
            else
            {
                IconView.Source = null;
            }

            string displaySuject = PolKit.PolicyKit.Instance.UseCleanText ? Topic.CleanSubject : Topic.Subject;
            string displayMessage = PolKit.PolicyKit.Instance.UseCleanText ? Topic.CleanMessage : Topic.Message;

            TitleLabel.Text = displaySuject;
            ContentLabel.Text = displayMessage;

            string dateString = Topic.LastReplyDate.ToTimestampString();

            if (dateString.Contains("/")){
                DateLabel.Text = "Last posted  " + dateString;
            }
            else
            {
                DateLabel.Text = "Last posted  " + dateString + " ago";
            }
                
            ViewLabel.Text = Topic.ReplyCount.ToShortString();
        }

    }
}
