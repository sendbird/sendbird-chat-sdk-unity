// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbPushTriggerOptionExtension
    {
        private static int _enumCount = 0;

        internal static SbPushTriggerOption? JsonNameToType(string inJsonName)
        {
            if (string.IsNullOrEmpty(inJsonName) == false)
            {
                if (_enumCount <= 0)
                    _enumCount = Enum.GetValues(typeof(SbPushTriggerOption)).Length;

                for (SbPushTriggerOption enumType = 0; enumType < (SbPushTriggerOption)_enumCount; enumType++)
                {
                    if (enumType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                        return enumType;
                }

                Logger.Warning(Logger.CategoryType.Command, $"SbPushTriggerOption::ToJsonName Invalid type name:{inJsonName}");
            }

            return null;
        }
    }
}