using Lumia.Imaging.Artistic;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.Filters
{
    public sealed partial class FXAntiqueFilter : FilterBase
    {
        public FXAntiqueFilter()
        {
            InitializeComponent();

            DisplayName = "antique";
            StatusBarName = "Antique";
            Category = FilterCategory.Effect;
        }

        public override bool hasEditorUI
        {
            get
            {
                return false;
            }
        }

        public override void CreateFilter()
        {
            Filter = new AntiqueFilter();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteFilterAsync();
        }

    }
}
