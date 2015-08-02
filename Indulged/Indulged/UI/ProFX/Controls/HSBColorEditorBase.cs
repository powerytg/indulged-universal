using Indulged.API.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.ProFX.Controls
{
    public class HSBColorEditorBase : UserControl
    {
        public static readonly DependencyProperty HSBColorSourceProperty = DependencyProperty.Register(
        "HSBColorSource",
        typeof(HSBColor),
        typeof(HSBColorEditorBase),
        new PropertyMetadata(null, OnHSBColorSourcePropertyChanged));

        public HSBColor HSBColorSource
        {
            get { return (HSBColor)GetValue(HSBColorSourceProperty); }
            set { SetValue(HSBColorSourceProperty, value); }
        }

        private static void OnHSBColorSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (HSBColorEditorBase)sender;
            target.OnHSBColorSourceChanged();
        }

        protected virtual void OnHSBColorSourceChanged()
        {

        }
    }
}
