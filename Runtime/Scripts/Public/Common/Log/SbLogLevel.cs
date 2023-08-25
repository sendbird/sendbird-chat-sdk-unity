// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Logger Level enumeration.
    /// Log will not be exposed if the priority value is lower than the configured log level. Logger Level follows the following priority.
    /// </summary>
    /// @since 4.0.0
    public enum SbLogLevel
    {
        /// <summary>
        /// Verbose level. Once set to this level, all logs will be printed.
        /// </summary>
        /// @since 4.0.0
        Verbose,

        /// <summary>
        /// Debug level. Once set to this level, all logs except verbose level logs will be printed.
        /// </summary>
        /// @since 4.0.0
        Debug,

        /// <summary>
        /// Info level. Once set to this level, all logs except verbose, and debug level logs will be printed.
        /// </summary>
        /// @since 4.0.0
        Info,

        /// <summary>
        /// Warn level. Once set to this level, only warn level logs and error level logs will be printed.
        /// </summary>
        /// @since 4.0.0
        Warning,

        /// <summary>
        /// Error level. Once set to this level, only error level logs will be printed.
        /// </summary>
        /// @since 4.0.0
        Error,

        /// <summary>
        /// None level. Once set to this level, no logs will be printed.
        /// </summary>
        /// @since 4.0.0
        None
    }
}