// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Text;
using UnityEngine;

namespace Sendbird.Chat
{
    internal static class Logger
    {
        internal enum CategoryType
        {
            Common,
            Connection,
            Http,
            WebSocket,
            Collection,
            Command,
            Message,
            Channel,
            User,
            Json,
            Rtc,
        }

        internal static InternalLogLevel InternalLogLevel { get; private set; } = InternalLogLevel.None;
        private static readonly StringBuilder _tempStringBuilder = new StringBuilder();

        internal static void Error(CategoryType inCategoryType, string inMessage)
        {
            if (InternalLogLevel <= InternalLogLevel.Error)
            {
                _tempStringBuilder.Clear();
                _tempStringBuilder.Append($"[Sb{inCategoryType}_{Time.frameCount}]{inMessage}");
                UnityEngine.Debug.LogError(_tempStringBuilder);
            }
        }

        internal static void Warning(CategoryType inCategoryType, string inMessage)
        {
            if (InternalLogLevel <= InternalLogLevel.Warning)
            {
                _tempStringBuilder.Clear();
                _tempStringBuilder.Append($"[Sb{inCategoryType}_{Time.frameCount}]{inMessage}");
                UnityEngine.Debug.LogWarning(_tempStringBuilder);
            }
        }

        internal static void Info(CategoryType inCategoryType, string inMessage)
        {
            if (InternalLogLevel <= InternalLogLevel.Info)
            {
                _tempStringBuilder.Clear();
                _tempStringBuilder.Append($"[Sb{inCategoryType}_{Time.frameCount}]{inMessage}");
                UnityEngine.Debug.Log(_tempStringBuilder);
            }
        }

        internal static void Debug(CategoryType inCategoryType, string inMessage)
        {
            if (InternalLogLevel <= InternalLogLevel.Debug)
            {
                _tempStringBuilder.Clear();
                _tempStringBuilder.Append($"[Sb{inCategoryType}_{Time.frameCount}]{inMessage}");
                UnityEngine.Debug.Log(_tempStringBuilder);
            }
        }

        internal static void Verbose(CategoryType inCategoryType, string inMessage)
        {
            if (InternalLogLevel <= InternalLogLevel.Verbose)
            {
                _tempStringBuilder.Clear();
                _tempStringBuilder.Append($"[Sb{inCategoryType}_{Time.frameCount}]{inMessage}");
                UnityEngine.Debug.Log(_tempStringBuilder);
            }
        }

        internal static void SendLogsToServer(InternalLogLevel inInternalLogLevel)
        {
            //disable send to server because experimental
            //_logCollector.Flush(logLevelType);
        }

        internal static void SetLogLevel(InternalLogLevel inInternalLogLevel)
        {
            InternalLogLevel = inInternalLogLevel;
        }
    }
}