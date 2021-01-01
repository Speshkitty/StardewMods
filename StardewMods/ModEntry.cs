using StardewValley;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewModTemplate
{
    public class ModEntry : Mod
    {
        internal static new IMonitor Monitor;
        internal static new IModHelper Helper;

        public override void Entry(IModHelper helper)
        {
            Monitor = base.Monitor;
            Helper = base.Helper;

            throw new NotImplementedException();
        }
    }
}
