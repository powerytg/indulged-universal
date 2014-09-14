using Indulged.UI.Dashboard.Models;
using Indulged.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Dashboard
{
    public class CatalogAlbumSelector : DataTemplateSelector
    {
        public DataTemplate AlbumRenderer { get; set; }
        public DataTemplate AlbumSectionHeaderRenderer { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item.GetType().Equals(typeof(DashboardAlbumHeader)))
            {
                return AlbumSectionHeaderRenderer;
            }
            else
            {
                return AlbumRenderer;
            }
        }
    }
}
