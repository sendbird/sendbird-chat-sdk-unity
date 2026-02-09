//
//  Copyright (c) 2022 Sendbird, Inc.
//

using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sendbird.Chat
{
    internal static class NewtonsoftJsonExtension
    {
        private static readonly JsonSerializerSettings JSON_SERIALIZER_SETTINGS = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Error = OnSerializerSettingsErrorHandler
        };

        private static readonly JsonSerializer JSON_SERIALIZER = new JsonSerializer
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        internal static string SerializeObjectIgnoreException(object inJsonObject)
        {
            try
            {
                return JsonConvert.SerializeObject(inJsonObject, JSON_SERIALIZER_SETTINGS);
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Json, $"DeserializeObject exception:{exception.Message}");
            }

            return default;
        }

        internal static TObject DeserializeObjectIgnoreException<TObject>(string inJsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<TObject>(inJsonString, JSON_SERIALIZER_SETTINGS);
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Json, $"DeserializeObject exception:{exception.Message}");
            }

            return default;
        }

        internal static object DeserializeObjectIgnoreException(string inJsonString, Type inType)
        {
            try
            {
                return JsonConvert.DeserializeObject(inJsonString, inType, JSON_SERIALIZER_SETTINGS);
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Json, $"DeserializeObject exception:{exception.Message}");
            }

            return default;
        }

        internal static TObject ToObjectIgnoreException<TObject>(this JObject inJObject)
        {
            try
            {
                return inJObject.ToObject<TObject>(JSON_SERIALIZER);
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Json, $"DeserializeObject exception:{exception.Message}");
            }

            return default;
        }

        internal static object ToObjectIgnoreException(this JObject inJObject, Type inType)
        {
            try
            {
                return inJObject.ToObject(inType, JSON_SERIALIZER);
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Json, $"DeserializeObject exception:{exception.Message}");
            }

            return default;
        }

        internal static TObject ToPropertyValueIgnoreException<TObject>(this JObject inJObject, string inPropertyName, TObject inDefaultIfFailed = default)
        {
            try
            {
                if (inJObject.TryGetValue(inPropertyName, out JToken token))
                {
                    return token.ToObject<TObject>(JSON_SERIALIZER);
                }
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Json, $"DeserializeObject exception:{exception.Message}");
            }

            return inDefaultIfFailed;
        }

        private static void OnSerializerSettingsErrorHandler(object inSender, Newtonsoft.Json.Serialization.ErrorEventArgs inErrorEventArgs)
        {
            Logger.Warning(Logger.CategoryType.Json, $"OnSerializerSettingsErrorHandler exception:{inErrorEventArgs.ErrorContext.Error.Message}");
            inErrorEventArgs.ErrorContext.Handled = true;
        }

        internal static JObject ParseToJObjectIgnoreException(string inJsonString)
        {
            if (string.IsNullOrEmpty(inJsonString))
            {
                return null;
            }

            try
            {
                return JObject.Parse(inJsonString);
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Json, $"ParseToJObjectIgnoreException exception:{exception.Message}");
            }

            return null;
        }

        internal static string ExtractTypeField(string inJsonString)
        {
            if (string.IsNullOrEmpty(inJsonString))
            {
                return null;
            }
            
            using (StringReader stringReader = new StringReader(inJsonString))
            using (JsonTextReader jsonReader = new JsonTextReader(stringReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName &&
                        string.Equals(jsonReader.Value as string, "type", StringComparison.Ordinal))
                    {
                        if (jsonReader.Read() && jsonReader.TokenType == JsonToken.String)
                        {
                            return jsonReader.Value as string;
                        }
                    }
                }
            }

            return null;
        }
    }
}
