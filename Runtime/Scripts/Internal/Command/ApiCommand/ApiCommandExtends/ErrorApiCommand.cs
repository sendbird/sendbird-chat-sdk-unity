// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class ErrorApiCommand
    {
        internal sealed class Response : ApiCommandAbstract.Response
        {
            private const string JSON_KEY_ERROR = "error";
            private const string JSON_KEY_CODE = "code";

            internal int errorCode = (int)SbErrorCode.UnknownError;
            internal bool isError = false;
            internal string errorMessage = null;
            internal string requestId = null;
            internal string commandType = null;

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

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return;

                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inJsonString))
                {
                    reader.Read();
                    if (reader.TokenType != JsonToken.StartObject)
                        return;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.EndObject)
                            break;

                        string propName = reader.Value as string;
                        reader.Read();
                        switch (propName)
                        {
                            case "code": errorCode = JsonStreamingHelper.ReadInt(reader); break;
                            case "error": isError = JsonStreamingHelper.ReadBool(reader); break;
                            case "message": errorMessage = JsonStreamingHelper.ReadString(reader); break;
                            case "request_id": requestId = JsonStreamingHelper.ReadString(reader); break;
                            case "type": commandType = JsonStreamingHelper.ReadString(reader); break;
                            default: JsonStreamingHelper.SkipValue(reader); break;
                        }
                    }
                }
            }

            internal static Response TryConvertJsonToResponse(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return null;

                if (inJsonString.Contains(JSON_KEY_ERROR) == false && inJsonString.Contains(JSON_KEY_CODE) == false)
                    return null;

                Response response = new Response();
                response.OnResponseAfterDeserialize(inJsonString);
                return response;
            }
        }
    }
}