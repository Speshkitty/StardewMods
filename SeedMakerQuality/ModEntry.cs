using HarmonyLib;
using StardewModdingAPI;
using StardewValley.GameData.Machines;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewValley.GameData.Crops;
using SObject = StardewValley.Object;
using System;
using System.Collections.Generic;
using GenericModConfigMenu;

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

            //Set up GenericModConfigMenu integration
            helper.Events.GameLoop.GameLaunched += (s, e) =>
            {
                var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
                if (configMenu is null)
                    return;

                configMenu.Register(
                    mod: ModManifest,
                    reset: () => Config = new ModConfig(),
                    save: () => Helper.WriteConfig(Config)
                    );

                CreateConfigSection("normal", configMenu, 0);
                CreateConfigSection("silver", configMenu, 1);
                CreateConfigSection("gold", configMenu, 2);
                CreateConfigSection("iridium", configMenu, 4);

            };

            //harmony.PatchAll();
            harmony.Patch(typeof(SObject).GetMethod("OutputSeedMaker"),
                postfix: new HarmonyMethod(typeof(Patch), "Postfix"));
        }

        private void CreateConfigSection(string header, IGenericModConfigMenuApi configMenu, int quality)
        {
            configMenu.AddSectionTitle(mod: ModManifest,
                text: () => Helper.Translation.Get($"title.quality.{header}")
                );

            configMenu.AddNumberOption(mod: ModManifest,
                getValue: () => Config!.GetAmountForQuality(quality).MinAmount,
                setValue: value => Config!.GetAmountForQuality(quality).MinAmount = value,
                name: () => Helper.Translation.Get("option.amount.minimum")
                );

            configMenu.AddNumberOption(
                    mod: ModManifest,
                    getValue: () => Config!.GetAmountForQuality(quality).MaxAmount,
                    setValue: value => Config!.GetAmountForQuality(quality).MaxAmount = value,
                    name: () => Helper.Translation.Get("option.amount.maximum")
                    );

            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => Config!.GetAmountForQuality(quality).AllowMixed,
                setValue: value => Config!.GetAmountForQuality(quality).AllowMixed = value,
                name: () => Helper.Translation.Get("option.canmake.mixed")
                );

            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => Config!.GetAmountForQuality(quality).AllowAncient,
                setValue: value => Config!.GetAmountForQuality(quality).AllowAncient = value,
                name: () => Helper.Translation.Get("option.canmake.ancient"),
                tooltip: () => Helper.Translation.Get("option.canmake.ancient.tooltip")
                );
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