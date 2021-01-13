using StardewModdingAPI;
using StardewModdingAPI.Events;
using System;
using StardewValley;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FarmToTableReadyMeals
{
    public class ModEntry : Mod
    {
        internal new static IModHelper Helper;
        internal new static IMonitor Monitor;

        List<CookingRecipe> CookingRecipes = new List<CookingRecipe>();

        internal static Dictionary<string, List<string>> RecipesAdded = new Dictionary<string, List<string>>();
        
        public override void Entry(IModHelper helper)
        {
            Helper = helper;
            Monitor = base.Monitor;
            
            Helper.Content.AssetEditors.Add(new CookingRecipeAssetEditor());
            Helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
        }

        private void AddRecipeToCurrentPlayer(string RecipeID, int Value)
        {
            if (RecipesAdded.ContainsKey(RecipeID))
            {
                foreach (string s in RecipesAdded[RecipeID])
                {
                    Game1.player.cookingRecipes.Add(s, 0);
                    Monitor.Log($"Unlocked \"{s}\" for player as a related recipe was unlocked!", LogLevel.Info);
                }
            }
        }
        private void RemoveRecipeFromCurrentPlayer(string RecipeID, int Value)
        {
            if (RecipesAdded.ContainsKey(RecipeID))
            {
                foreach (string s in RecipesAdded[RecipeID])
                {
                    Game1.player.cookingRecipes.Remove(s);
                    Monitor.Log($"Removed \"{s}\" for player as a related recipe was removed!", LogLevel.Info);
                }
            }
        }
        
        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            Game1.player.cookingRecipes.OnValueAdded += AddRecipeToCurrentPlayer;
            Game1.player.cookingRecipes.OnValueRemoved += RemoveRecipeFromCurrentPlayer;

            //Unlock any recipes we added as relevant
            foreach (var kvp in RecipesAdded)
            {
                if (Game1.player.cookingRecipes.ContainsKey(kvp.Key))
                {
                    foreach (string s in kvp.Value)
                    {
                        if (Game1.player.cookingRecipes.ContainsKey(s))
                        {
                            continue;
                        }
                        Game1.player.cookingRecipes.Add(s, 0);
                    }
                }
            }
        }

        
        
    }

    
}
