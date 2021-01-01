using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmToTableReadyMeals
{
    static class Extensions
    {
        public static bool ContentEquals<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Dictionary<TKey, TValue> otherDictionary)
        {
            if (dictionary.Count != otherDictionary.Count)
            {
                return false;
            }
            return (otherDictionary ?? new Dictionary<TKey, TValue>())
                .OrderBy(kvp => kvp.Key)
                .SequenceEqual((dictionary ?? new Dictionary<TKey, TValue>())
                                   .OrderBy(kvp => kvp.Key));
        }
    }
}
