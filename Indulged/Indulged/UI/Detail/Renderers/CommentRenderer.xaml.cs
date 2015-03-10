using Indulged.API.Utils;
using System;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Detail.Renderers
{
    public sealed partial class CommentRenderer : CommentRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommentRenderer()
        {
            this.InitializeComponent();
        }

        protected override void OnCommentChanged()
        {
            base.OnCommentChanged();

            if (Comment == null)
            {
                return;
            }

            BodyTextView.Text = Comment.Message;
            AvatarView.Source = new BitmapImage(new Uri(Comment.Author.AvatarUrl));
            StatusLabel.Text = Comment.Author.Name + " · " + Comment.CreationDate.ToTimestampString();
        }

    }
}
