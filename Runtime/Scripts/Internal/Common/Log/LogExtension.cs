// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;

namespace Sendbird.Chat
{
    internal static class LogExtension
    {
        private static int _enumCount = 0;
        internal static InternalLogLevel JsonNameToType(string inJsonName)
        {
            if (_enumCount <= 0)
                _enumCount = Enum.GetValues(typeof(InternalLogLevel)).Length;
            
            for (InternalLogLevel internalLogLevel = 0; internalLogLevel <= (InternalLogLevel)_enumCount; internalLogLevel++)
                if (internalLogLevel.ToJsonName().Equals(inJsonName))
                    return internalLogLevel;

            return InternalLogLevel.None;
        }
        
        internal static InternalLogLevel ToInternalLogLevel(this SbLogLevel inLogLevel)
        {
            switch (inLogLevel)
            {
                case SbLogLevel.Verbose: return InternalLogLevel.Verbose;
                case SbLogLevel.Debug:   return InternalLogLevel.Debug;
                case SbLogLevel.Info:    return InternalLogLevel.Info;
                case SbLogLevel.Warning: return InternalLogLevel.Warning;
                case SbLogLevel.Error:   return InternalLogLevel.Error;
                case SbLogLevel.None:    return InternalLogLevel.None;
            }

            return InternalLogLevel.None;
        }
    }
}