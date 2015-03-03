using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.GroupStream
{
    public partial class GroupRendererBase : UserControl
    {
        public static readonly DependencyProperty GroupProperty = DependencyProperty.Register(
        "Group",
        typeof(FlickrGroup),
        typeof(GroupRendererBase),
        new PropertyMetadata(null, OnGroupPropertyChanged));

        public FlickrGroup Group
        {
            get { return (FlickrGroup)GetValue(GroupProperty); }
            set { SetValue(GroupProperty, value); }
        }

        protected static void OnGroupPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (GroupRendererBase)sender;
            target.OnGroupChanged();
        }

        protected virtual void OnGroupChanged()
        {
            // Subclass should override this method
        }
    }
}
