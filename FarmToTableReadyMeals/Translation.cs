using StardewModdingAPI;

namespace FarmToTableReadyMeals
{
    class Translation
    {
        private static ITranslationHelper i18n = ModEntry.Helper.Translation;

        internal static string GetString(string key)
        {
            return i18n.Get(key);
        }
        internal static string GetString(string key, object tokens)
        {
            return i18n.Get(key, tokens);
        }
    }
}
