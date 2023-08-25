// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// The SbOpenChannel class represents a open channel which is a public chat.
    /// </summary>
    /// @since 4.0.0
    public partial class SbOpenChannel : SbBaseChannel
    {
        /// <summary>
        /// 
        /// </summary>
        /// @since 4.0.0
        public override SbChannelType ChannelType => SbChannelType.Open;

        /// <summary>
        /// The operators of this channel.
        /// </summary>
        /// @since 4.0.0
        public IReadOnlyList<SbUser> Operators => _operators;

        /// <summary>
        /// The number of participants in this channel.
        /// </summary>
        /// @since 4.0.0
        public int ParticipantCount { get => _participantCount; internal set => _participantCount = value; }

        /// <summary>
        /// Enters this channel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Enter(SbErrorHandler inCompletionHandler)
        {
            EnterInternal(inCompletionHandler);
        }

        /// <summary>
        /// Exits this channel.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Exit(SbErrorHandler inCompletionHandler)
        {
            ExitInternal(inCompletionHandler);
        }

        /// <summary>
        /// Updates this channel with SbOpenChannelUpdateParams class.
        /// </summary>
        /// <param name="inChannelUpdateParams"></param>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void UpdateChannel(SbOpenChannelUpdateParams inChannelUpdateParams, SbOpenChannelCallbackHandler inCompletionHandler)
        {
            UpdateChannelInternal(inChannelUpdateParams, inCompletionHandler);
        }

        /// <summary>
        /// Deletes this channel
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Delete(SbErrorHandler inCompletionHandler)
        {
            DeleteInternal(inCompletionHandler);
        }

        /// <summary>
        /// Refreshes the channel information.
        /// </summary>
        /// <param name="inCompletionHandler"></param>
        /// @since 4.0.0
        public void Refresh(SbErrorHandler inCompletionHandler)
        {
            RefreshInternal(inCompletionHandler);
        }

        /// <summary>
        /// Returns the user is an operator or not.
        /// </summary>
        /// <param name="inUser"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public bool IsOperator(SbUser inUser)
        {
            return IsOperatorInternal(inUser?.UserId);
        }

        /// <summary>
        /// Returns the user ID is an operator or not.
        /// </summary>
        /// <param name="inUserId"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public bool IsOperator(string inUserId)
        {
            return IsOperatorInternal(inUserId);
        }

        /// <summary>
        /// Creates a participant list query for this channel.
        /// </summary>
        /// <param name="inParams"></param>
        /// <returns></returns>
        /// @since 4.0.0
        public SbParticipantListQuery CreateParticipantListQuery(SbParticipantListQueryParams inParams)
        {
            return CreateParticipantListQueryInternal(inParams);
        }
    }
}