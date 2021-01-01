using StardewModdingAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomiseChildBedroom
{
    internal class Multiplayer
    {
        internal class Packet
        {
        }

        //This class holds data used to toggle beds in a multiplayer setting
        internal class TogglePacket
        {
            public static string StringType = "TogglePacketType";

            string PlayerNameOrCabinID;
            Bed WhichBed;

            public TogglePacket() : base() { }
            public TogglePacket(string PlayerNameOrCabinID, Bed WhichBed) : base()
            {
                this.PlayerNameOrCabinID = PlayerNameOrCabinID;
                this.WhichBed = WhichBed;
            }
        }

        internal class CurrentFarmPacket
        {
            public static string StringType = "CurrentFarmPacketType";

            List<FarmData> CurrentFarmData;
            public CurrentFarmPacket() : base() { }
        }

        //Runs when someone connects to the game
        //I only want this to run for the host, and it will send a message out with the current farm config
        internal static void PeerConnected(object sender, PeerConnectedEventArgs e)
        {

        }

        //Runs when a mod sends a message to us
        internal static void ModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            if (e.FromModID != ModEntry.Helper.Multiplayer.ModID || e.Type != Multiplayer.TogglePacket.StringType) // This isn't from our mod or a packet we know how to handle
            {
                return;
            }
        }
    }
}