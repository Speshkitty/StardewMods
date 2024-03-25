using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedMakerQuality
{
    class ModConfig
    {
        public SeedAmountData NoQualityInput { get; set; } = new SeedAmountData(1, 4, true, true);
        public SeedAmountData SilverQualityInput { get; set; } = new SeedAmountData(2, 5, true, true);
        public SeedAmountData GoldQualityInput { get; set; } = new SeedAmountData(3, 6, true, true);
        public SeedAmountData IridiumQualityInput { get; set; } = new SeedAmountData(4, 7, true, true);

        public override bool Equals(object? obj)
        {
            return obj is ModConfig config &&
                   EqualityComparer<SeedAmountData>.Default.Equals(NoQualityInput, config.NoQualityInput) &&
                   EqualityComparer<SeedAmountData>.Default.Equals(SilverQualityInput, config.SilverQualityInput) &&
                   EqualityComparer<SeedAmountData>.Default.Equals(GoldQualityInput, config.GoldQualityInput) &&
                   EqualityComparer<SeedAmountData>.Default.Equals(IridiumQualityInput, config.IridiumQualityInput);
        }

        public SeedAmountData GetAmountForQuality(int quality)
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

        public SeedAmountData GetAmountForQuality(string quality)
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

        internal class SeedAmountData
        {
            public int MinAmount { get; set; }
            public int MaxAmount { get; set; }
            public bool AllowMixed { get; set; }
            public bool AllowAncient { get; set; }

            public SeedAmountData(int min, int max, bool allowMixed, bool allowAncient)
            {
                MinAmount = min;
                MaxAmount = max;
                AllowMixed = allowMixed;
                AllowAncient = allowAncient;
            }

            public override bool Equals(object? obj)
            {
                return obj is SeedAmountData data &&
                       MinAmount == data.MinAmount &&
                       MaxAmount == data.MaxAmount &&
                       AllowMixed == data.AllowMixed &&
                       AllowAncient == data.AllowAncient;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(MinAmount, MaxAmount, AllowMixed, AllowAncient);
            }
        }
    }
}
