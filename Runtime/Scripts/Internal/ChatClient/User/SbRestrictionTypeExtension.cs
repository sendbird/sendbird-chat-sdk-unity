// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbRestrictionTypeExtension
    {
        private static int _enumCount = 0;

        internal static SbRestrictionType JsonNameToType(string inJsonName)
        {
            if (_enumCount <= 0)
                _enumCount = Enum.GetValues(typeof(SbRestrictionType)).Length;

            for (SbRestrictionType roleType = 0; roleType < (SbRestrictionType)_enumCount; roleType++)
            {
                if (roleType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                    return roleType;
            }

            Logger.Warning(Logger.CategoryType.Command, $"SbRestrictionTypeExtension::ToJsonName Invalid type name:{inJsonName}");
            return SbRestrictionType.Muted;
        }
    }
}