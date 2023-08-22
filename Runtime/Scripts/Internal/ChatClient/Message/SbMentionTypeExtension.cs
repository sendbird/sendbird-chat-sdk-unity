// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbMentionTypeExtension
    {
        private static int _enumCount = 0;

        internal static SbMentionType JsonNameToType(string inJsonName)
        {
            if (string.IsNullOrEmpty(inJsonName) == false)
            {
                if (_enumCount <= 0)
                    _enumCount = Enum.GetValues(typeof(SbMentionType)).Length;

                for (SbMentionType mentionType = 0; mentionType < (SbMentionType)_enumCount; mentionType++)
                {
                    if (mentionType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                        return mentionType;
                }

                Logger.Warning(Logger.CategoryType.Command, $"SbMentionTypeExtension::ToJsonName Invalid type name:{inJsonName}");
            }

            return SbMentionType.Channel;
        }
    }
}