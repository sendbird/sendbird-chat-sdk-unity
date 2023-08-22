// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbGroupChannelPushTriggerOptionExtension
    {
        private static int _enumCount = 0;

        internal static SbGroupChannelPushTriggerOption JsonNameToType(string inJsonName)
        {
            if (string.IsNullOrEmpty(inJsonName) == false)
            {
                if (_enumCount <= 0)
                    _enumCount = Enum.GetValues(typeof(SbGroupChannelPushTriggerOption)).Length;

                for (SbGroupChannelPushTriggerOption enumType = 0; enumType < (SbGroupChannelPushTriggerOption)_enumCount; enumType++)
                {
                    if (enumType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                        return enumType;
                }

                Logger.Warning(Logger.CategoryType.Command, $"SbGroupChannelPushTriggerOptionExtension::ToJsonName Invalid type name:{inJsonName}");
            }

            return SbGroupChannelPushTriggerOption.Default;
        }
    }
}