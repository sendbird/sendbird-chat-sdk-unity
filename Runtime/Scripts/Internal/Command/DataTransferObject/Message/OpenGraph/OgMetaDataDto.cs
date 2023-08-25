// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal class OgMetaDataDto
    {
#pragma warning disable CS0649
        [JsonProperty("og:title")] internal readonly string title;
        [JsonProperty("og:url")] internal readonly string url;
        [JsonProperty("og:description")] internal readonly string description;
        [JsonProperty("og:image")] internal readonly OgImageDto image;
#pragma warning restore CS0649
    }
}