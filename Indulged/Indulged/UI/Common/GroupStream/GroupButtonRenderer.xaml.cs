using Indulged.UI.Group;
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

namespace Indulged.UI.Common.GroupStream
{
    public sealed partial class GroupButtonRenderer : GroupRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GroupButtonRenderer()
        {
            this.InitializeComponent();
        }

        protected override void OnGroupChanged()
        {
            base.OnGroupChanged();

            if (Group != null)
            {
                TitleLabel.Text = Group.Name;
                AlertIcon.Visibility = Group.IsEighteenPlus ? Visibility.Visible : Visibility.Collapsed;
                AdminIcon.Visibility = Group.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
                PrivateIcon.Visibility = Group.IsInvitationOnly ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(GroupPage), Group.ResourceId);
        }

        

    }
}
