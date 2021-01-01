using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmToTableReadyMeals
{
    class CookingRecipe
    {
        internal string Name { get; set; }
        internal int Amount { get; set; } = 1;
        internal int OutputID { get; set; }
        internal string Source { get; set; }
        internal string MysteryText { get; set; }

        internal Dictionary<int, int> Ingredients { get; set; } = new Dictionary<int, int>();

        internal string GetIngredientsString()
        {
            string toReturn = "";

            foreach (KeyValuePair<int, int> kvp in Ingredients)
            {
                toReturn += $"{kvp.Key} {kvp.Value} ";
            }

            return toReturn.Trim();
        }

        internal void AddIngredient(int IngredientID, int Amount)
        {
            Ingredients.Add(IngredientID, Amount);
        }

        internal string GetKey()
        {
            return Name;
        }
        internal string GetValue()
        {
            return $"{GetIngredientsString()}/{MysteryText}/{OutputID}/{Source}";
        }
    }
}
