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


        public void LoadData(object sender, EventArgs e)
        {
            FishInfo.Clear();

            Dictionary<string, LocationData> GameLocationData = DataLoader.Locations(Game1.content);
            Dictionary<string, string> GameFishData = DataLoader.Fish(Game1.content);
            string locationName;
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
                    //Getting Season info per location
                    if (fish.Season.HasValue)
                    {
                        fishData.AddSeasonForLocation(locationName, fish.Season.Value.ToFishSeason());
                    }
                    if (fish.Condition != null)
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
                            fishData.FishName = data.Name;
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
            FishData octTest = GetOrCreateData("(O)149");
        }

        /*
        public void LoadData15(object sender, EventArgs e)
        {
            FishInfo.Clear();

            
            
            Dictionary<string, string> LocationData = Helper.GameContent.Load<Dictionary<string, string>>("Data/Locations");
            foreach (KeyValuePair<string, string> locdata in LocationData)
            {
                string locationName = locdata.Key;

                if (locationName == "fishingGame" || locationName == "Temp") continue; //don't want these - what the fuck even is temp

                string[] data = locdata.Value.Split('/');

                
                /*
                 * Sample data line
                 * "Desert": "88 .5 90 .5/88 .5 90 .5/88 .5 90 .5/88 .5 90 .5/153 -1 164 -1 165 -1/153 -1 164 -1 165 -1/153 -1 164 -1 165 -1/153 -1 164 -1 165 -1/390 .25 330 1",
                 * "Farm": "-1/-1/-1/-1/-1/-1/-1/-1/382 .05 770 .1 390 .25 330 1",
                 * Format:
                 * Key: Spring Forage/Summer Forage/Fall Forage/Winter Forage/Spring Fish/Summer Fish/Fall Fish/Winter Forage/Dig spots
                 * 
                 * 
                 * 
                 * 
                 * 
                 * /
                 
                string[] seasonData;
                for (int i = 4; i <= 7; i++)
                {
                    
                    if(string.CompareOrdinal(data[i], "-1") == 0)
                    {
                        continue;
                    }

                    seasonData = data[i].Split(' ');

                    Queue<string> testQueue = new Queue<string>(seasonData);

                    int FishID;
                    int ForestRegion;
                    FishData fd;

                    while(testQueue.Count > 0)
                    {
                        string NextFish = testQueue.Dequeue();
                        if (NextFish == "-1") continue; // If the fish ID taken from the list is -1, skip it
                        
                        if(NextFish == "1069-1") //This is the fix for the issue with More New Fish on Spirit's Eve
                        {
                            FishID = 1069;
                            ForestRegion = -1;
                        }
                        else
                        {
                            int.TryParse(NextFish, out FishID);
                            if (testQueue.Count == 0)
                            {
                                ForestRegion = -1;
                            }
                            else
                            {
                                int.TryParse(testQueue.Peek(), out ForestRegion);
                            }
                        }

                        fd = GetOrCreateData(FishID);

                        Season currentSeason = (Season)(1 << (i - 4));
                        if (string.CompareOrdinal(locationName, "Forest") == 0)
                        {
                            if (ForestRegion == -1)
                            {
                                fd.AddSeasonForLocation("ForestRiver", currentSeason);
                                fd.AddSeasonForLocation("ForestPond", currentSeason);
                            }
                            else if (ForestRegion == 0)
                            {
                                fd.AddSeasonForLocation("ForestRiver", currentSeason);
                            }
                            else
                            {
                                fd.AddSeasonForLocation("ForestPond", currentSeason);
                            }
                        }
                        else
                        {
                            fd.AddSeasonForLocation(locationName, currentSeason);
                        }
                    }
                }

            }
            Dictionary<int, string> FishData = Helper.Content.Load<Dictionary<int, string>>("Data\\Fish", ContentSource.GameContent);
            foreach (KeyValuePair<int, string> fishData in FishData)
            {
                int FishID = fishData.Key;
                string[] fishInfo = fishData.Value.Split('/');

                FishData fd = GetOrCreateData(FishID);
                if (fishInfo.Length == 14)
                {
                    fd.FishName = fishInfo[13];
                }
                else if (fishInfo.Length == 13 || fishInfo.Length == 7)
                {
                    string data = Helper.Content.Load<Dictionary<int, string>>("Data\\ObjectInformation", ContentSource.GameContent)[FishID];
                    fd.FishName = data.Split('/')[4];
                }
                else if (fishInfo.Length == 8)
                {
                    fd.FishName = fishInfo[7];
                }
                else
                {
                    fd.FishName = fishInfo[4];
                }

                if (string.CompareOrdinal(fishInfo[1], "trap") == 0) //crabpot
                {
                    fd.IsCrabPot = true;
                    fd.AddSeasonForLocation(fishInfo[4], Season.None);
                }
                else
                {
                    string[] times = fishInfo[5].Split(' ');

                    for (int time = 0; time < times.Length; time += 2)
                    {
                        fd.AddTimes(int.Parse(times[time]), int.Parse(times[time + 1]));
                    }

                    if (string.CompareOrdinal(fishInfo[7], "sunny") == 0 || string.CompareOrdinal(fishInfo[7], "both") == 0)
                    {
                        fd.AddWeather(Weather.Sun);
                    }
                    if (string.CompareOrdinal(fishInfo[7], "rainy") == 0 || string.CompareOrdinal(fishInfo[7], "both") == 0)
                    {
                        fd.AddWeather(Weather.Rain);
                    }
                }
            }
        }
            */
    }
}
 