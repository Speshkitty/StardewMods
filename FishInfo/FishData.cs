using HarmonyLib;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FishInfo
{
    internal class FishData
    {
        internal Dictionary<string, Season> LocationData = new Dictionary<string, Season>();
        internal List<TimePair> CatchingTimes = new List<TimePair>();
        internal Weather weather = Weather.None;

        internal string FishName { get; set; }
        internal bool IsCrabPot = false;
        internal bool FishDataProcessed { get; set; } = false;

        private string infoLocation = "";
        private string infoWeather = "";
        private string infoTime = "";
        private string infoSeasons = "";
        private string infoLocationSeasons = "";

        internal string InfoLocation
        {
            get
            {
                if (string.IsNullOrWhiteSpace(infoLocation))
                {
                    CreateLocationString();
                }
                return infoLocation;
            }
            private set => infoLocation = value;
        }
        internal string InfoWeather
        {
            get
            {
                if (string.IsNullOrWhiteSpace(infoWeather))
                {
                    CreateWeatherString();
                }
                return infoWeather;
            }
            private set => infoWeather = value;
        }
        internal string InfoTime
        {
            get
            {
                if (string.IsNullOrWhiteSpace(infoTime))
                {
                    CreateTimeString();
                }
                return infoTime;
            }
            private set => infoTime = value;
        }
        internal string InfoSeason
        {
            get
            {
                if (string.IsNullOrWhiteSpace(infoSeasons))
                {
                    CreateSeasonString();
                }
                return infoSeasons;
            }
            private set => infoSeasons = value;

        }
        internal string InfoSeasonsWithLocations
        {
            get
            {
                if (string.IsNullOrWhiteSpace(infoLocationSeasons))
                {
                    CreateSeasonsWithLocations();
                }
                return infoLocationSeasons;
            }
            private set => infoLocationSeasons = value;

        }

        private void CreateSeasonString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"{Translation.GetString("season.prefix")}:".AsGameText());
            sb.Append($"  {LocationData.Values.Distinct().Join(new Func<Season, string>(Translation.GetString))}".AsGameText());
            InfoSeason = sb.ToString();
        }
        private void CreateWeatherString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"{Translation.GetString("weather.prefix")}:".AsGameText());
            sb.AppendLine($"  {Translation.GetString(weather)}".AsGameText());

            InfoWeather = sb.ToString();
        }
        private void CreateTimeString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"{Translation.GetString("time.prefix")}:".AsGameText());

            List<string> strings = new List<string>();
            foreach (TimePair times in CatchingTimes)
            {
                strings.Add($"{Game1.getTimeOfDayString(times.StartTime)} - {Game1.getTimeOfDayString(times.EndTime)}");
            }
            sb.AppendLine($"  {strings.Join()}".AsGameText());

            InfoTime = sb.ToString();
        }
        private void CreateSeasonsWithLocations()
        {
            if (FishName == "Goby") 
            { 
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("Caught in:");
            foreach (Season s in LocationData.Values.Distinct())
            {
                sb.AppendLine($"  {Translation.GetString(s)}".AsGameText());
                List<string> locs = new List<string>();
                foreach (var v in LocationData.Where(x => x.Value == s))
                {
                    locs.Add(v.Key);
                }

                sb.Append($"    {locs.Join(Translation.GetLocationName).AsGameText()}");
                sb.AppendLine();
            }

            infoLocationSeasons= sb.ToString();
        }

        internal void AddTimes(int StartTime, int EndTime)
        {
            TimePair times = new TimePair(StartTime, EndTime);
            if (!CatchingTimes.Contains(times))
            {
                CatchingTimes.Add(times);
                InfoTime = "";
            }
        }
        internal void AddWeather(Weather weather)
        {
            if (!this.weather.HasFlag(weather))
            {
                this.weather |= weather;
                InfoWeather = "";
            }
        }
        
        internal void AddSeasonForLocation(string location, Season season)
        {
            if (!LocationData.ContainsKey(location))
            {
                LocationData.Add(location, season);
            }
            else
            {
                LocationData[location] |= season;
            }
        }
        
        internal void SetCrabPot(bool IsCrabPot)
        {
            this.IsCrabPot = IsCrabPot;
        }
        
        internal void CreateLocationString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine();

            

            if (IsCrabPot)
            {
                sb.AppendLine(
                    Translation.GetString(
                        "location.crabpot",
                        new
                        {
                            location = LocationData.Keys.Join(new Func<string, string>(Translation.GetLocationName))
                        }
                    )
                );
            }
            else
            {
                sb.AppendLine($"{Translation.GetString("location.prefix")}:");
                sb.Append("  ");

                if (LocationData.Count == 0)
                {
                    sb.AppendLine(Translation.GetString("location.none"));
                }
                else
                {
                    
                    sb.AppendLine(LocationData.Keys.Join(new Func<string, string>(Translation.GetLocationName)).AsGameText());
                }

            }

            InfoLocation = sb.ToString();
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(FishName);
            sb.AppendLine();
            return base.ToString();

            //return FishName + Environment.NewLine +
            //InfoLocation + InfoSeason + InfoTime + InfoWeather;
        }
    }
}