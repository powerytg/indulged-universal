using Indulged.API.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.UI.Dashboard.Events
{
    public class StreamSelectionChangedEventArgs : EventArgs
    {
        public FlickrPhotoStream SelectedStream { get; set; }
    }
}
