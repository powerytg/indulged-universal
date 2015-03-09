using Indulged.API.Storage.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Detail.Renderers
{
    public class CommentRendererBase : UserControl
    {
        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register(
        "Comment",
        typeof(FlickrComment),
        typeof(CommentRendererBase),
        new PropertyMetadata(null, OnCommentPropertyChanged));

        public FlickrComment Comment
        {
            get { return (FlickrComment)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }

        private static void OnCommentPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CommentRendererBase)sender;
            target.OnCommentChanged();
        }

        protected virtual void OnCommentChanged()
        {
        }
    }
}
