using GenericModConfigMenu;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Locations;
using StardewValley.GameData.Objects;
using System;
using System.Collections.Generic;

namespace FishInfo
{
    public class ModEntry : Mod
    {
        internal static Dictionary<string, FishData> FishInfo = new Dictionary<string, FishData>();

        internal static new IModHelper Helper;
        internal static new IMonitor Monitor;

        internal static ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            Helper = helper;
            Monitor = base.Monitor;
            LoadConfig();
            Patches.DoPatches();
            
            Helper.Events.GameLoop.DayStarted += LoadData;

            helper.Events.GameLoop.GameLaunched += (s, e) =>
            {
                var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
                if (configMenu is null)
                    return;

                configMenu.Register(
                    mod: ModManifest,
                    reset: () => Config = new ModConfig(),
                    save: () => Helper.WriteConfig(Config)
                    );

                CreateConfigSection("caught", configMenu, Config.CaughtFishData);
                CreateConfigSection("uncaught", configMenu, Config.UncaughtFishData);
            };
        }

        private void CreateConfigSection(string header, IGenericModConfigMenuApi configMenu, ShowFishData dataBean)
        {
            configMenu.AddSectionTitle(mod: ModManifest,
                text: () => Helper.Translation.Get($"config.header.{header}")
                );

            configMenu.AddBoolOption(mod: ModManifest,
                getValue: () => dataBean.AlwaysShowName,
                setValue: value => dataBean.AlwaysShowName = value,
                name: () => Helper.Translation.Get("option.display.name")
                );

            configMenu.AddBoolOption(
                    mod: ModManifest,
                    getValue: () => dataBean.AlwaysShowLocation,
                    setValue: value => dataBean.AlwaysShowLocation = value,
                    name: () => Helper.Translation.Get("option.display.location")
                    );

            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => dataBean.AlwaysShowSeason,
                setValue: value => dataBean.AlwaysShowSeason = value,
                name: () => Helper.Translation.Get("option.display.season")
                );

            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => dataBean.AlwaysShowTime,
                setValue: value => dataBean.AlwaysShowTime = value,
                name: () => Helper.Translation.Get("option.display.time")
                );

            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => dataBean.AlwaysShowWeather,
                setValue: value => dataBean.AlwaysShowWeather = value,
                name: () => Helper.Translation.Get("option.display.weather")
                );
        }

        internal static FishData GetOrCreateData(string fishID)
        {
            if (FishInfo.TryGetValue(fishID, out FishData data))
            {
                return data;
            }
            else
            {
                FishInfo.Add(fishID, new FishData());
                return FishInfo[fishID];
            }
        }

        private void LoadConfig()
        {
            ModConfig ReadConfig = Helper.ReadConfig<ModConfig>();

            if(ReadConfig.Equals(new ModConfig()))
            {
                Helper.WriteConfig(ReadConfig);
            }

            Config = ReadConfig;
        }

        private void SpecialHandledFish(string id, Weather weather, Tuple<int, int>[] times, Tuple<string, Season>[] locationsWithSeason)
        {
            FishData fishData = GetOrCreateData(id);
            fishData.FishName = new StardewValley.Object(id[3..], 1).DisplayName;
            fishData.AddWeather(weather);
            foreach (var t in times)
            {
                fishData.AddTimes(t.Item1, t.Item2);
            }
            foreach(var l in locationsWithSeason)
            {
                fishData.LocationData.Add(l.Item1, l.Item2);
            }
        }

        public void LoadData(object sender, EventArgs e)
        {
            FishInfo.Clear();

            Dictionary<string, LocationData> GameLocationData = DataLoader.Locations(Game1.content);
            Dictionary<string, string> GameFishData = DataLoader.Fish(Game1.content);
            string locationName;

            SpecialHandledFish("(O)Goby", Weather.Rain | Weather.Sun, new Tuple<int, int>[] { new(600, 2600) }, new Tuple<string, Season>[] { new("forestwaterfall", Season.All_seasons) });
            SpecialHandledFish("(O)158", Weather.Rain | Weather.Sun, new Tuple<int, int>[] { new(600, 2600) }, new Tuple<string, Season>[] { new("mines20", Season.All_seasons) }); //Stonefish
            SpecialHandledFish("(O)161", Weather.Rain | Weather.Sun, new Tuple<int, int>[] { new(600, 2600) }, new Tuple<string, Season>[] { new("mines60", Season.All_seasons) }); //Ice Pip
            SpecialHandledFish("(O)162", Weather.Rain | Weather.Sun, new Tuple<int, int>[] { new(600, 2600) }, new Tuple<string, Season>[] { new("mines100", Season.All_seasons) }); //Lava Eel
            SpecialHandledFish("(O)682", Weather.Rain | Weather.Sun, new Tuple<int, int>[] { new(600, 2600) }, new Tuple<string, Season>[] { new("Sewer", Season.All_seasons) }); //Mutant Carp

            foreach (var locData in GameLocationData)
            {
                locationName = locData.Key;
                if (locationName == "fishingGame" || locationName == "Temp") { continue; } //Ignore... whatever these are. I think fishingGame is the Autumn Fair, but Temp???

                foreach (var fish in locData.Value.Fish)
                {
                    if(fish.ItemId == null)
                    {
                        continue;
                    }

                    FishData fishData = GetOrCreateData(fish.ItemId);

                    if (fish.ItemId == "(O)CaveJelly" || fish.ItemId == "(O)RiverJelly" || fish.ItemId == "(O)SeaJelly")
                    {
                        fishData.FishName = new StardewValley.Object(fish.ItemId[3..], 1).DisplayName;
                        if (fishData.CatchingTimes.Count == 0)
                        {
                            fishData.AddTimes(600, 2600);
                        }
                        fishData.AddWeather(Weather.Rain | Weather.Sun);
                    }

                    //Getting Season info per location
                    if (fish.Season.HasValue)
                    {
                        fishData.AddSeasonForLocation(locationName, fish.Season.Value.ToFishSeason());
                    }
                    else if (fish.Condition != null)
                    {
                        var conditionData = GameStateQuery.Parse(fish.Condition);
                        foreach (var condition in conditionData)
                        {
                            var splitQuery = condition.Query;
                            if (splitQuery[0] == "LOCATION_SEASON")
                            {
                                for(int i = 2;i < splitQuery.Length; i++)
                                {
                                    fishData.AddSeasonForLocation(locationName, Utils.StringToSeason(splitQuery[i]));
                                }
                            }
                        }
                    }
                    else
                    {
                        fishData.AddSeasonForLocation(locationName, Season.All_seasons);
                    }
                    //Time and weather info/crabpot info
                    if (!fishData.FishDataProcessed)
                    {
                        string fishNumber = fish.ItemId[3..];
                        GameFishData.TryGetValue(fishNumber, out string fishDataString);
                        if (fishDataString == null)
                        {
                            continue;
                        }
                        if(Game1.objectData.TryGetValue(fishNumber, out ObjectData data))
                        {
                            fishData.FishName = new StardewValley.Object(fish.ItemId[3..], 1).DisplayName; 
                        }
                        else
                        {
                            continue;
                        }
                        string[] fishDataFromFile = ArgUtility.SplitQuoteAware(fishDataString, '/');
                        if (fishDataFromFile[1].ToLower() == "trap")
                        {
                            fishData.SetCrabPot(true);
                            fishData.AddSeasonForLocation(fishDataFromFile[1], Season.None);
                            fishData.FishDataProcessed = true;
                        }
                        else
                        {
                            string[] times = ArgUtility.SplitBySpace(fishDataFromFile[5]);
                            for (int i = 0; i < times.Length; i += 2)
                            {
                                fishData.AddTimes(int.Parse(times[i]), int.Parse(times[i + 1]));
                            }
                            fishData.AddWeather(Utils.StringToWeather(fishDataFromFile[7]));

                            fishData.FishDataProcessed = true;
                        }
                    }
                    
                }
            }
        }
    }
}
 