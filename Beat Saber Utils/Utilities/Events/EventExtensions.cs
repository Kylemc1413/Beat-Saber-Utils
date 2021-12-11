using System;
using System.Linq;

namespace BS_Utils.Utilities.Events
{
    public static class EventExtensions
    {
        /// <summary>
        /// Raises an event, wrapping each delegate in a try/catch. 
        /// Exceptions thrown are logged, using <paramref name="eventName"/> to provide the name of the event the exception was thrown from.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="eventName"></param>
        public static void RaiseEventSafe(this EventHandler e, object sender, string eventName)
        {
            if (e == null) return;
            EventHandler[] handlers = e.GetInvocationList().Select(d => (EventHandler)d).ToArray()
                ?? Array.Empty<EventHandler>();
            for (int i = 0; i < handlers.Length; i++)
            {
                try
                {
                    handlers[i].Invoke(sender, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    Logger.log?.Error($"Error in '{eventName}' handlers '{handlers[i]?.Method.Name}': {ex.Message}");
                    Logger.log?.Debug(ex);
                }
            }
        }

        /// <summary>
        /// Raises an event, wrapping each delegate in a try/catch. 
        /// Exceptions thrown are logged, using <paramref name="eventName"/> to provide the name of the event the exception was thrown from.
        /// </summary>
        /// <typeparam name="TArg"></typeparam>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="eventName"></param>
        public static void RaiseEventSafe<TArg>(this EventHandler<TArg> e, object sender, TArg args, string eventName)
        {
            if (e == null) return;
            EventHandler<TArg>[] handlers = e.GetInvocationList().Select(d => (EventHandler<TArg>)d).ToArray()
                ?? Array.Empty<EventHandler<TArg>>();
            for (int i = 0; i < handlers.Length; i++)
            {
                try
                {
                    handlers[i].Invoke(sender, args);
                }
                catch (Exception ex)
                {
                    Logger.log?.Error($"Error in '{eventName}' handlers '{handlers[i]?.Method.Name}': {ex.Message}");
                    Logger.log?.Debug(ex);
                }
            }
        }
    }
}
