// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System;
using System.Reflection;

namespace Sendbird.Chat
{
    internal class JsonNameAttribute : Attribute
    {
        internal string JsonName { get; }

        internal JsonNameAttribute(string inJsonName)
        {
            JsonName = inJsonName;
        }
    }
}