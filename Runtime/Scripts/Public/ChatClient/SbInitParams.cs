// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Represents a params used in SendbirdChat.Init().
    /// </summary>
    /// @since 4.0.0
    public class SbInitParams
    {
        private const int MAX_APP_VERSION_LENGTH = 30;

        /// <summary>
        /// Sendbird Application ID.
        /// </summary>
        public string ApplicationId { get; }

        /// <summary>
        /// The log level for SendbirdChat SDK. Default is [LogLevel.Warning].
        /// </summary>
        public SbLogLevel SbLogLevel { get; }

        /// <summary>
        /// The version of the application.
        /// The version string will be trimmed if the length is longer than 30.
        /// </summary>
        public string AppVersion { get; }

        /// <summary>
        /// Parameter constructor
        /// </summary>
        /// <param name="inApplicationId">Sendbird Application ID</param>
        /// <param name="inLogLevel">log level</param>
        /// <param name="inAppVersion">host app version</param>
        public SbInitParams(string inApplicationId, SbLogLevel inLogLevel = SbLogLevel.Warning, string inAppVersion = "")
        {
            ApplicationId = inApplicationId;
            SbLogLevel = inLogLevel;
            if (string.IsNullOrEmpty(inAppVersion))
            {
                AppVersion = string.Empty;
            }
            else
            {
                AppVersion = inAppVersion;
            }

            if (MAX_APP_VERSION_LENGTH < AppVersion.Length)
            {
                AppVersion = AppVersion.Substring(0, MAX_APP_VERSION_LENGTH);
            }
        }
    }
}