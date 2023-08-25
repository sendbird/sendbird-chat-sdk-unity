// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    internal enum HttpMethodType
    {
        Get,
        Post,
        Put,
        Delete,
    }

    internal static class HttpMethodTypeExtension
    {
        internal static string ToMethodString(this HttpMethodType inHttpMethodType)
        {
            switch (inHttpMethodType)
            {
                case HttpMethodType.Get:    return "GET";
                case HttpMethodType.Post:   return "POST";
                case HttpMethodType.Put:    return "PUT";
                case HttpMethodType.Delete: return "DELETE";
            }
            
            Logger.Warning(Logger.CategoryType.Http, $"HttpMethodTypeExtension::ToMethodString Have to add type:{inHttpMethodType}");
            return string.Empty;
        }
    }
}