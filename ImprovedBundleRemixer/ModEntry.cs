using StardewModdingAPI;
using System.Collections.Generic;

namespace ImprovedBundleRemixer
{
    public class ModEntry : Mod
    {
        internal static new IModHelper Helper;
        internal static new IMonitor Monitor;

        internal static string BundleDatafile = "BundleData.json";

        public override void Entry(IModHelper helper)
        {
            ModEntry.Helper = helper;
            ModEntry.Monitor = base.Monitor;

            helper.Content.AssetLoaders.Add(new RemixedBundleAssetReplacer());

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;

            Logger.LogInfo("Ready to mix bundles!");
        }

        private void GameLoop_GameLaunched(object sender, StardewModdingAPI.Events.GameLaunchedEventArgs e)
        {
            //Read data file, and create a default
            List<CustomBundleData> var = Helper.Data.ReadJsonFile<List<CustomBundleData>>(BundleDatafile);
            if (var is null || var.Count == 0)
            {
                var = DefaultBundleData.GetDefaults();
                Helper.Data.WriteJsonFile(BundleDatafile, var);
            }
        }
    }

    public class RemixedBundleAssetReplacer : IAssetLoader
    {
        public bool CanLoad<T>(IAssetInfo asset) => asset.AssetNameEquals("Data\\RandomBundles");

        public T Load<T>(IAssetInfo asset) => ModEntry.Helper.Content.Load<T>(ModEntry.BundleDatafile);
    }
}
