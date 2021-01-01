using System;
using System.Collections.Generic;

namespace CustomiseChildBedroom
{
    public class FarmData
    {
        List<CabinData> AllCabins = new List<CabinData>();

        internal CabinData GetCabin(string SearchText) => AllCabins.Find(x => x.PlayerNameOrCabinID.Equals(SearchText, StringComparison.OrdinalIgnoreCase));

        internal void AddCabin(string PlayerNameOrCabinID, Bed EnabledBeds)
        {
            AllCabins.Add(new CabinData(PlayerNameOrCabinID, EnabledBeds));
        }

        internal void RemoveCabin(string PlayerNameOrCabinID)
        {
            AllCabins.Remove(AllCabins.Find(x => x.PlayerNameOrCabinID.Equals(PlayerNameOrCabinID, StringComparison.OrdinalIgnoreCase)));
        }
    }

    class CabinData
    {
        internal string PlayerNameOrCabinID;
        internal Bed EnabledBeds = Bed.ALL;

        public CabinData(string PlayerNameOrCabinID, Bed EnabledBeds)
        {
            this.PlayerNameOrCabinID = PlayerNameOrCabinID;
            this.EnabledBeds = EnabledBeds;
        }
    }
}
