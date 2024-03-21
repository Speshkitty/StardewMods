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

    internal static class SeasonExt
    {
        internal static Season ToFishSeason(this StardewValley.Season season)
        {
            switch (season)
            {
                case StardewValley.Season.Spring: return Season.Spring;
                case StardewValley.Season.Summer: return Season.Summer;
                case StardewValley.Season.Fall: return Season.Fall;
                case StardewValley.Season.Winter: return Season.Winter;
                default: return Season.None;
            }
        }
    }
}
