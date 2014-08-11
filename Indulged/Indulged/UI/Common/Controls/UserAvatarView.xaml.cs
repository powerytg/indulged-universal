using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Common.Controls
{
    public sealed partial class UserAvatarView : UserControl
    {
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(
       "User",
       typeof(FlickrUser),
       typeof(UserAvatarView),
       new PropertyMetadata(null, OnUserPropertyChanged));

        public FlickrUser User
        {
            get { return (FlickrUser)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        private static void OnUserPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (UserAvatarView)sender;
            target.OnUserChanged();
        }

        private void OnUserChanged()
        {
            ImageView.Source = new BitmapImage(new Uri(User.AvatarUrl));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserAvatarView()
        {
            this.InitializeComponent();
        }
    }
}
