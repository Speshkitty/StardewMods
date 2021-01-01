using StardewModdingAPI;

namespace CustomiseChildBedroom
{
    class Translation
    {
        private static ITranslationHelper i18n = ModEntry.Helper.Translation;

        internal static string GetString(string key) => i18n.Get(key);
        
        internal static string GetString(string key, object tokens) => i18n.Get(key, tokens);
        
    }
}
