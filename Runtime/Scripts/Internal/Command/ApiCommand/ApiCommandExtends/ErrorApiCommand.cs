// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    internal sealed class ErrorApiCommand
    {
        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            private const string JSON_KEY_ERROR = "error";
            private const string JSON_KEY_CODE = "code";

            [JsonProperty(JSON_KEY_CODE)] internal readonly int errorCode = (int)SbErrorCode.UnknownError;
            [JsonProperty(JSON_KEY_ERROR)] internal readonly bool isError = false;
            [JsonProperty("message")] internal readonly string errorMessage = null;
            [JsonProperty("request_id")] internal readonly string requestId = null;
            [JsonProperty("type")] internal readonly string commandType = null;

            internal bool IsError()
            {
                return isError;
            }

            internal SbErrorCode GetErrorCode()
            {
                return (SbErrorCode)errorCode;
            }

            internal string GetMessage()
            {
                return errorMessage;
            }

            internal static Response TryConvertJsonToResponse(JObject inJObject)
            {
                if (inJObject == null || (inJObject.ContainsKey(JSON_KEY_ERROR) == false && inJObject.ContainsKey(JSON_KEY_CODE) == false))
                    return null;

                return inJObject.ToObjectIgnoreException<Response>();
            }
        }
    }
}