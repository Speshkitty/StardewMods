using Harmony;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Events;
using StardewValley.Locations;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomiseChildBedroom
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private static readonly string SaveDataKey = "speshkitty.customisechildbedrooms";
        /*****************************/
        /**      Properties         **/
        /*****************************/
        ///<summary>The config file from the player</summary>
        internal static ModConfig Config;
        private Commands Commands;

        internal new static IMonitor Monitor;
        internal new static IModHelper Helper;

        internal FarmData CurrentFarm;

        /*****************************/
        /**      Public methods     **/
        /*****************************/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Helper = base.Helper;
            Monitor = base.Monitor;

            Config = Helper.ReadConfig<ModConfig>(); //TODO: Feed this into the save game properly, then retire it

            HarmonyPatch.Install();

            Commands = new Commands(Helper);

            //Add loading and saving our own data after the game is done
            Helper.Events.GameLoop.SaveLoaded += (object sender, SaveLoadedEventArgs e) => { CurrentFarm = Helper.Data.ReadSaveData<FarmData>(SaveDataKey); };
            Helper.Events.GameLoop.Saved += (object sender, SavedEventArgs e) => { Helper.Data.WriteSaveData(SaveDataKey, CurrentFarm); };

            //Watch for building changes to update our data
            Helper.Events.World.LocationListChanged += World_LocationListChanged;

            //Run daily for when commands are ran
            Helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;

            if (Game1.IsMasterGame)
            {
                Helper.Events.Multiplayer.PeerConnected += Multiplayer.PeerConnected; // Runs when someone joins the game - This should only run for the host though
            }
            else
            {
                Helper.Events.Multiplayer.ModMessageReceived += Multiplayer.ModMessageReceived; // Runs when a modmessage is sent - This should only run for clients, not the host
            }

        }

        //Raised when buildings are added or removed
        private void World_LocationListChanged(object sender, LocationListChangedEventArgs e)
        {
            //We only care about cabins (and farmhouses, but that should never actually change)

            foreach (FarmHouse fh in e.Added.OfType<FarmHouse>())
            {
                string ownerorID;
                if ((ownerorID = fh.GetOwner()) == "")
                {
                    ownerorID = fh.uniqueName;
                }

                CurrentFarm.AddCabin(ownerorID, Bed.ALL);
            }

            foreach (FarmHouse fh in e.Removed.OfType<FarmHouse>())
            {
                string ownerorID;
                if ((ownerorID = fh.GetOwner()) == "")
                {
                    ownerorID = fh.uniqueName;
                }

                CurrentFarm.RemoveCabin(ownerorID);
            }
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            bool MadeChanges = false;
            Farm Farm = GetFarmInfo();
            if (Farm.FarmName.Equals(string.Empty))
            {
                string FarmName = Game1.player.farmName.Value;
                Log(Translation.GetString("error.farmnamenotfound", new { FarmName }));
                Log(Translation.GetString("error.creatingconfigentry"));
                Farm.FarmName = FarmName;
                Config.Farms.Add(Farm);
                MadeChanges = true;
            }

            List<GameLocation> buildings = new List<GameLocation>() { Game1.getLocationFromName("FarmHouse") };
            buildings.AddRange(getCabins());

            foreach (GameLocation building in buildings)
            {
                if (!(building is FarmHouse)) // Cabins are a type of farmhouse
                {
                    continue;
                }


                FarmHouse farmHouse = building as FarmHouse;

                int BuildingUpgradeLevel = farmHouse.GetLevel();
                string HouseOwner = farmHouse.GetOwner();

                if (HouseOwner.Equals(string.Empty)) { HouseOwner = farmHouse.uniqueName; } //Building doesn't have an owner - in this case we actually want to use the cabin ID


                if (!Farm.FarmerInfo.TryGetValue(HouseOwner, out Farmer BuildingOwner)) //No config entry for farmer name - Make a default one and continue working
                {
                    BuildingOwner = new Farmer();

                    Config.Farms[Config.Farms.IndexOf(Farm)].FarmerInfo.Add(HouseOwner, BuildingOwner);
                    MadeChanges = true;

                    Farm.FarmerInfo[HouseOwner] = BuildingOwner;
                }

                if (BuildingUpgradeLevel <= 1) //Building doesn't have bedrooms 
                {
                    //This check is after attempting to get info because this way it generates a config file block for every farmhand, not just those who have already upgraded
                    continue;
                }

                if (!BuildingOwner.ShowCrib)
                {
                    farmHouse.RemoveCrib();
                }

                if (!BuildingOwner.ShowLeftBed)
                {
                    farmHouse.RemoveLeftBed();
                }

                if (!BuildingOwner.ShowRightBed)
                {
                    farmHouse.RemoveRightBed();
                }

            }

            if (MadeChanges)
            {
                Config.Save(); //Write any changes made
            }
        }

        internal static void Log(object text, LogLevel logLevel = LogLevel.Info)
            => Monitor.Log(text.ToString(), logLevel);

        internal static void LogError(object text)
            => Log(text, LogLevel.Error);


        private Farm GetFarmInfo()
            => Config.Farms.Find(x => x.FarmName == Game1.player.farmName.Value) ?? new Farm();


        public static List<GameLocation> getCabins()
        {
            StardewValley.Farm farm = Game1.getLocationFromName("Farm") as StardewValley.Farm;
            List<GameLocation> cabins = new List<GameLocation>();

            farm.buildings.Where(x => x.isCabin).All(i => { cabins.Add(i.indoors.Value); return true; });

            return cabins;
        }

    }
}