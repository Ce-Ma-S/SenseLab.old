using System;
using System.Windows.Threading;

namespace CeMaS.Common
{
    /// <summary>
    /// Helps with UI action dispatching.
    /// </summary>
    public static class UIDispatcherHelper
    {
        /// <summary>
        /// Call this initialization on main UI thread to setup <see cref="Dispatcher"/>.
        /// </summary>
        public static void Init()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        /// <summary>
        /// Main UI dispatcher initialized by <see cref="Init"/>.
        /// </summary>
        public static Dispatcher Dispatcher { get; private set; }

        public static void CallOnDispatcher(this Action method, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            Dispatcher.Invoke(method, priority);
        }
        public static DispatcherOperation CallOnDispatcherAsync(this Action method, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return Dispatcher.InvokeAsync(method, priority);
        }
        public static T CallOnDispatcher<T>(this Func<T> method, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return Dispatcher.Invoke(method, priority);
        }
        public static DispatcherOperation<T> CallOnDispatcherAsync<T>(this Func<T> method, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            return Dispatcher.InvokeAsync(method, priority);
        }
    }
}
