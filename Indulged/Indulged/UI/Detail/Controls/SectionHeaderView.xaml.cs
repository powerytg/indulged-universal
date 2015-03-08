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

namespace Indulged.UI.Detail.Controls
{
    public sealed partial class SectionHeaderView : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        "Title",
        typeof(string),
        typeof(SectionHeaderView),
        new PropertyMetadata(null, OnTitlePropertyChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (SectionHeaderView)sender;
            target.OnTitleChanged();
        }

        private void OnTitleChanged()
        {
            TitleLabel.Text = Title;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SectionHeaderView()
        {
            this.InitializeComponent();
        }
    }
}
