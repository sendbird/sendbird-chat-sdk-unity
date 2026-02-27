//
//  Copyright (c) 2026 Sendbird, Inc.
//

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal static class JsonStreamingPool
    {
        internal static JsonTextReader CreateReader(string inJsonString)
        {
            JsonTextReader reader = new JsonTextReader(new StringReader(inJsonString));
            reader.ArrayPool = JsonArrayPool.EVENT_INSTANCE;
            return reader;
        }

        internal static JsonTextReader CreateReader(string inSource, int inOffset)
        {
            TextReader textReader = inOffset > 0
                ? (TextReader)new OffsetStringReader(inSource, inOffset)
                : new StringReader(inSource);
            JsonTextReader reader = new JsonTextReader(textReader);
            reader.ArrayPool = JsonArrayPool.EVENT_INSTANCE;
            return reader;
        }

        internal static JsonTextReader CreateReader(byte[] inResponseBytes)
        {
            MemoryStream stream = new MemoryStream(inResponseBytes, writable: false);
            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
            JsonTextReader reader = new JsonTextReader(streamReader);
            reader.ArrayPool = JsonArrayPool.EVENT_INSTANCE;
            return reader;
        }

        internal static JsonTextWriter CreateWriter(StringWriter inStringWriter)
        {
            JsonTextWriter writer = new JsonTextWriter(inStringWriter);
            writer.ArrayPool = JsonArrayPool.EVENT_INSTANCE;
            return writer;
        }

        internal static T ReadIgnoreException<T>(string inJsonString, Func<JsonTextReader, T> inReadFunc)
        {
            if (string.IsNullOrEmpty(inJsonString))
                return default;

            try
            {
                using (JsonTextReader reader = CreateReader(inJsonString))
                {
                    reader.Read();
                    return inReadFunc(reader);
                }
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Command,
                    $"JsonStreamingPool.ReadIgnoreException failed: {exception.Message}");
                return default;
            }
        }

        internal static T ReadIgnoreException<T>(string inSource, int inOffset, Func<JsonTextReader, T> inReadFunc)
        {
            if (string.IsNullOrEmpty(inSource))
                return default;

            try
            {
                using (JsonTextReader reader = CreateReader(inSource, inOffset))
                {
                    reader.Read();
                    return inReadFunc(reader);
                }
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Command,
                    $"JsonStreamingPool.ReadIgnoreException failed: {exception.Message}");
                return default;
            }
        }

        internal static string WriteIgnoreException(Action<JsonTextWriter> inWriteAction)
        {
            try
            {
                using (StringWriter stringWriter = new StringWriter())
                using (JsonTextWriter writer = CreateWriter(stringWriter))
                {
                    inWriteAction(writer);
                    return stringWriter.ToString();
                }
            }
            catch (Exception exception)
            {
                Logger.Warning(Logger.CategoryType.Command,
                    $"JsonStreamingPool.WriteIgnoreException failed: {exception.Message}");
                return null;
            }
        }
    }
}
