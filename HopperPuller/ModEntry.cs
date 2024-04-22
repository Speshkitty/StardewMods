using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.BigCraftables;
using StardewValley.Objects;
using System.Reflection;
using System.Reflection.PortableExecutable;
using SObject = StardewValley.Object;

namespace HopperPuller
{
    public class ModEntry : Mod
    {
        new static IMonitor? Monitor;
        public override void Entry(IModHelper helper)
        {
            Monitor = base.Monitor;
            Harmony harmony = new Harmony(ModManifest.UniqueID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(SObject), nameof(SObject.onReadyForHarvest))]
        public static class PatchOnReadyForHarvest
        {
            public static void Postfix(SObject __instance)
            {
                GameLocation position = __instance.Location;
                Vector2 tile = __instance.TileLocation;
                SObject? belowItem = position.getObjectAtTile((int)tile.X, (int)tile.Y + 1);

                if (belowItem != null && belowItem is Chest belowChest && belowChest.SpecialChestType == Chest.SpecialChestTypes.AutoLoader) //hopper below this machine
                {
                    __instance.heldObject.Value = (SObject)belowChest.addItem(__instance.heldObject.Value);
                    __instance.readyForHarvest.Value = false;
                    if (__instance is Cask cask)
                    {
                        cask.MinutesUntilReady = -1;
                        cask.agingRate.Value = 0;
                        cask.daysToMature.Value = 0;
                    }
                    __instance.AttemptAutoLoad(new Farmer());

                    if(position.getObjectAtTile((int)belowChest.TileLocation.X, (int)belowChest.TileLocation.Y + 1) is SObject machine) //pass it down the chain
                    {
                        machine.AttemptAutoLoad(new Farmer());
                    }
                    //__instance.checkForAction(new Farmer());
                }
            }
        }

    }

   
}