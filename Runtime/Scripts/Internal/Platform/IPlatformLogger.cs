// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal interface IPlatformLogger
    {
        void Error(string inMessage);
        void Warning(string inMessage);
        void Info(string inMessage);
        void Debug(string inMessage);
        void Verbose(string inMessage);
    }
}