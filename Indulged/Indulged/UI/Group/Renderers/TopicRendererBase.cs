using Indulged.API.Storage.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Group.Renderers
{
    public class TopicRendererBase : UserControl
    {
        public static readonly DependencyProperty TopicProperty = DependencyProperty.Register(
        "Topic",
        typeof(FlickrTopic),
        typeof(TopicRendererBase),
        new PropertyMetadata(null, OnTopicPropertyChanged));

        public FlickrTopic Topic
        {
            get { return (FlickrTopic)GetValue(TopicProperty); }
            set { SetValue(TopicProperty, value); }
        }

        private static void OnTopicPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (TopicRendererBase)sender;
            target.OnTopicChanged();
        }

        protected virtual void OnTopicChanged()
        {
        }
    }
}
