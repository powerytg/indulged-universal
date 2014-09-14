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

namespace Indulged.UI.Dashboard
{
    public sealed partial class StreamSectionView : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        "Title",
        typeof(string),
        typeof(StreamSectionView),
        new PropertyMetadata(null, OTitlePropertyChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (StreamSectionView)sender;
            target.OnAlbumChanged();
        }

        private void OnAlbumChanged()
        {
            TitleLabel.Text = Title;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StreamSectionView()
        {
            this.InitializeComponent();
        }
    }
}
