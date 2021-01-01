using StardewValley;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomiseChildBedroom
{
    class Commands
    {

        IModHelper Helper;
        public Commands(IModHelper helper)
        {
            Helper = helper;

            MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (MethodInfo method in methods)
            {
                string MethodName;
                string MethodDescription;

                try
                {
                    MethodName = method.GetCustomAttribute<CommandName>().Name;
                }
                catch
                {
                    continue;
                }
                MethodDescription = Translation.GetString($"command.{MethodName}.description", new { CommandName = MethodName });

                Helper.ConsoleCommands.Add(MethodName, MethodDescription, (Action<string, string[]>)method.CreateDelegate(typeof(Action<string, string[]>), this));

            }
        }

        [CommandName("togglecrib")]
        internal void ToggleCrib(string command, string[] args)
        {
            string MethodName = MethodBase.GetCurrentMethod().GetCustomAttribute<CommandName>().Name;

            if (!Game1.IsMasterGame) // We aren't the host
            {
                ModEntry.LogError(Translation.GetString("command.failed.nothost"));
                return;
            }

            //handling spaces in names
            //options: 
            //replace spaces with other character - not ideal
            //l2handle them

            //We need to verify we're the host, if not fail

            if (args.Length == 0 || args.Length > 1) //no player name given
            {
                CallHelp(MethodName);
            }
            else
            {
                string FarmerName = args[0];
                Farmer Who = ModEntry.Config.GetCurrentFarm().GetFarmer(FarmerName);
                if (Who is null)
                {
                    ModEntry.LogError(Translation.GetString("error.farmernamenotfound", new { FarmerName }));
                    return;
                }

                Who.ToggleCrib();

                //We need to also send this to other games if this is multiplayer
                Multiplayer.TogglePacket packet = new Multiplayer.TogglePacket(FarmerName, Bed.CRIB);
                Helper.Multiplayer.SendMessage(packet, Multiplayer.TogglePacket.StringType, new[] { Helper.ModRegistry.ModID });

                ModEntry.Log(Translation.GetString("command.togglecrib.toggledo" + (Who.ShowCrib ? "n" : "ff"), new { FarmerName }));
                ModEntry.Config.Save();
            }
        }

        [CommandName("togglebed")]
        internal void ToggleBed(string command, string[] args)
        {
            string MethodName = MethodBase.GetCurrentMethod().GetCustomAttribute<CommandName>().Name;

            if (!Game1.IsMasterGame) // We aren't the host
            {
                ModEntry.LogError(Translation.GetString("command.failed.nothost"));
                return;
            }

            if (args.Length == 0 || args.Length > 2)
            {
                CallHelp(MethodName);
            }
            else
            {
                string FarmerName = args[0];
                Farmer Who = ModEntry.Config.GetCurrentFarm().GetFarmer(FarmerName);
                if (Who is null)
                {
                    ModEntry.LogError(Translation.GetString("error.farmernamenotfound", new { FarmerName }));
                    return;
                }

                bool DoLeft = false;
                bool DoRight = false;

                if (args.Length == 1)
                {
                    DoLeft = true;
                    DoRight = true;
                }
                else
                {
                    if (args[1].Equals("left", StringComparison.OrdinalIgnoreCase))
                    {
                        DoLeft = true;
                    }
                    else if (args[1].Equals("right", StringComparison.OrdinalIgnoreCase))
                    {
                        DoRight = true;
                    }
                    else
                    {
                        CallHelp(MethodName);
                        return;
                    }
                }
                Bed bedData = 0;
                if (DoLeft)
                {
                    Who.ToggleLeftBed();
                    bedData |= Bed.LEFT;
                    ModEntry.Log(Translation.GetString("command.togglebed.left.toggledo" + (Who.ShowLeftBed ? "n" : "ff"), new { FarmerName }));

                }
                if (DoRight)
                {
                    Who.ToggleRightBed();
                    bedData |= Bed.RIGHT;
                    ModEntry.Log(Translation.GetString("command.togglebed.right.toggledo" + (Who.ShowRightBed ? "n" : "ff"), new { FarmerName }));
                }

                if (DoLeft || DoRight)
                {
                    Multiplayer.TogglePacket packet = new Multiplayer.TogglePacket(FarmerName, bedData);
                    Helper.Multiplayer.SendMessage(packet, Multiplayer.TogglePacket.StringType, new[] { Helper.ModRegistry.ModID });
                    ModEntry.Config.Save();
                }
            }

        }

        private void CallHelp(params string[] CommandName) => Helper.ConsoleCommands.Trigger("help", CommandName);


    }
}
