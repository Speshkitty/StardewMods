using HarmonyLib;
using StardewModdingAPI;
using StardewValley.GameData.Machines;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.GameData.Crops;
using SObject = StardewValley.Object;
using System;
using System.Collections.Generic;

namespace SeedMakerQuality
{
    public class ModEntry : Mod
    {
        internal static ModConfig? Config { get; private set; }
        internal new static IMonitor? Monitor { get; private set; }

        public override void Entry(IModHelper helper)
        {
            Monitor = base.Monitor;
            Harmony harmony = new (ModManifest.Name);
            Config = LoadConfig();

            //harmony.PatchAll();
            harmony.Patch(typeof(SObject).GetMethod("OutputSeedMaker"),
                postfix: new HarmonyMethod(typeof(Patch), "Postfix"));
        }
        private ModConfig LoadConfig()
        {
            ModConfig ReadConfig = Helper.ReadConfig<ModConfig>();

            if (ReadConfig.Equals(new ModConfig()))
            {
                Helper.WriteConfig(ReadConfig);
            }

            return ReadConfig;
        }
    }

    [HarmonyPatch(typeof(SObject), nameof(SObject.OutputSeedMaker))]
    public class Patch
    {
        public static void Postfix(ref Item __result, SObject machine, Item inputItem, bool probe, MachineItemOutput outputData, int? overrideMinutesUntilReady)
        {

            if(__result == null) { return; }
            var configData = ModEntry.Config!.GetAmountForQuality(inputItem.Quality);

            string SeedItem = "";
            Vector2 value = machine.TileLocation;
            Random random = Utility.CreateDaySaveRandom(value.X, value.Y * 77f, Game1.timeOfDay);

            foreach (KeyValuePair<string, CropData> cropDatum in Game1.cropData)
            {
                if (ItemRegistry.HasItemId(inputItem, cropDatum.Value.HarvestItemId))
                {
                    SeedItem = cropDatum.Key;
                    break;
                }
            }

            if(string.IsNullOrWhiteSpace(SeedItem)) { return; }

            if (__result.Name == "Mixed Seeds" && !configData.AllowMixed)
            {
                __result = new SObject(SeedItem, random.Next(configData.MinAmount, configData.MaxAmount));
            }
            else if(__result.Name == "Ancient Seeds" && !configData.AllowAncient && inputItem.Name != "Ancient Fruit")
            {
                __result = new SObject(SeedItem, random.Next(configData.MinAmount, configData.MaxAmount));
            }
            else
            {
                __result.Stack = random.Next(configData.MinAmount, configData.MaxAmount);
            }
        }
    }
}