// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbPushTokenTypeExtension
    {
        private static int _enumCount = 0;

        internal static SbPushTokenType JsonNameToType(string inJsonName)
        {
            if (string.IsNullOrEmpty(inJsonName) == false)
            {
                if (_enumCount <= 0)
                    _enumCount = Enum.GetValues(typeof(SbMentionType)).Length;

                for (SbPushTokenType mentionType = 0; mentionType < (SbPushTokenType)_enumCount; mentionType++)
                {
                    if (mentionType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                        return mentionType;
                }

                Logger.Warning(Logger.CategoryType.Command, $"SbMentionTypeExtension::ToJsonName Invalid type name:{inJsonName}");
            }

            return SbPushTokenType.Fcm;
        }
    }
}