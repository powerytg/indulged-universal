using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.Controls
{
    public sealed partial class SectionControl : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SectionControl), new PropertyMetadata(null, OnTitlePropertyChanged));

        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static void OnTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((SectionControl)sender).OnTitleChanged();
        }

        private void OnTitleChanged()
        {
            Label.Text = Title;
        }

        // Constructor
        public SectionControl()
        {
            InitializeComponent();
        }
    }
}
