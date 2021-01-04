using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishInfo
{
    static class Extensions
    {
        public static string AsGameText(this string text)
        {
            return Game1.parseText(text, Game1.smallFont, 256);
        }
    }
}
