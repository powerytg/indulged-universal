using Indulged.UI.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.PhotoStream
{
    public class PhotoTileRendererSelector : DataTemplateSelector
    {
        public DataTemplate MagazineRenderer { get; set; }
        public DataTemplate JournalRenderer { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var tile = item as PhotoTile;
            if (tile.Style == PhotoTile.LayoutStyle.Journal)
            {
                return JournalRenderer;
            }
            else if (tile.Style == PhotoTile.LayoutStyle.Magazine)
            {
                return MagazineRenderer;
            }
            
            return base.SelectTemplateCore(item, container);
        }
    }
}
