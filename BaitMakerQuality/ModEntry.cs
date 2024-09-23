using Force.DeepCloner;
using GenericModConfigMenu;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Machines;
using static BaitMakerQuality.ModConfig;

namespace BaitMakerQuality
{
    public class ModEntry : Mod
    {
        internal static ModConfig? Config { get; private set; }
        public override void Entry(IModHelper helper)
        {
            Config = LoadConfig();

            //Set up GenericModConfigMenu integration
            helper.Events.GameLoop.GameLaunched += (s, e) =>
            {
                var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
                var data = DataLoader.Machines(Game1.content);

                if (configMenu is null)
                    return;

                configMenu.Register(
                    mod: ModManifest,
                    reset: () => Config = new ModConfig(),
                    save: () => Helper.WriteConfig(Config)
                    );

                CreateConfigSection("normal", configMenu, Config.NoQualityInput);
                CreateConfigSection("silver", configMenu, Config.SilverQualityInput);
                CreateConfigSection("gold", configMenu, Config.GoldQualityInput);
                CreateConfigSection("iridium", configMenu, Config.IridiumQualityInput);
            };

            helper.Events.Content.AssetRequested += Content_AssetRequested;
        }

        private void Content_AssetRequested(object? sender, StardewModdingAPI.Events.AssetRequestedEventArgs e)
        {
            if (!e.NameWithoutLocale.IsEquivalentTo("Data/Machines")) { return; }


            e.Edit(asset =>
            {
                var data = asset.AsDictionary<string, MachineData>();

                var rules = data.Data["(BC)BaitMaker"].OutputRules;

                var outputTemplate = rules[0];

                rules.Clear();

                rules.Add(CreateOutput(outputTemplate, 0));
                rules.Add(CreateOutput(outputTemplate, 1));
                rules.Add(CreateOutput(outputTemplate, 2));
                rules.Add(CreateOutput(outputTemplate, 4));
                
            });
        }

        private MachineOutputRule CreateOutput(MachineOutputRule template, int quality)
        {
            BaitAmountData configData = Config!.GetAmountForQuality(quality);

            MachineOutputRule output = template.DeepClone();

            output.Id = $"Qual_{quality}";
            output.Triggers[0].Condition = $"ITEM_QUALITY Input {quality} {quality}";

            output.OutputItem[0].MinStack = configData.MinAmount;
            output.OutputItem[0].MaxStack = configData.MaxAmount;

            return output;
        }

        private void CreateConfigSection(string header, IGenericModConfigMenuApi configMenu, BaitAmountData quality)
        {
            configMenu.AddSectionTitle(mod: ModManifest,
                text: () => Helper.Translation.Get($"title.quality.{header}")
                );

            configMenu.AddNumberOption(mod: ModManifest,
                getValue: () => quality.MinAmount,
                setValue: value => quality.MinAmount = value,
                name: () => Helper.Translation.Get("option.amount.minimum")
                );

            configMenu.AddNumberOption(
                    mod: ModManifest,
                    getValue: () => quality.MaxAmount,
                    setValue: value => quality.MaxAmount = value,
                    name: () => Helper.Translation.Get("option.amount.maximum")
                    );
        }

        private ModConfig LoadConfig()
        {
            ModConfig ReadConfig = Helper.ReadConfig<ModConfig>();

            if (ReadConfig.Equals(new ModConfig()))
            {
                Helper.WriteConfig(ReadConfig);
            }

            return ReadConfig;
        }
    }
}