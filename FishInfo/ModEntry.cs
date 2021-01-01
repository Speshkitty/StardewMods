using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
            Config = Helper.ReadConfig<ModConfig>();
            Helper.WriteConfig(Config);
            Patches.DoPatches();

            //Helper.Events.GameLoop.SaveLoaded += LoadData;
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

        public void LoadData(object sender, EventArgs e)
        {
            StopwatchCollection.CreateAndStartStopwatch("LoadData");
            FishInfo.Clear();

            StopwatchCollection.CreateAndStartStopwatch("LocationData");
            Dictionary<string, string> LocationData = Helper.Content.Load<Dictionary<string, string>>("Data\\Locations", ContentSource.GameContent);
            foreach (KeyValuePair<string, string> locdata in LocationData)
            {
                string locationName = locdata.Key;

                if (locationName == "fishingGame" || locationName == "Temp") continue; //don't want these - what the fuck even is temp

                string[] data = locdata.Value.Split('/');

                //if (locationName == "BugLand") locationName = "MutantBugLair"; //fucking bugland lmao

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
                    //StopwatchCollection.CreateAndStartStopwatch($"SeasonData{locationName}{i}");
                    
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

                        if(string.CompareOrdinal(locationName, "Forest") == 0)
                        {
                            if (ForestRegion == -1)
                            {
                                fd.AddLocation("ForestRiver");
                                fd.AddLocation("ForestPond");
                            }
                            else if (ForestRegion == 0)
                            {
                                fd.AddLocation("ForestRiver");
                            }
                            else
                            {
                                fd.AddLocation("ForestPond");
                            }
                        }
                        else
                        {
                            fd.AddLocation(locationName);
                        }

                        fd.AddSeason((Season)(1 << (i - 4)));
                    }
                    
                    //StopwatchCollection.StopStopwatchAndGetTime($"SeasonData{locationName}{i}");
                }

            }
            StopwatchCollection.StopStopwatchAndGetTime("LocationData");

            StopwatchCollection.CreateAndStartStopwatch("FishData");
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
                    fd.AddLocation(fishInfo[4]);
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
            StopwatchCollection.StopStopwatchAndGetTime("FishData");

            StopwatchCollection.StopStopwatchAndGetTime("LoadData");
        }

        private void ParseInts(string StepInLoop, string NextStepInLoop, out int FishID, out int region)
        {
            
            if (string.CompareOrdinal(StepInLoop, "1069-1") == 0) //Terrible hacky fix for issue that only occurs on spirit's eve when More new Fish is installed
            {
                FishID = 1069;
                region = -1;
                return;
            }
            FishID = int.Parse(StepInLoop);
            region = int.Parse(NextStepInLoop);
        }
    }
}
 