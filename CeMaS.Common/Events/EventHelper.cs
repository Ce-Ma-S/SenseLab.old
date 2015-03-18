using System;
using System.ComponentModel;

namespace CeMaS.Common.Events
{
    public static class EventHelper
    {
        public static void RaiseEvent(this EventHandler handler, object sender, Func<EventArgs> arguments = null)
        {
            if (handler != null)
                handler(sender, arguments == null ? EventArgs.Empty : arguments());
        }
        public static void RaiseEvent<T>(this EventHandler<T> handler, object sender, Func<T> arguments)
            where T : EventArgs
        {
            if (handler != null)
                handler(sender, arguments());
        }
        public static void RaiseEvent(this PropertyChangedEventHandler handler, object sender, string propertyName)
        {
            if (handler != null)
                handler(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
