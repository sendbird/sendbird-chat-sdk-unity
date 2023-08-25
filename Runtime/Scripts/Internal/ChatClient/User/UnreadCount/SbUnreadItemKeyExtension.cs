// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbUnreadItemKeyExtension
    {
        private static int _enumCount = 0;

        internal static SbUnreadItemKey JsonNameToType(string inJsonName)
        {
            if (_enumCount <= 0)
                _enumCount = Enum.GetValues(typeof(SbUnreadItemKey)).Length;

            for (SbUnreadItemKey enumType = 0; enumType < (SbUnreadItemKey)_enumCount; enumType++)
                if (enumType.ToJsonName().Equals(inJsonName))
                    return enumType;

            return default;
        }
    }
}