namespace SlimeWaterFiller
{
    static class Logger
    {
        internal static void LogInfo(object TextToLog)
        {
            ModEntry.Monitor.Log(TextToLog.ToString(), StardewModdingAPI.LogLevel.Info);
        }

        internal static void LogDebug(object TextToLog)
        {
#if DEBUG
            ModEntry.Monitor.Log(TextToLog.ToString(), StardewModdingAPI.LogLevel.Debug);
#endif
        }
    }
}
