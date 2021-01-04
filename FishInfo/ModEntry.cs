using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;

namespace FishInfo
{
    public class ModEntry : StardewModdingAPI.Mod
    {
        internal static Dictionary<int, FishData> FishInfo = new Dictionary<int, FishData>();

        internal static new IModHelper Helper;
        internal static new IMonitor Monitor;

        internal static ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            ModEntry.Helper = helper;
            ModEntry.Monitor = base.Monitor;

            LoadConfig();
            Patches.DoPatches();

            Logger.LogDebug(Game1.getTimeOfDayString(0));
            Logger.LogDebug(Game1.getTimeOfDayString(600));
            Logger.LogDebug(Game1.getTimeOfDayString(1200));
            Logger.LogDebug(Game1.getTimeOfDayString(1800));

            Helper.Events.GameLoop.DayStarted += LoadData;
        }

        private static FishData GetOrCreateData(int fishID)
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

            Dictionary<string, string> LocationData = Helper.Content.Load<Dictionary<string, string>>("Data\\Locations", ContentSource.GameContent);
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
                 */
                 
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
    }
}
 