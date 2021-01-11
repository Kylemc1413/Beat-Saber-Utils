using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Utils.Utilities.Events
{
    public static class EventExtensions
    {
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
