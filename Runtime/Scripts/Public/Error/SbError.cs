// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// <summary>
    /// Custom Error class for SendbirdChat.
    /// </summary>
    /// @since 4.0.0
    public partial class SbError
    {
        /// <summary>
        /// Error Code that represents the type of the error.
        /// </summary>
        public SbErrorCode ErrorCode { get; }
        /// <summary>
        /// Error message.
        /// </summary>
        public string ErrorMessage { get; }
    }
}