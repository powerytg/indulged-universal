using Indulged.API.Networking;
using Indulged.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Search.Sections
{
    public sealed partial class GroupResultSection : SearchResultSectionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GroupResultSection()
        {
            this.InitializeComponent();
        }

        protected override async void OnKeywordChanged()
        {
            base.OnKeywordChanged();

            var ds = new GroupCollection();
            ds.Query = Keyword;
            GroupListView.ItemsSource = ds;

            // Load first page
            await ds.LoadMoreItemsAsync((uint)APIService.PerPage);
        }
    }
}
