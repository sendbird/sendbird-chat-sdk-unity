// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    public class ThumbnailDto
    {
#pragma warning disable CS0649
        [JsonProperty("url")] internal readonly string url;
        [JsonProperty("width")] internal readonly int width;
        [JsonProperty("height")] internal readonly int height;
        [JsonProperty("real_width")] internal readonly int realWidth;
        [JsonProperty("real_height")] internal readonly int realHeight;
#pragma warning restore CS0649
    }
}