using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Profile.Sections
{
    public class UserSectionBase : UserControl
    {
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(
        "User",
        typeof(FlickrUser),
        typeof(UserSectionBase),
        new PropertyMetadata(null, OnUserPropertyChanged));

        public FlickrUser User
        {
            get { return (FlickrUser)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        private static void OnUserPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (UserSectionBase)sender;
            target.OnUserChanged();
        }

        protected virtual void OnUserChanged()
        {
        }
    }
}
