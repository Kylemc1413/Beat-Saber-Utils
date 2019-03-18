using System;

namespace BS_Utils.Utilities
{
    public class Logger
    {
        public static void Log(string modName, string message)
        {
            Console.WriteLine("[{0}] {1}", modName, message);
        }

        internal static void Log(string message)
        {
            Console.WriteLine("[{0}] {1}", "BS-Utils", message);
        }
    }
}
