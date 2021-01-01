using Harmony;
using StardewValley;
using StardewValley.Events;
using System.Reflection;

namespace CustomiseChildBedroom
{
    class HarmonyPatch
    {
        public static void Install()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("speshkitty.childbedconfig.harmony");

            harmony.Patch(typeof(QuestionEvent).GetMethod("setUp", BindingFlags.Public | BindingFlags.Instance),
                prefix: new HarmonyMethod(typeof(HarmonyPatch).GetMethod("PreventPregnancy")));
        }

        public static bool PreventPregnancy(QuestionEvent __instance, int ___whichQuestion)
        {
            //If it's a 2 it's a barn animal, and that's ok
            if (___whichQuestion != 2)
            {
                if (!ModEntry.Config.GetCurrentFarm().GetFarmer(Game1.player.Name).ShowCrib)
                {
                    ModEntry.Log(Translation.GetString("effect.didwork"), StardewModdingAPI.LogLevel.Debug);
                    return false;
                }
            }
            return true;
        }
    }
}
