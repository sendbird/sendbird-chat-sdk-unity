// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class SbSendingStatusExtension
    {
        private static int _enumCount = 0;

        internal static SbSendingStatus JsonNameToType(string inJsonName)
        {
            if (string.IsNullOrEmpty(inJsonName) == false)
            {
                if (_enumCount <= 0)
                    _enumCount = Enum.GetValues(typeof(SbSendingStatus)).Length;

                for (SbSendingStatus sendingStatus = 0; sendingStatus < (SbSendingStatus)_enumCount; sendingStatus++)
                {
                    if (sendingStatus.ToJsonName().Equals(inJsonName, StringComparison.OrdinalIgnoreCase))
                        return sendingStatus;
                }
            }

            Logger.Warning(Logger.CategoryType.Command, $"SbSendingStatusExtension::ToJsonName Invalid type name:{inJsonName}");
            return SbSendingStatus.None;
        }
    }
}