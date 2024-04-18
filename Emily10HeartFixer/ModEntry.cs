using StardewModdingAPI;
using StardewValley;
using StardewValley.Delegates;
using StardewValley.GameData;
using StardewValley.Triggers;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static StardewValley.GameStateQuery;

namespace Emily10HeartFixer
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Events.Content.AssetRequested += Content_AssetRequested;
        }

        private void Content_AssetRequested(object? sender, StardewModdingAPI.Events.AssetRequestedEventArgs e)
        {
            if (!e.NameWithoutLocale.IsEquivalentTo("Data/TriggerActions")) { return; }
            
            e.Edit(asset =>
            {
                var data = asset.GetData<List<TriggerActionData>>();
                data.ForEach(action =>
                {
                    if (action.Id == "Mail_Emily_10heart")
                    {
                        action.Condition = action.Condition.Replace(" PLAYER_NPC_RELATIONSHIP Current Emily Dating Engaged Married,", "");
                        return;
                    }
                });
            });
        }
    }
}