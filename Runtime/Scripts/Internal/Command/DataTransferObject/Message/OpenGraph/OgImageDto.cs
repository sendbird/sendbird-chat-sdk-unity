// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class OgImageDto
    {
#pragma warning disable CS0649
        [JsonProperty("url")] internal readonly string url;
        [JsonProperty("secure_url")] internal readonly string secureUrl;
        [JsonProperty("type")] internal readonly string type;
        [JsonProperty("alt")] internal readonly string alt;
        [JsonProperty("width")] internal readonly int width;
        [JsonProperty("height")] internal readonly int height;
#pragma warning restore CS0649
    }
}