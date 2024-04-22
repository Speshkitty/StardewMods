using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using System.Reflection;
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
                Chest? belowChest;
                if (belowItem != null && belowItem is Chest && (belowChest = (belowItem as Chest))!.SpecialChestType == Chest.SpecialChestTypes.AutoLoader)
                {
                    __instance.heldObject.Value = (SObject)belowChest.addItem(__instance.heldObject.Value);
                }
            }
        }

    }

   
}