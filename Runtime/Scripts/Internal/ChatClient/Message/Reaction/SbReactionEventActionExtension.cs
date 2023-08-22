// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbReactionEventActionExtension
    {
        private static int _enumCount = 0;

        internal static SbReactionEventAction JsonNameToType(string inJsonName)
        {
            if (string.IsNullOrEmpty(inJsonName) == false)
            {
                if (_enumCount <= 0)
                    _enumCount = Enum.GetValues(typeof(SbReactionEventAction)).Length;

                for (SbReactionEventAction enumType = 0; enumType < (SbReactionEventAction)_enumCount; enumType++)
                {
                    if (enumType.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                        return enumType;
                }

                Logger.Warning(Logger.CategoryType.Command, $"SbReactionEventActionExtension::ToJsonName Invalid type name:{inJsonName}");
            }

            return SbReactionEventAction.Delete;
        }
    }
}