using System;

namespace FishInfo
{
    [Flags]
    internal enum Season
    {
        None = 0,
        Spring = 1 << 0,
        Summer = 1 << 1,
        Fall = 1 << 2,
        Winter = 1 << 3,

        All_seasons = Spring | Summer | Fall | Winter
    }
}
