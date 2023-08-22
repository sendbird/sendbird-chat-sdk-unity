// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public abstract class SbSessionHandler
    {
        /// <summary>
        /// App needs to fetch a new token.
        /// </summary>
        /// <param name="inSessionTokenRequester"></param>
        /// @since 4.0.0
        public abstract void OnSessionTokenRequired(ISbSessionTokenRequester inSessionTokenRequester);

        /// <summary>
        /// Called when the SDK can’t refresh the session. App should force a user to a login page to connect again.
        /// </summary>
        /// @since 4.0.0
        public abstract void OnSessionClosed();

        /// <summary>
        /// Called when the SDK run into an error while refreshing the session key.
        /// </summary>
        /// <param name="inError"></param>
        /// @since 4.0.0
        public virtual void OnSessionError(SbError inError) { }

        /// <summary>
        /// Called when SDK refreshed the session key.
        /// </summary>
        /// @since 4.0.0
        public virtual void OnSessionRefreshed() { }
    }
}