using StardewValley.GameData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImprovedBundleRemixer
{
    class DefaultBundleData
    {
        internal static List<CustomBundleData> GetDefaults()
        {
            List<CustomBundleData> toReturn = new List<CustomBundleData>
            {
                CreateRoomCraftsRoom(),
                CreateRoomPantry(),
                CreateRoomFishTank(),
                CreateRoomBoilerRoom(),
                CreateRoomBulletinBoard()
            };

            return toReturn;
        }

        private static CustomBundleData CreateRoomCraftsRoom()
        {
            CustomBundleData CBD = new CustomBundleData
            {
                AreaName = "Crafts Room",
                Keys = "13 14 15 16 17 19"
            };

            CBD.AddBundle("Green", "1 Wild Horseradish, 1 Daffodil, 1 Leek, 1 Dandelion, 1 Spring Onion", "Spring Foraging", 4, -1, "30 Spring Seeds", @"13");
            CBD.AddBundle("Yellow", "1 Grape, 1 Spice Berry, 1 Sweet Pea", "Summer Foraging", 3, -1, "30 Summer Seeds", @"14");
            CBD.AddBundle("Orange", "1 Common Mushroom, 1 Wild Plum, 1 Hazelnut, 1 Blackberry", "Fall Foraging", 4, -1, "30 Fall Seeds", @"15");
            CBD.AddBundle("Teal", "1 Winter Root, 1 Crystal Fruit, 1 Snow Yam, 1 Crocus, 1 Holly", "Winter Foraging", 4, -1, "30 Winter Seeds", @"16");
            CBD.AddBundle("Red", "99 Wood, 99 Wood, 99 Stone, 10 Hardwood", "Construction", -1, -1, "1 Charcoal Kiln", @"17");
            CBD.AddBundle("Yellow", "500 Sap", "Sticky", -1, -1, "1 Charcoal Kiln", @"LooseSprites\BundleSprites:10");
            CBD.AddBundle("Purple", "1 Coconut, 1 Cactus Fruit, 1 Cave Carrot, 1 Red Mushroom, 1 Purple Mushroom, 1 Maple Syrup, 1 Oak Resin, 1 Pine Tar, 1 Morel", "Exotic Foraging", -1, 5, "5 Autumn's Bounty", @"19");
            CBD.AddBundle("Green", "5 Purple Mushroom, 5 Fiddlehead Fern, 5 White Algae, 5 Hops", "Wild Medicine", -1, 3, "2 Cookout Kit", @"LooseSprites\BundleSprites:9");

            return CBD;
        }
        private static CustomBundleData CreateRoomPantry()
        {
            CustomBundleData CBD = new CustomBundleData
            {
                AreaName = "Pantry",
                Keys = "0 1 2 3 4 5"
            };

            CBD.AddBundle("Green", "1 Parsnip, 1 Green Bean, 1 Cauliflower, 1 Potato", "Spring Crops", 4, -1, "20 Speed-Gro", @"0");
            CBD.AddBundle("Yellow", "1 Tomato, 1 Hot Pepper, 1 Blueberry, 1 Melon", "Summer Crops", 4, -1, "1 Quality Sprinkler", @"1");
            CBD.AddBundle("Orange", "1 Corn, 1 Eggplant, 1 Pumpkin, 1 Yam", "Fall Crops", 4, -1, "1 Bee House", @"2");
            CBD.AddBundle("Teal", "[5 GQ Parsnip|5 GQ Green Bean|5 GQ Cauliflower|5 GQ Potato], [5 GQ Melon|5 GQ Hot Pepper|5 GQ Blueberry], [5 GQ Pumpkin|5 GQ Eggplant|5 GQ Yam], [5 GQ Corn]", "Quality Crops", -1, 3, "1 Preserves Jar", @"3");
            CBD.AddBundle("Teal", "1 Ancient Fruit, 1 Sweet Gem Berry", "Rare Crops", -1, 1, "1 Preserves Jar", @"LooseSprites\BundleSprites:3");
            CBD.AddBundle("Red", "1 Large Milk, 1 180, 1 182, 1 L. Goat Milk, 1 Wool, 1 Duck Egg ", "Animal", -1, 5, "1 Cheese Press", @"4");
            CBD.AddBundle("Blue", "15 Roe, 15 Aged Roe, 1 Squid Ink", "Fish Farmer's", -1, 2, "1 Worm Bin", @"LooseSprites\BundleSprites:8");
            CBD.AddBundle("Red", "1 Tulip, 1 Summer Spangle, 1 Fairy Rose, 1 Blue Jazz, 1 Sunflower", "Garden", -1, 4, "1 Quality Sprinkler", @"LooseSprites\BundleSprites:7");
            CBD.AddBundle("Purple", "1 Truffle Oil, 1 Cloth, 1 Goat Cheese, 1 Honey, 1 Jelly, 1 Apple, 1 Apricot, 1 Orange, 1 Peach, 1 Pomegranate, 1 Cherry", "Artisan", -1, 6, "1 Keg", @"5");
            CBD.AddBundle("Orange", "1 Mead, 1 Wine, 1 Juice, 1 Pale Ale, 1 Green Tea", "Brewer's", -1, 4, "1 Keg", @"LooseSprites\BundleSprites:2");

            return CBD;
        }
        private static CustomBundleData CreateRoomFishTank()
        {
            CustomBundleData CBD = new CustomBundleData
            {
                AreaName = "Fish Tank",
                Keys = "6 7 8 9 10 11"
            };

            CBD.AddBundle("Teal", "1 Sunfish, 1 Catfish, 1 Shad, 1 Tiger Trout", "River Fish", 4, -1, "30 Bait", @"6");
            CBD.AddBundle("Green", "1 Largemouth Bass, 1 Carp, 1 Bullhead, 1 Sturgeon", "Lake Fish", 4, -1, "1 Dressed Spinner", @"7");
            CBD.AddBundle("Blue", "1 Sardine, 1 Tuna, 1 Red Snapper, 1 Tilapia", "Ocean Fish", 4, -1, "5 Warp Totem: Beach", @"8");
            CBD.AddBundle("Purple", "1 Walleye, 1 Bream, 1 Eel", "Night Fishing", 3, -1, "1 Small Glow Ring", @"9");
            CBD.AddBundle("Purple", "1 Lobster, 1 Crayfish, 1 Crab, 1 Cockle, 1 Mussel, 1 Shrimp, 1 Snail, 1 Periwinkle, 1 Oyster, 1 Clam", "Crab Pot", -1, 5, "3 Crab Pot", @"11");
            CBD.AddBundle("Red", "1 Pufferfish, 1 Ghostfish, 1 Sandfish, 1 Woodskip", "Specialty Fish", -1, 4, "1 Dish O' The Sea", @"10");
            CBD.AddBundle("Red", "1 GQ Largemouth Bass, 1 GQ Shad, 1 GQ Tuna, 1 GQ Walleye", "Quality Fish", -1, 4, "1 Dish O' The Sea", @"LooseSprites\BundleSprites:4");
            CBD.AddBundle("Red", "1 Lava Eel, 1 Scorpion Carp, 1 Octopus, 1 Blobfish", "Master Fisher's", -1, 2, "1 Dish O' The Sea", @"12");

            return CBD;
        }
        private static CustomBundleData CreateRoomBoilerRoom()
        {
            CustomBundleData CBD = new CustomBundleData
            {
                AreaName = "Boiler Room",
                Keys = "20 21 22"
            };

            CBD.AddBundle("Orange", "1 Copper Bar, 1 Iron Bar, 1 Gold Bar", "Blacksmith's", -1, 3, "1 Furnace", @"20");
            CBD.AddBundle("Purple", "1 Quartz, 1 Earth Crystal, 1 Frozen Tear, 1 Fire Quartz", "Geologist's", -1, 4, "5 Omni Geode", @"21");
            CBD.AddBundle("Purple", "99 Slime, 10 Bat Wing, 1 Solar Essence, 1 Void Essence, 10 Bone Fragment", "Adventurer's", 4, 2, "1 Small Magnet Ring", @"22");
            CBD.AddBundle("Yellow", "1 Amethyst, 1 Topaz, 1 Emerald, 1 Diamond, 1 Ruby, 1 Aquamarine", "Treasure Hunter's", -1, 5, "1 Lucky Lunch", @"LooseSprites\BundleSprites:1");
            CBD.AddBundle("Purple", "1 Iridium Ore, 1 Battery, 5 Refined Quartz", "Engineer's", -1, 3, "1 Furnace", @"LooseSprites\BundleSprites:11");

            return CBD;
        }
        private static CustomBundleData CreateRoomBulletinBoard()
        {
            CustomBundleData CBD = new CustomBundleData
            {
                AreaName = "Bulletin Board",
                Keys = "31 32 33 34 35"
            };

            CBD.AddBundle("Red", "1 Maple Syrup, 1 Fiddlehead Fern, 1 Truffle, 1 Poppy, 1 Maki Roll, 1 Fried Egg", "Chef's", -1, -1, "1 Pink Cake", @"31");
            CBD.AddBundle("Teal", "[1 Red Mushroom|1 Beet], [1 Sea Urchin|1 Amaranth], [1 Sunflower|1 Starfruit], [1 Duck Feather|1 Cactus Fruit], [1 Aquamarine|1 Blueberry], [1 Red Cabbage|1 Iridium Bar]", "Dye", -1, -1, "1 Seed Maker", @"34");
            CBD.AddBundle("Blue", "1 Purple Mushroom, 1 Nautilus Shell, 1 Chub, 1 Frozen Geode", "Field Research", -1, -1, "1 Recycling Machine", @"32");
            CBD.AddBundle("Yellow", "10 Wheat, 10 Hay, 3 Apple", "Fodder", -1, -1, "1 Heater", @"35");
            CBD.AddBundle("Purple", "1 Oak Resin, 1 Wine, 1 Rabbit's Foot, 1 Pomegranate", "Enchanter's", -1, -1, "5 Gold Bar", @"33");
            CBD.AddBundle("Green", "1 Ancient Doll, 1 Ice Cream, 1 Cookie, 10 Salmonberry", "Children's", -1, 3, "3 Battery", @"LooseSprites\BundleSprites:0");
            CBD.AddBundle("Orange", "50 Salmonberry, 50 Blackberry, 20 Wild Plum", "Forager's", -1, 2, "3 Tapper", @"LooseSprites\BundleSprites:5");
            CBD.AddBundle("Yellow", "10 EggCategory, 10 MilkCategory, 100 Wheat Flour", "Home Cook's", -1, -1, "5 Complete Breakfast", @"LooseSprites\BundleSprites:6");

            return CBD;
        }

    }
}
