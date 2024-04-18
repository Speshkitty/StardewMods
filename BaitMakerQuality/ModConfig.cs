using System;
using System.Collections.Generic;

namespace BaitMakerQuality
{
    class ModConfig
    {
        public BaitAmountData NoQualityInput { get; set; } = new BaitAmountData(5, 10);
        public BaitAmountData SilverQualityInput { get; set; } = new BaitAmountData(10, 15);
        public BaitAmountData GoldQualityInput { get; set; } = new BaitAmountData(15, 20);
        public BaitAmountData IridiumQualityInput { get; set; } = new BaitAmountData(20, 30);

        public override bool Equals(object? obj)
        {
            return obj is ModConfig config &&
                   EqualityComparer<BaitAmountData>.Default.Equals(NoQualityInput, config.NoQualityInput) &&
                   EqualityComparer<BaitAmountData>.Default.Equals(SilverQualityInput, config.SilverQualityInput) &&
                   EqualityComparer<BaitAmountData>.Default.Equals(GoldQualityInput, config.GoldQualityInput) &&
                   EqualityComparer<BaitAmountData>.Default.Equals(IridiumQualityInput, config.IridiumQualityInput);
        }

        public BaitAmountData GetAmountForQuality(int quality)
        {
            return quality switch
            {
                0 => NoQualityInput,
                1 => SilverQualityInput,
                2 => GoldQualityInput,
                4 => IridiumQualityInput,
                _ => NoQualityInput,
            };
        }

        public BaitAmountData GetAmountForQuality(string quality)
        {
            return quality switch
            {
                "none" => NoQualityInput,
                "silver" => SilverQualityInput,
                "gold" => GoldQualityInput,
                "iridium" => IridiumQualityInput,
                _ => NoQualityInput,
            };
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NoQualityInput, SilverQualityInput, GoldQualityInput, IridiumQualityInput);
        }

        internal class BaitAmountData
        {
            public int MinAmount { get; set; }
            public int MaxAmount { get; set; }

            public BaitAmountData(int min, int max)
            {
                MinAmount = min;
                MaxAmount = max;
            }

            public override bool Equals(object? obj)
            {
                return obj is BaitAmountData data &&
                       MinAmount == data.MinAmount &&
                       MaxAmount == data.MaxAmount;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(MinAmount, MaxAmount);
            }
        }
    }
}
