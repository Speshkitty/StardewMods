using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FarmToTableReadyMeals
{
    class CookingRecipeAssetEditor : IAssetEditor
    {
        private List<CookingRecipe> KnownRecipes = new List<CookingRecipe>();

        public bool CanEdit<T>(IAssetInfo asset) => asset.AssetNameEquals("Data/CookingRecipes");

        public void Edit<T>(IAssetData asset)
        {
            ModEntry.RecipesAdded.Clear();
            IDictionary<string, string> BaseData = asset.AsDictionary<string, string>().Data;
            KnownRecipes = GetRecipesFromAsset(asset);
            int AddedRecipes = 0;

            Dictionary<int, int> AllIngredients = new Dictionary<int, int>();
            foreach (CookingRecipe testRecipe in KnownRecipes)
            {
                AllIngredients.Clear();

                AllIngredients = GetAllIngredientsFromChildren(testRecipe.OutputID);

                bool isNew = !testRecipe.Ingredients.ContentEquals(AllIngredients);

                if (isNew)
                {
                    CookingRecipe newRecipe = new CookingRecipe()
                    {
                        Name = testRecipe.Name,
                        Source = "null",// testRecipe.Source,
                        MysteryText = testRecipe.MysteryText,
                        OutputID = testRecipe.OutputID,
                        Ingredients = AllIngredients
                    };
                    
                    string NameToAdd = newRecipe.GetKey();

                    if (ModEntry.RecipesAdded.TryGetValue(newRecipe.GetKey(), out List<string> SecondaryRecipes))
                    {
                        //We have already created a list
                        if(SecondaryRecipes.Count == 0)
                        {
                            NameToAdd = $"Alt: {NameToAdd}";
                        }
                        else
                        {
                            NameToAdd = $"Alt {SecondaryRecipes.Count + 1}: {NameToAdd}";
                        }

                        SecondaryRecipes.Add(NameToAdd);
                        Logger.LogDebug($"{newRecipe.GetKey()}: {string.Join(", ", SecondaryRecipes)}");
                    }
                    else
                    {
                        NameToAdd = $"Alt: {NameToAdd}";
                        ModEntry.RecipesAdded.Add(newRecipe.GetKey(), new List<string>() { $"{NameToAdd}" });
                    }
                    
                    BaseData[NameToAdd] = newRecipe.GetValue();
                    AddedRecipes++;
                }
            }
            Logger.LogInfo($"Added {AddedRecipes} recipes!");
        }
        
        private List<CookingRecipe> GetRecipesFromAsset(IAssetData asset)
        {
            List<CookingRecipe> toReturn = new List<CookingRecipe>();
            Dictionary<string, string> BaseRecipes = (Dictionary<string, string>)asset.Data;

            CookingRecipe cr;
            foreach (KeyValuePair<string, string> recipe in BaseRecipes)
            {
                cr = new CookingRecipe();
                string[] recipeLine = recipe.Value.Split('/');
                cr.Name = recipe.Key;
                cr.Source = recipeLine[3];

                if (Regex.IsMatch(recipeLine[2], "[0-9]+ [0-9]+"))
                {
                    cr.OutputID = int.Parse(recipeLine[2].Split(' ')[0]);
                    cr.Amount = int.Parse(recipeLine[2].Split(' ')[1]);
                }
                else
                {
                    cr.OutputID = int.Parse(recipeLine[2]);
                }

                string[] ingredientPairs = recipeLine[0].Split(' ');
                for (int i = 0; i < ingredientPairs.Length; i = i + 2)
                {
                    int ingredientId = int.Parse(ingredientPairs[i]);
                    int ingredientAmount = int.Parse(ingredientPairs[i + 1]);

                    cr.AddIngredient(ingredientId, ingredientAmount);
                }

                toReturn.Add(cr);
            }
            return toReturn;
        }

        private Dictionary<int, int> GetAllIngredientsFromChildren(int ItemCreated)
        {
            Dictionary<int, int> ingredientsFound = new Dictionary<int, int>();

            //ItemCreated is an item we can cook
            IEnumerable<CookingRecipe> recipes = KnownRecipes.Where(x => x.OutputID == ItemCreated);
            foreach (CookingRecipe ingredientsForChild in recipes)
            {
                foreach (KeyValuePair<int, int> IngredientPairs in ingredientsForChild.Ingredients)
                {
                    Dictionary<int, int> newToAdd = GetAllIngredientsFromChildren(IngredientPairs.Key);

                    if (newToAdd.Count == 0)
                    {
                        if (ingredientsFound.TryGetValue(IngredientPairs.Key, out int amount))
                        {
                            ingredientsFound[IngredientPairs.Key] += amount;
                        }
                        else
                        {
                            ingredientsFound.Add(IngredientPairs.Key, IngredientPairs.Value);
                        }
                    }
                    else
                    {
                        foreach (var value in newToAdd)
                        {

                            if (ingredientsFound.TryGetValue(value.Key, out int amount))
                            {
                                ingredientsFound[value.Key] += value.Value * IngredientPairs.Value;
                            }
                            else
                            {
                                ingredientsFound.Add(value.Key, value.Value * IngredientPairs.Value);
                            }
                        }
                    }
                }
            }

            return ingredientsFound;
        }

    }
}
