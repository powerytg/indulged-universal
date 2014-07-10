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

namespace Indulged.UI.Common.Controls
{
    public sealed partial class SimpleLoadingView : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        "Text",
        typeof(string),
        typeof(SimpleLoadingView),
        new PropertyMetadata(null, OnTextPropertyChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (SimpleLoadingView)sender;
            target.OnTextChanged();
        }

        private void OnTextChanged()
        {
            Label.Text = Text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleLoadingView()
        {
            this.InitializeComponent();
        }
    }
}
