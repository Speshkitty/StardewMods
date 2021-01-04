using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishInfo
{
    class ModConfig
    {
        /// <summary>
        /// Should we show information about caught fish?
        /// </summary>
        public ShowFishData CaughtFishData { get; set; } = new ShowFishData();

        /// <summary>
        /// Should we show information about uncaught fish?
        /// </summary>
        public ShowFishData UncaughtFishData { get; set; } = new ShowFishData();
        
        public ModConfig()
        {
            CaughtFishData.SetAll(true);
        }

        public override bool Equals(object conf)
        {
            if(!(conf is ModConfig))
            {
                return false;
            }

            ModConfig opp = conf as ModConfig;

            if (!CaughtFishData.Equals(opp.CaughtFishData)) { return false; }
            if (!UncaughtFishData.Equals(opp.UncaughtFishData)) { return false; }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = -766248109;
            hashCode = hashCode * -1521134295 + EqualityComparer<ShowFishData>.Default.GetHashCode(CaughtFishData);
            hashCode = hashCode * -1521134295 + EqualityComparer<ShowFishData>.Default.GetHashCode(UncaughtFishData);
            return hashCode;
        }
    }
    internal class ShowFishData
    {
        public bool AlwaysShowName { get; set; } = true;
        public bool AlwaysShowLocation { get; set; } = false;
        public bool AlwaysShowSeason { get; set; } = false;
        public bool AlwaysShowTime { get; set; } = false;
        public bool AlwaysShowWeather { get; set; } = false;


        public void SetAll(bool SetTo)
        {
            AlwaysShowName = SetTo;
            AlwaysShowLocation = SetTo;
            AlwaysShowSeason = SetTo;
            AlwaysShowTime = SetTo;
            AlwaysShowWeather = SetTo;
        }

        public override bool Equals(object conf)
        {
            if(!(conf is ShowFishData))
            {
                return false;
            }

            ShowFishData opp = conf as ShowFishData;

            if (AlwaysShowName != opp.AlwaysShowName) { return false; }
            if (AlwaysShowLocation != opp.AlwaysShowLocation) { return false; }
            if (AlwaysShowSeason != opp.AlwaysShowSeason) { return false; }
            if (AlwaysShowTime != opp.AlwaysShowTime) { return false; }
            if (AlwaysShowWeather != opp.AlwaysShowWeather) { return false; }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 98380371;
            hashCode = hashCode * -1521134295 + AlwaysShowName.GetHashCode();
            hashCode = hashCode * -1521134295 + AlwaysShowLocation.GetHashCode();
            hashCode = hashCode * -1521134295 + AlwaysShowSeason.GetHashCode();
            hashCode = hashCode * -1521134295 + AlwaysShowTime.GetHashCode();
            hashCode = hashCode * -1521134295 + AlwaysShowWeather.GetHashCode();
            return hashCode;
        }
    }
}
