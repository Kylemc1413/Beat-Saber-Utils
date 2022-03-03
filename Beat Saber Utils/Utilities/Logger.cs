using IPALogger = IPA.Logging.Logger;

namespace BS_Utils.Utilities
{
	internal class Logger
	{
		internal static IPALogger log { get; set; }

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