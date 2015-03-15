using Indulged.API.Storage.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Group.Sections
{
    public class GroupSectionBase : UserControl
    {
        public static readonly DependencyProperty GroupProperty = DependencyProperty.Register(
        "Group",
        typeof(FlickrGroup),
        typeof(GroupSectionBase),
        new PropertyMetadata(null, OnGroupPropertyChanged));

        public FlickrGroup Group
        {
            get { return (FlickrGroup)GetValue(GroupProperty); }
            set { SetValue(GroupProperty, value); }
        }

        private static void OnGroupPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (GroupSectionBase)sender;
            target.OnGroupChanged();
        }

        protected virtual void OnGroupChanged()
        {
        }

        public virtual void AddEventListeners()
        {

        }

        public virtual void RemoveEventListeners()
        {

        }
    }
}
