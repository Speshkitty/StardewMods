using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishInfo
{
    static class StopwatchCollection
    {
#if DEBUG
        private static Dictionary<string, Stopwatch> Stopwatches = new Dictionary<string, Stopwatch>();
#endif

        internal static void CreateAndStartStopwatch(string name)
        {
#if DEBUG
            Stopwatches.Add(name, new Stopwatch());
            Stopwatches[name].Start();
#endif
        }

        internal static void PrintStopwatchCurrentTime(string name)
        {
#if DEBUG
            Logger.LogDebug($"Stopwatch {name} current time: {Stopwatches[name].Elapsed}");
#endif
        }

        internal static void StopStopwatchAndGetTime(string name)
        {
#if DEBUG
            Stopwatches[name].Stop();
            Logger.LogDebug($"Stopwatch {name} ended on {Stopwatches[name].Elapsed}");
            Stopwatches.Remove(name);
#endif
        }
    }
}
