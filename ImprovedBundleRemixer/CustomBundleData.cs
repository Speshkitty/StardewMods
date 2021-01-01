using StardewValley.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImprovedBundleRemixer
{
    class CustomBundleData : RandomBundleData
    {
        public CustomBundleData() : base()
        {
            this.Bundles = new List<BundleData>();
        }

        internal void AddBundle(string Colour, string Items, string Name, int Pick, int RequiredItems, string Reward, string Sprite)
        {

            this.Bundles.Add(new BundleData()
            {
                Color = Colour,
                Items = Items,
                Index = -1,
                Name = Name,
                Pick = Pick,
                RequiredItems = RequiredItems,
                Reward = Reward,
                Sprite = Sprite
            });
        }
    }
}
