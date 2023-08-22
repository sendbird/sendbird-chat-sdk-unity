// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbChannelTypeExtension
    {
        private static int _enumCount = 0;

        internal static SbChannelType JsonNameToType(string inJsonName)
        {
            if (string.IsNullOrEmpty(inJsonName) == false)
            {
                if (_enumCount <= 0)
                    _enumCount = Enum.GetValues(typeof(SbChannelType)).Length;

                for (SbChannelType channelType = 0; channelType < (SbChannelType)_enumCount; channelType++)
                {
                    if (channelType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                        return channelType;
                }

                Logger.Warning(Logger.CategoryType.Command, $"SbChannelTypeExtension::ToJsonName Invalid type name:{inJsonName}");
            }

            Logger.Warning(Logger.CategoryType.Command, $"SbChannelTypeExtension::ToJsonName type name is null.");
            return SbChannelType.Group;
        }
    }
}