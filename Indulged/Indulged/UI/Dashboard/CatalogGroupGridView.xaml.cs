using Indulged.API.Storage.Models;
using Indulged.UI.Common.GroupStream;
using Indulged.UI.Group;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Dashboard
{
    public sealed partial class CatalogGroupGridView : UserControl
    {
        private static int MAX_DISPLAY_COUNT = 6;

        private List<FlickrGroup> groupList;
        public List<FlickrGroup> GroupList 
        {
            set
            {
                groupList = value;
                UpdateDisplayList();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CatalogGroupGridView()
        {
            this.InitializeComponent();
        }

        private void UpdateDisplayList()
        {
            if(groupList.Count == 0)
            {
                EmptyGroupLabel.Visibility = Visibility.Visible;
                GroupContainer.Visibility = Visibility.Collapsed;
                ViewAllButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                EmptyGroupLabel.Visibility = Visibility.Collapsed;
                GroupContainer.Visibility = Visibility.Visible;
                ViewAllButton.Visibility = Visibility.Visible;

                GroupContainer.Children.Clear();
                int displayCount = Math.Min(MAX_DISPLAY_COUNT, groupList.Count);
                
                for(var i = 0; i < displayCount; i++)
                {
                    var group = groupList[i];
                    var renderer = new GroupButtonRenderer();
                    renderer.Group = group;

                    GroupContainer.Children.Add(renderer);
                }
            }
        }

        private void ViewAllButton_Click(object sender, RoutedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(GroupListPage));
        }

    }
}
