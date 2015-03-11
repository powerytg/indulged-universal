using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Indulged.UI.Common.Controls
{
    public class LoadingViewBase : UserControl
    {
        /// <summary>
        /// Title property
        /// </summary>
        public static readonly DependencyProperty LoadingTextProperty = DependencyProperty.Register(
        "LoadingText",
        typeof(string),
        typeof(LoadingViewBase),
        new PropertyMetadata("Loading", OnLoadingTextPropertyChanged));

        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }

        private static void OnLoadingTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (LoadingViewBase)sender;
            target.OnLoadingTextChanged();
        }

        protected virtual void OnLoadingTextChanged()
        {
        }

        /// <summary>
        /// Error text property
        /// </summary>
        public static readonly DependencyProperty ErrorTextProperty = DependencyProperty.Register(
        "ErrorText",
        typeof(string),
        typeof(LoadingViewBase),
        new PropertyMetadata("An error has occurred during the operation", OnErrorTextPropertyChanged));

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        private static void OnErrorTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (LoadingViewBase)sender;
            target.OnErrorTextChanged();
        }

        protected virtual void OnErrorTextChanged()
        {
        }

        /// <summary>
        /// Text and spinner foreground
        /// </summary>
        public static readonly DependencyProperty ThemeColorProperty = DependencyProperty.Register(
        "ThemeColor",
        typeof(Brush),
        typeof(LoadingViewBase),
        new PropertyMetadata(new SolidColorBrush(Colors.White), OnThemeColorPropertyChanged));

        public Brush ThemeColor
        {
            get { return (Brush)GetValue(ThemeColorProperty); }
            set { SetValue(ThemeColorProperty, value); }
        }

        private static void OnThemeColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (LoadingViewBase)sender;
            target.OnThemeColorChanged();
        }

        protected virtual void OnThemeColorChanged()
        {
        }
    }
}
