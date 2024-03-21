
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.GameData.Objects;
using StardewValley.Menus;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FishInfo
{
    public class Patches
    {
        public static void DoPatches()
        {
            Harmony harmony = new Harmony("speshkitty.fishinfo.harmony");

            harmony.PatchAll();

            //harmony.Patch(typeof(CollectionsPage).GetMethod("performHoverAction", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly),
            //    postfix: new HarmonyMethod(typeof(CollectionsPage_PerformHoverAction).GetMethod("Postfix")));

            //harmony.Patch(typeof(CollectionsPage).GetMethod("createDescription", BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly),
            //    );
            
        }

        /// <summary>
        /// Runs when mousing over an uncaught fish, or a caught fish after CollectionsPage_CreateDescription
        /// </summary>
        ///
        [HarmonyPatch(typeof(CollectionsPage), nameof(CollectionsPage.performHoverAction))]
        public class CollectionsPage_PerformHoverAction
        {
            public static void Postfix(CollectionsPage __instance, ref string ___hoverText, int ___currentTab, int ___currentPage, int x, int y)
            {
                if (___currentTab != 1) // We only care about the fish tab
                {
                    return;
                }
                foreach (ClickableTextureComponent c in __instance.collections[___currentTab][___currentPage])
                {
                    if (!c.containsPoint(x, y))
                    {
                        c.scale = Math.Max(c.scale - 0.02f, c.baseScale);
                    }
                    else
                    {
                        c.scale = Math.Min(c.scale + 0.02f, c.baseScale + 0.1f);
                        string baseFishName = ArgUtility.SplitBySpaceAndGet(c.name, 0);
                        string FishID = "(O)" + baseFishName;
                        //string FishID = c.name.Split(new char[] { ' ' })[0];
                        if (!ModEntry.FishInfo.TryGetValue(FishID, out FishData fishData))
                        {
                            if (DataLoader.Fish(Game1.content).TryGetValue(baseFishName, out string tempFishData))
                            {
                                fishData = ModEntry.GetOrCreateData(FishID);
                                string[] data = ArgUtility.SplitQuoteAware(tempFishData, '/');
                                fishData.FishName = data[0];
                                if (data[1] == "trap")
                                {
                                    fishData.IsCrabPot= true;
                                    fishData.AddSeasonForLocation(data[4], Season.None);
                                }
                                else
                                {
                                    ___hoverText = "???";
                                    continue;
                                }
                            }
                            else
                            {
                                ___hoverText = "???";
                                continue;
                            }
                            StringBuilder sb = new StringBuilder();
                            if (ModEntry.Config.UncaughtFishData.AlwaysShowName)
                            {
                                sb.AppendLine(fishData.FishName);
                            }
                            if (fishData.IsCrabPot && ModEntry.Config.UncaughtFishData.AlwaysShowLocation)
                            {
                                sb.AppendLine(fishData.InfoLocation);
                                ___hoverText = sb.ToString();
                                return;
                            }
                        }

                        if (!Convert.ToBoolean(ArgUtility.SplitBySpaceAndGet(c.name, 1))) //Isn't unlocked
                        {
                            ___hoverText = "";



                            StringBuilder sb = new StringBuilder();
                            if (ModEntry.Config.UncaughtFishData.AlwaysShowName)
                            {
                                sb.AppendLine(fishData.FishName);
                            }
                            if (fishData.IsCrabPot && ModEntry.Config.UncaughtFishData.AlwaysShowLocation)
                            {
                                sb.AppendLine(fishData.InfoLocation);
                                ___hoverText = sb.ToString();
                                return;
                            }
                            //if (ModEntry.Config.UncaughtFishData.AlwaysShowLocation)
                            //{
                            //    sb.AppendLine(fishData.InfoLocation);
                            //}
                            if (ModEntry.Config.UncaughtFishData.AlwaysShowSeason && ModEntry.Config.UncaughtFishData.AlwaysShowLocation)
                            {
                                sb.AppendLine(fishData.InfoSeasonsWithLocations);
                            }
                            else if (ModEntry.Config.UncaughtFishData.AlwaysShowLocation)
                            {
                                sb.AppendLine(fishData.InfoLocation);
                            }
                            else if (ModEntry.Config.UncaughtFishData.AlwaysShowSeason)
                            {
                                sb.AppendLine(fishData.InfoSeason);
                            }
                            if (ModEntry.Config.UncaughtFishData.AlwaysShowTime)
                            {
                                sb.AppendLine(fishData.InfoTime);
                            }
                            if (ModEntry.Config.UncaughtFishData.AlwaysShowWeather)
                            {
                                sb.AppendLine(fishData.InfoWeather);
                            }

                            ___hoverText = sb.ToString().Trim();
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Runs when mousing over a caught fish
        /// </summary>
        [HarmonyPatch(typeof(CollectionsPage), nameof(CollectionsPage.createDescription))]
        public class CollectionsPage_CreateDescription
        {
            public static void Postfix(CollectionsPage __instance, ref string __result, string id)
            {
                if (!Game1.objectData.TryGetValue(id, out ObjectData itemData)) // Break if it's not an item - dodges weird edge cases
                {
                    return;
                }
                
                
                    


                if (itemData.Type != "Fish") { return; } //break if it isn't a fish

                if (ModEntry.FishInfo.TryGetValue("(O)" + id, out FishData loadedData))
                {


                    if (!loadedData.IsCrabPot)
                    {
                        __result += Environment.NewLine;
                        if (ModEntry.Config.CaughtFishData.AlwaysShowSeason && ModEntry.Config.CaughtFishData.AlwaysShowLocation)
                        {
                            __result += loadedData.InfoSeasonsWithLocations;
                        }
                        else if (!ModEntry.Config.CaughtFishData.AlwaysShowSeason && ModEntry.Config.CaughtFishData.AlwaysShowLocation)
                        {
                            __result += loadedData.InfoLocation;
                        }
                        else if (ModEntry.Config.CaughtFishData.AlwaysShowSeason && !ModEntry.Config.CaughtFishData.AlwaysShowLocation)
                        {
                            __result += loadedData.InfoSeason;
                        }
                        if (ModEntry.Config.CaughtFishData.AlwaysShowTime)
                        {
                            __result += loadedData.InfoTime;
                        }
                        if (ModEntry.Config.CaughtFishData.AlwaysShowWeather)
                        {
                            __result += loadedData.InfoWeather;
                        }
                    }
                    else
                    {
                        if (ModEntry.Config.CaughtFishData.AlwaysShowLocation)
                        {
                            __result += loadedData.InfoLocation;
                        }
                    }
                }
            }
            
        }

    }
}
