// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbGroupChannelHiddenStateExtension
    {
        private static int _enumCount = 0;

        internal static SbGroupChannelHiddenState JsonNameToType(string inJsonName)
        {
            if (string.IsNullOrEmpty(inJsonName) == false)
            {
                if (_enumCount <= 0)
                    _enumCount = Enum.GetValues(typeof(SbGroupChannelHiddenState)).Length;

                for (SbGroupChannelHiddenState enumType = 0; enumType < (SbGroupChannelHiddenState)_enumCount; enumType++)
                {
                    if (enumType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                        return enumType;
                }

                Logger.Warning(Logger.CategoryType.Command, $"SbCountPreferenceExtension::ToJsonName Invalid type name:{inJsonName}");
            }

            return SbGroupChannelHiddenState.Unhidden;
        }
    }
}