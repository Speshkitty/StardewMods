using System;

namespace FishInfo
{
    [Flags]
    internal enum Weather
    {
        None = 0,
        Sun = 1 << 0,
        Rain = 1 << 1
    }
}
