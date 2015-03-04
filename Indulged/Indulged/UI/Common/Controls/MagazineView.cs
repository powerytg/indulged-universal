using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Common.Controls
{
    public class MagazineView : GridView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element,
        object item)
        {
            try
            {
                dynamic localItem = item;
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, localItem.RowSpan);
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, localItem.ColSpan);
            }
            catch
            {
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, 1);
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 1);
            }

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}
