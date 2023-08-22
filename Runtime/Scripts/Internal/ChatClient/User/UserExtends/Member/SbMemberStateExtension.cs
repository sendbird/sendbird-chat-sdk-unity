// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbMemberStateExtension
    {
        private static int _enumCount = 0;

        internal static SbMemberState JsonNameToType(string inJsonName)
        {
            if (string.IsNullOrEmpty(inJsonName) == false)
            {
                if (_enumCount <= 0)
                    _enumCount = Enum.GetValues(typeof(SbMemberState)).Length;

                for (SbMemberState enumType = 0; enumType < (SbMemberState)_enumCount; enumType++)
                {
                    if (enumType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                        return enumType;
                }

                Logger.Warning(Logger.CategoryType.Command, $"SbCountPreferenceExtension::ToJsonName Invalid type name:{inJsonName}");
            }

            return SbMemberState.None;
        }
    }
}