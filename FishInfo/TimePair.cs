using System;

namespace FishInfo
{
    internal class TimePair
    {
        internal readonly int StartTime;
        internal readonly int EndTime;

        internal TimePair(int StartTime, int EndTime)
        {
            this.StartTime = StartTime;
            this.EndTime = EndTime;
        }

        public override bool Equals(object obj)
        {
            return obj is TimePair pair &&
                   StartTime == pair.StartTime &&
                   EndTime == pair.EndTime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartTime, EndTime);
        }
    }
}
