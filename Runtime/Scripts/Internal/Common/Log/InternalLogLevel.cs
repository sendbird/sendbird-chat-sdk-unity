// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal enum InternalLogLevel
    {
        [JsonName("verbose")] Verbose,
        [JsonName("debug")] Debug,
        [JsonName("info")] Info,
        [JsonName("warning")] Warning,
        [JsonName("error")] Error,
        [JsonName("none")] None
    }
}