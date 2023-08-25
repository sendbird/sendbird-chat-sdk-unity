// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal static class DictionaryExtension
    {
        internal static void ForEachByValue<TKeyType, TValueType>(this Dictionary<TKeyType, TValueType> inDictionary, Action<TValueType> inAction)
        {
            if (inAction == null)
                return;

            foreach (TValueType value in inDictionary.Values)
            {
                inAction.Invoke(value);
            }
        }

        internal static bool AddIfNotContains<TKeyType, TValueType>(this Dictionary<TKeyType, TValueType> inDictionary, TKeyType inKey, TValueType inValue)
        {
            if (inKey == null)
                return false;

            if (inDictionary.ContainsKey(inKey) == false)
            {
                inDictionary.Add(inKey, inValue);
                return true;
            }

            return false;
        }

        internal static bool RemoveIfContains<TKeyType, TValueType>(this Dictionary<TKeyType, TValueType> inDictionary, TKeyType inKey)
        {
            if (inKey == null)
                return false;

            if (inDictionary.ContainsKey(inKey))
            {
                inDictionary.Remove(inKey);
                return true;
            }

            return false;
        }

        internal static bool Remove<TKeyType, TValueType>(this Dictionary<TKeyType, TValueType> inDictionary, TKeyType inKey)
        {
            if (inKey == null)
                return false;

            inDictionary.Remove(inKey);
            return true;
        }
    }
}