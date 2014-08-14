using Indulged.API.Networking.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indulged.API.Networking
{
    public partial class APIService
    {
        public EventHandler<APIEventArgs> AlbumListReturned;
        public EventHandler<APIEventArgs> AlbumListFailedReturn;
    }
}
