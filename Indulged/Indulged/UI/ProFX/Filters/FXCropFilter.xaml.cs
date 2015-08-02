using Lumia.Imaging;
using Lumia.Imaging.Transforms;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.ProFX.Filters
{
    public sealed partial class FXCropFilter : FilterBase
    {
        // Crop area
        private Windows.Foundation.Rect cropRect = new Windows.Foundation.Rect(0, 0, 0, 0);

        // Constructor
        public FXCropFilter()
        {
            InitializeComponent();
            Category = FilterCategory.Transform;

            DisplayName = "crop";
        }

        public override bool hasEditorUI
        {
            get
            {
                return false;
            }
        }

        public override IFilter FinalOutputFilter
        {
            get
            {
                double xFactor = OriginalImageWidth / CurrentImage.PixelWidth;
                double yFactor = OriginalImageHeight / CurrentImage.PixelHeight;

                Windows.Foundation.Rect finalCropRect = new Windows.Foundation.Rect(cropRect.X * xFactor, cropRect.Y * yFactor, cropRect.Width * xFactor, cropRect.Height * yFactor);

                IFilter finalCropFilter = new CropFilter(finalCropRect);
                return finalCropFilter;
            }
        }

        public override void CreateFilter()
        {
            if (cropRect.Width == 0 || cropRect.Height == 0)
            {
                cropRect.Width = OriginalPreviewImageWidth / 2;
                cropRect.Height = OriginalPreviewImageHeight / 2;

            }

            Filter = new CropFilter(cropRect);
        }

        override protected void DeleteFilter()
        {
            cropRect = new Windows.Foundation.Rect(0, 0, 0, 0);
            base.DeleteFilter();
        }

        public void UpdateCropRect(double x, double y, double w, double h)
        {
            // Translate coordinations
            cropRect.X = x;
            cropRect.Y = y;
            cropRect.Width = w;
            cropRect.Height = h;
        }

    }
}
