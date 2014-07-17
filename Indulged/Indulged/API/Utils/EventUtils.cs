using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Indulged.API.Utils
{
    public static class EventUtils
    {
        public static void DispatchEvent<T>(this EventHandler<T> eventHandler, object sender, T args)
        {
            if (eventHandler != null)
            {
                eventHandler(sender, args);
            }
        }

        public static async void DispatchEventOnMainThread<T>(this EventHandler<T> eventHandler, object sender, T args)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(new CoreDispatcherPriority(), () =>
            {
                if (eventHandler != null)
                {
                    eventHandler(sender, args);
                }
            });
        }
    }
}
