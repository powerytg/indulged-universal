using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.Controls
{
    public class CommonHeaderBase : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        "Title",
        typeof(string),
        typeof(CommonHeaderBase),
        new PropertyMetadata("", OnTitlePropertyChanged));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CommonHeaderBase)sender;
            target.OnTitleChanged();
        }

        protected virtual void OnTitleChanged()
        {
        }

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(
        "Theme",
        typeof(string),
        typeof(CommonHeaderBase),
        new PropertyMetadata("Light", OnThemePropertyChanged));

        public string Theme
        {
            get { return (string)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        private static void OnThemePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CommonHeaderBase)sender;
            target.OnThemeChanged();
        }

        protected virtual void OnThemeChanged()
        {            
        }
    }
}
