using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Utils.Utilities
{
    public class Logger
    {

        public static void Log(string modName, string message)
        {
            Console.WriteLine("[{0}] {1}", modName, message);
        }

        internal static void Log( string message)
        {
            Console.WriteLine("[{0}] {1}", "BS-Utils", message);
        }

    }
}
