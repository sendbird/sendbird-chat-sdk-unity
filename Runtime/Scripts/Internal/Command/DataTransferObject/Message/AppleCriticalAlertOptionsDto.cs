// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class AppleCriticalAlertOptionsDto
    {
#pragma warning disable CS0649
        [JsonProperty("critical_sound")] internal readonly string name;
        [JsonProperty("volume")] internal readonly double volume;
#pragma warning restore CS0649

        internal AppleCriticalAlertOptionsDto(SbAppleCriticalAlertOptions inSbAppleCriticalAlertOptions)
        {
            name = inSbAppleCriticalAlertOptions.Name;
            volume = inSbAppleCriticalAlertOptions.Volume;
        }
    }
}