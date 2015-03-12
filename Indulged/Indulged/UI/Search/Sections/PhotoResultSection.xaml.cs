using Indulged.API.Storage.Models;
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
    public sealed partial class PhotoResultSection : SearchResultSectionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PhotoResultSection()
        {
            this.InitializeComponent();
        }

        protected override void OnKeywordChanged()
        {
            base.OnKeywordChanged();

            var stream = new FlickrPhotoStream(FlickrPhotoStreamType.SearchStream);
            stream.QueryType = QueryType;
            stream.Query = Keyword;

            StreamListView.Stream = stream;
        }
    }
}
