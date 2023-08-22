// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbRoleExtension
    {
        private static int _enumCount = 0;

        internal static SbRole JsonNameToType(string inJsonName)
        {
            if (_enumCount <= 0)
                _enumCount = Enum.GetValues(typeof(SbRole)).Length;

            for (SbRole roleType = 0; roleType < (SbRole)_enumCount; roleType++)
            {
                if (roleType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                    return roleType;
            }

            Logger.Warning(Logger.CategoryType.Command, $"SbRoleExtension::ToJsonName Invalid type name:{inJsonName}");
            return SbRole.None;
        }
    }
}