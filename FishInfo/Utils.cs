using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishInfo
{
    internal static class Utils
    {
        internal static Season StringToSeason(string str)
        {
            switch (str.ToLower())
            {
                case "spring":
                    return Season.Spring;
                case "summer":
                    return Season.Summer;
                case "autumn":
                case "fall":
                    return Season.Fall;
                case "winter":
                    return Season.Winter;
                case "all":
                    return Season.All_seasons;
                default: 
                    return Season.None;
            }
        }

        internal static Weather StringToWeather(string str)
        {
            switch (str.ToLower())
            {
                case "sunny":
                    return Weather.Sun;
                case "rainy":
                    return Weather.Rain;
                case "both":
                    return Weather.Rain | Weather.Sun;
                default:
                    return Weather.None;
            }
        }
    }
}
