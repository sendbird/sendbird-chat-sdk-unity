//
//  Copyright (c) 2026 Sendbird, Inc.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal static class JsonStreamingHelper
    {
        // --- Read helpers ---

        internal static string ReadRawJsonString(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType == JsonToken.StartObject ||
                inReader.TokenType == JsonToken.StartArray)
            {
                StringWriter stringWriter = new StringWriter();
                JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
                jsonTextWriter.WriteToken(inReader);
                jsonTextWriter.Flush();
                return stringWriter.ToString();
            }

            return inReader.Value?.ToString();
        }

        internal static string ReadString(JsonTextReader inReader)
        {
            return inReader.Value as string;
        }

        internal static long ReadLong(JsonTextReader inReader)
        {
            if (inReader.Value == null)
                return 0;

            if (inReader.Value is long l) return l;
            if (inReader.Value is int i) return i;
            if (inReader.Value is double d) return (long)d;

            if (long.TryParse(inReader.Value.ToString(), out long result))
                return result;

            Logger.Warning(Logger.CategoryType.Command, $"JsonStreamingHelper.ReadLong failed to parse: {inReader.Value}");
            return 0;
        }

        internal static long? ReadNullableLong(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null || inReader.Value == null)
                return null;

            if (inReader.Value is long l) return l;
            if (inReader.Value is int i) return i;
            if (inReader.Value is double d) return (long)d;

            if (long.TryParse(inReader.Value.ToString(), out long result))
                return result;

            Logger.Warning(Logger.CategoryType.Command, $"JsonStreamingHelper.ReadNullableLong failed to parse: {inReader.Value}");
            return null;
        }

        internal static int ReadInt(JsonTextReader inReader)
        {
            if (inReader.Value == null)
                return 0;

            if (inReader.Value is int i) return i;
            if (inReader.Value is long l) return (int)l;
            if (inReader.Value is double d) return (int)d;

            if (int.TryParse(inReader.Value.ToString(), out int result))
                return result;

            Logger.Warning(Logger.CategoryType.Command, $"JsonStreamingHelper.ReadInt failed to parse: {inReader.Value}");
            return 0;
        }

        internal static int? ReadNullableInt(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null || inReader.Value == null)
                return null;

            if (inReader.Value is int i) return i;
            if (inReader.Value is long l) return (int)l;
            if (inReader.Value is double d) return (int)d;

            if (int.TryParse(inReader.Value.ToString(), out int result))
                return result;

            Logger.Warning(Logger.CategoryType.Command, $"JsonStreamingHelper.ReadNullableInt failed to parse: {inReader.Value}");
            return null;
        }

        internal static bool ReadBool(JsonTextReader inReader)
        {
            if (inReader.Value == null)
                return false;

            if (inReader.Value is bool b) return b;

            if (bool.TryParse(inReader.Value.ToString(), out bool result))
                return result;

            Logger.Warning(Logger.CategoryType.Command, $"JsonStreamingHelper.ReadBool failed to parse: {inReader.Value}");
            return false;
        }

        internal static bool? ReadNullableBool(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null || inReader.Value == null)
                return null;

            if (inReader.Value is bool b) return b;

            if (bool.TryParse(inReader.Value.ToString(), out bool result))
                return result;

            Logger.Warning(Logger.CategoryType.Command, $"JsonStreamingHelper.ReadNullableBool failed to parse: {inReader.Value}");
            return null;
        }

        internal static double ReadDouble(JsonTextReader inReader)
        {
            if (inReader.Value == null)
                return 0.0;

            if (inReader.Value is double d) return d;
            if (inReader.Value is float f) return f;
            if (inReader.Value is long l) return l;
            if (inReader.Value is int i) return i;

            if (double.TryParse(inReader.Value.ToString(), out double result))
                return result;

            Logger.Warning(Logger.CategoryType.Command, $"JsonStreamingHelper.ReadDouble failed to parse: {inReader.Value}");
            return 0.0;
        }

        internal static float ReadFloat(JsonTextReader inReader)
        {
            if (inReader.Value == null)
                return 0f;

            if (inReader.Value is float f) return f;
            if (inReader.Value is double d) return (float)d;
            if (inReader.Value is long l) return l;
            if (inReader.Value is int i) return i;

            if (float.TryParse(inReader.Value.ToString(), out float result))
                return result;

            Logger.Warning(Logger.CategoryType.Command, $"JsonStreamingHelper.ReadFloat failed to parse: {inReader.Value}");
            return 0f;
        }

        internal static List<string> ReadStringList(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<string> list = new List<string>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                list.Add(inReader.Value as string);
            }

            return list;
        }

        internal static List<long> ReadLongList(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<long> list = new List<long>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                if (inReader.Value != null)
                    list.Add(ReadLong(inReader));
            }

            return list;
        }

        internal static List<string> ReadRawJsonStringList(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<string> list = new List<string>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                if (inReader.TokenType == JsonToken.StartObject ||
                    inReader.TokenType == JsonToken.StartArray)
                {
                    StringWriter stringWriter = new StringWriter();
                    JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
                    jsonTextWriter.WriteToken(inReader);
                    jsonTextWriter.Flush();
                    list.Add(stringWriter.ToString());
                }
            }

            return list;
        }

        internal static Dictionary<string, string> ReadStringDictionary(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string key = inReader.Value as string;
                inReader.Read();
                string value = inReader.Value as string;
                if (key != null)
                    dict[key] = value;
            }

            return dict;
        }

        internal static Dictionary<string, long> ReadStringLongDictionary(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            Dictionary<string, long> dict = new Dictionary<string, long>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string key = inReader.Value as string;
                inReader.Read();
                long value = ReadLong(inReader);
                if (key != null)
                    dict[key] = value;
            }

            return dict;
        }

        internal static Dictionary<string, int> ReadStringIntDictionary(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            Dictionary<string, int> dict = new Dictionary<string, int>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string key = inReader.Value as string;
                inReader.Read();
                int value = ReadInt(inReader);
                if (key != null)
                    dict[key] = value;
            }

            return dict;
        }

        internal static Dictionary<string, List<string>> ReadStringListDictionary(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartObject)
                return null;

            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                string key = inReader.Value as string;
                inReader.Read();
                List<string> value = ReadStringList(inReader);
                if (key != null)
                    dict[key] = value;
            }

            return dict;
        }

        internal static void SkipValue(JsonTextReader inReader)
        {
            inReader.Skip();
        }

        /// <summary>
        /// Reads a JSON array of message objects and directly deserializes each into the appropriate
        /// BaseMessageDto subclass using streaming parsing. This avoids the intermediate List&lt;string&gt;
        /// accumulation of ReadRawJsonStringList, reducing peak memory from N*MessageSize to ~1*MessageSize.
        /// </summary>
        internal static List<BaseMessageDto> ReadMessageDtoListDirect(JsonTextReader inReader)
        {
            if (inReader.TokenType == JsonToken.Null)
                return null;

            if (inReader.TokenType != JsonToken.StartArray)
                return null;

            List<BaseMessageDto> list = new List<BaseMessageDto>();
            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndArray)
                    break;

                if (inReader.TokenType == JsonToken.StartObject)
                {
                    BaseMessageDto dto = ReadMessageDtoFromReader(inReader);
                    if (dto != null)
                        list.Add(dto);
                }
            }

            return list;
        }

        /// <summary>
        /// Reads a single message JSON object from the reader and returns the appropriate BaseMessageDto subclass.
        /// Scans for the "type" field to determine the DTO subclass, buffering any fields encountered before "type".
        /// Fields after "type" are read directly into the DTO without re-serialization.
        /// </summary>
        private static BaseMessageDto ReadMessageDtoFromReader(JsonTextReader inReader)
        {
            // Phase 1: Scan for "type" field, buffering pre-type fields as raw JSON.
            // In Sendbird's JSON, "type" is typically early, so the buffer stays small.
            // The large fields (message, data) come after "type" and are read directly.
            StringBuilder preTypeBuffer = null;
            StringWriter preTypeSw = null;
            JsonTextWriter preTypeJw = null;
            string messageTypeString = null;
            bool reachedEnd = false;

            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                {
                    reachedEnd = true;
                    break;
                }

                string propName = inReader.Value as string;
                inReader.Read();

                if (propName == "type")
                {
                    messageTypeString = inReader.Value as string;
                    break;
                }

                // Buffer this field as raw JSON (typically small fields like message_id)
                if (preTypeBuffer == null)
                {
                    preTypeBuffer = new StringBuilder();
                    preTypeSw = new StringWriter(preTypeBuffer);
                    preTypeJw = new JsonTextWriter(preTypeSw);
                    preTypeJw.WriteStartObject();
                }

                preTypeJw.WritePropertyName(propName);
                preTypeJw.WriteToken(inReader);
            }

            if (string.IsNullOrEmpty(messageTypeString))
            {
                SkipToEndOfObject(inReader, reachedEnd);
                return null;
            }

            // Create the appropriate DTO subclass based on message type
            WsCommandType wsCommandType = WsCommandTypeExtension.JsonNameToType(messageTypeString);
            BaseMessageDto dto = CreateMessageDtoByType(wsCommandType);
            if (dto == null)
            {
                SkipToEndOfObject(inReader, reachedEnd);
                return null;
            }

            // Phase 2a: Replay buffered pre-type fields into the DTO
            if (preTypeBuffer != null && preTypeBuffer.Length > 1)
            {
                preTypeJw.WriteEndObject();
                preTypeJw.Flush();
                string bufferedJson = preTypeBuffer.ToString();

                try
                {
                    using (JsonTextReader bufReader = JsonStreamingPool.CreateReader(bufferedJson))
                    {
                        bufReader.Read(); // StartObject
                        while (bufReader.Read())
                        {
                            if (bufReader.TokenType == JsonToken.EndObject)
                                break;

                            string propName = bufReader.Value as string;
                            bufReader.Read();

                            if (dto.TryReadSubclassField(bufReader, propName))
                                continue;
                            if (dto.TryReadBaseField(bufReader, propName))
                                continue;
                            SkipValue(bufReader);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logger.Warning(Logger.CategoryType.Command,
                        $"ReadMessageDtoFromReader: Failed to replay buffered fields: {exception.Message}");
                }
            }

            // Phase 2b: Continue reading remaining fields directly from the original reader (zero-copy for large fields)
            if (!reachedEnd)
            {
                while (inReader.Read())
                {
                    if (inReader.TokenType == JsonToken.EndObject)
                        break;

                    string propName = inReader.Value as string;
                    inReader.Read();

                    if (dto.TryReadSubclassField(inReader, propName))
                        continue;
                    if (dto.TryReadBaseField(inReader, propName))
                        continue;
                    SkipValue(inReader);
                }
            }

            dto.PostDeserialize();
            return dto;
        }

        private static BaseMessageDto CreateMessageDtoByType(WsCommandType inWsCommandType)
        {
            if (inWsCommandType == WsCommandType.UserMessage || inWsCommandType == WsCommandType.UpdateUserMessage)
                return new UserMessageDto();
            if (inWsCommandType == WsCommandType.FileMessage || inWsCommandType == WsCommandType.UpdateFileMessage)
                return new FileMessageDto();
            if (inWsCommandType == WsCommandType.AdminMessage || inWsCommandType == WsCommandType.UpdateAdminMessage)
                return new AdminMessageDto();

            return null;
        }

        private static void SkipToEndOfObject(JsonTextReader inReader, bool inAlreadyAtEnd)
        {
            if (inAlreadyAtEnd)
                return;

            while (inReader.Read())
            {
                if (inReader.TokenType == JsonToken.EndObject)
                    break;

                // Skip property name
                inReader.Read();
                SkipValue(inReader);
            }
        }

        // --- Write helpers ---

        internal static void WritePropertyIfNotNull(JsonTextWriter inWriter, string inName, string inValue)
        {
            if (inValue == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue);
        }

        internal static void WritePropertyIfNotDefault(JsonTextWriter inWriter, string inName, long inValue)
        {
            if (inValue == 0)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue);
        }

        internal static void WritePropertyIfNotDefault(JsonTextWriter inWriter, string inName, int inValue)
        {
            if (inValue == 0)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue);
        }

        internal static void WritePropertyIfNotDefault(JsonTextWriter inWriter, string inName, bool inValue)
        {
            if (inValue == false)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue);
        }

        internal static void WritePropertyIfNotDefault(JsonTextWriter inWriter, string inName, double inValue)
        {
            if (inValue == 0.0)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue);
        }

        internal static void WritePropertyIfNotDefault(JsonTextWriter inWriter, string inName, float inValue)
        {
            if (inValue == 0f)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue);
        }

        internal static void WriteProperty(JsonTextWriter inWriter, string inName, string inValue)
        {
            inWriter.WritePropertyName(inName);
            if (inValue != null)
                inWriter.WriteValue(inValue);
            else
                inWriter.WriteNull();
        }

        internal static void WriteProperty(JsonTextWriter inWriter, string inName, long inValue)
        {
            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue);
        }

        internal static void WriteProperty(JsonTextWriter inWriter, string inName, int inValue)
        {
            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue);
        }

        internal static void WriteProperty(JsonTextWriter inWriter, string inName, bool inValue)
        {
            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue);
        }

        internal static void WriteNullableInt(JsonTextWriter inWriter, string inName, int? inValue)
        {
            if (inValue == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue.Value);
        }

        internal static void WriteNullableLong(JsonTextWriter inWriter, string inName, long? inValue)
        {
            if (inValue == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue.Value);
        }

        internal static void WriteNullableBool(JsonTextWriter inWriter, string inName, bool? inValue)
        {
            if (inValue == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteValue(inValue.Value);
        }

        internal static void WriteRawJsonProperty(JsonTextWriter inWriter, string inName, string inRawJson)
        {
            if (string.IsNullOrEmpty(inRawJson))
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteRawValue(inRawJson);
        }

        internal static void WriteStringList(JsonTextWriter inWriter, string inName, List<string> inList)
        {
            if (inList == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteStartArray();
            foreach (string item in inList)
            {
                inWriter.WriteValue(item);
            }
            inWriter.WriteEndArray();
        }

        internal static void WriteLongList(JsonTextWriter inWriter, string inName, List<long> inList)
        {
            if (inList == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteStartArray();
            foreach (long item in inList)
            {
                inWriter.WriteValue(item);
            }
            inWriter.WriteEndArray();
        }

        internal static void WriteStringDictionary(JsonTextWriter inWriter, string inName, Dictionary<string, string> inDict)
        {
            if (inDict == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteStartObject();
            foreach (KeyValuePair<string, string> pair in inDict)
            {
                inWriter.WritePropertyName(pair.Key);
                if (pair.Value != null)
                    inWriter.WriteValue(pair.Value);
                else
                    inWriter.WriteNull();
            }
            inWriter.WriteEndObject();
        }

        internal static void WriteStringLongDictionary(JsonTextWriter inWriter, string inName, Dictionary<string, long> inDict)
        {
            if (inDict == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteStartObject();
            foreach (KeyValuePair<string, long> pair in inDict)
            {
                inWriter.WritePropertyName(pair.Key);
                inWriter.WriteValue(pair.Value);
            }
            inWriter.WriteEndObject();
        }

        internal static void WriteStringIntDictionary(JsonTextWriter inWriter, string inName, Dictionary<string, int> inDict)
        {
            if (inDict == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteStartObject();
            foreach (KeyValuePair<string, int> pair in inDict)
            {
                inWriter.WritePropertyName(pair.Key);
                inWriter.WriteValue(pair.Value);
            }
            inWriter.WriteEndObject();
        }

        internal static void WriteStringListDictionary(JsonTextWriter inWriter, string inName, Dictionary<string, List<string>> inDict)
        {
            if (inDict == null)
                return;

            inWriter.WritePropertyName(inName);
            inWriter.WriteStartObject();
            foreach (KeyValuePair<string, List<string>> pair in inDict)
            {
                inWriter.WritePropertyName(pair.Key);
                inWriter.WriteStartArray();
                if (pair.Value != null)
                {
                    foreach (string item in pair.Value)
                    {
                        inWriter.WriteValue(item);
                    }
                }
                inWriter.WriteEndArray();
            }
            inWriter.WriteEndObject();
        }
    }
}
