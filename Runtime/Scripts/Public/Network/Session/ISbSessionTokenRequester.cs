// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public interface ISbSessionTokenRequester
    {
        /// <summary>
        /// Call this block method when failed to retrieve a new token.
        /// </summary>
        /// @since 4.0.0
        void OnFail();

        /// <summary>
        /// Call this block method after retrieving a new token. In case when app decides not to refresh the session for this user, they should call this with nil.
        /// </summary>
        /// <param name="inNewToken"></param>
        /// @since 4.0.0
        void OnSuccess(string inNewToken);
    }
}