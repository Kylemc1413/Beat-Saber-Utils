using System;
using IPALogger = IPA.Logging.Logger;

namespace BS_Utils.Utilities
{
	// TODO: Mark class as internal upon removal of the obsolete method.
	public class Logger
	{
		internal static IPALogger log { get; set; }

		[Obsolete("The logger class will become internal in a future update. Get your own logger from BSIPA instead.")]
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