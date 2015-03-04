using Indulged.UI.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.PhotoStream
{
    public class PhotoTileRendererSelector : DataTemplateSelector
    {
        public DataTemplate MagazinePhotoRenderer { get; set; }
        public DataTemplate JournalPhotoRenderer { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var tile = item as PhotoTile;
            switch (tile.Style)
            {
                case PhotoTile.LayoutStyle.Journal:
                    return JournalPhotoRenderer;
                case PhotoTile.LayoutStyle.Magazine:
                    return MagazinePhotoRenderer;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
