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
        private static Dictionary<string, Stopwatch> Stopwatches = new Dictionary<string, Stopwatch>();


        internal static void CreateAndStartStopwatch(string name)
        {
            Stopwatches.Add(name, new Stopwatch());
            Stopwatches[name].Start();
            return;
        }

        internal static void PrintStopwatchCurrentTime(string name)
        {
            Logger.LogDebug($"Stopwatch {name} current time: {Stopwatches[name].Elapsed}");
            return;
        }

        internal static void StopStopwatchAndGetTime(string name)
        {
            Stopwatches[name].Stop();
            Logger.LogDebug($"Stopwatch {name} ended on {Stopwatches[name].Elapsed}");
            Stopwatches.Remove(name);
            return;
        }
    }
}
