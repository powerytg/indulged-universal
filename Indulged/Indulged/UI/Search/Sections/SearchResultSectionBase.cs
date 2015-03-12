using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Indulged.UI.Search.Sections
{
    public class SearchResultSectionBase : UserControl
    {
        public string QueryType { get; set; }

        public static readonly DependencyProperty KeywordProperty = DependencyProperty.Register(
        "Keyword",
        typeof(string),
        typeof(SearchResultSectionBase),
        new PropertyMetadata(null, OnKeywordPropertyChanged));

        public string Keyword
        {
            get { return (string)GetValue(KeywordProperty); }
            set { SetValue(KeywordProperty, value); }
        }

        private static void OnKeywordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (SearchResultSectionBase)sender;
            target.OnKeywordChanged();
        }

        protected virtual void OnKeywordChanged()
        {
        }

        public virtual void AddEventListeners()
        {

        }

        public virtual void RemoveEventListeners()
        {

        }
    }
}
