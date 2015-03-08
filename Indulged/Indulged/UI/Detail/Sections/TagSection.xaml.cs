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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Detail.Sections
{
    public sealed partial class TagSection : DetailSectionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TagSection()
        {
            this.InitializeComponent();
        }

        protected override void OnPhotoChanged()
        {
            base.OnPhotoChanged();

            if (Photo == null)
            {
                return;
            }

            if (Photo.Tags.Count == 0)
            {
                NoTagsView.Visibility = Visibility.Visible;
                NoTagsView.Text = "This photo does not have tags";
            }
            else
            {
                NoTagsView.Visibility = Visibility.Collapsed;
                TagListView.Children.Clear();

                foreach (var tag in Photo.Tags)
                {
                    var tagButton = new Button();
                    tagButton.Style = Application.Current.Resources["TagButtonStyle"] as Style;
                    tagButton.Content = tag;
                    tagButton.Margin = new Thickness(4, 0, 4, 4);
                    TagListView.Children.Add(tagButton);
                }

                TagListView.Visibility = Visibility.Visible;
            }
        }

    }
}
