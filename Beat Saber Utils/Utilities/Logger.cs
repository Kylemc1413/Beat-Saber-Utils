using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPALogger = IPA.Logging.Logger;
namespace BS_Utils.Utilities
{
    public class Logger
    {

        internal static IPALogger log { get; set; }
        public static void Log(string modName, string message)
        {
            log.Info($"[{modName}]  {message}");
        }

        internal static void Log(string message)
        {
            log.Info($"[BS-Utils]  {message}");
        }
        internal static void Log(string message, IPALogger.Level level)
        {
            log.Log(level, $"[BS-Utils]  {message}");
        }
    }
}
