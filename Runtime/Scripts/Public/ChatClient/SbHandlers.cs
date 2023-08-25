// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// @since 4.0.0
    public delegate void SbBaseMessageHandler(SbBaseMessage inMessage, SbError inError);

    /// @since 4.0.0
    public delegate void SbChannelInvitationPreferenceHandler(bool inAutoAccept, SbError inError);

    /// @since 4.0.0
    public delegate void SbCountHandler(int inCount, SbError inError);

    /// @since 4.0.0
    public delegate void SbDisconnectHandler();

    /// @since 4.0.0
    public delegate void SbDoNotDisturbHandler(bool inEnabled, int inStartHour, int inStartMin, int inEndHour, int inEndMin, string inTimezone, SbError inError);

    /// @since 4.0.0
    public delegate void SbErrorHandler(SbError inError);

    /// @since 4.0.0
    public delegate void SbFileMessageHandler(SbFileMessage inFileMessage, SbError inError);

    /// @since 4.0.0
    public delegate void SbGetGroupChannelHandler(SbGroupChannel inGroupChannel, bool inIsFromCache, SbError inError);

    /// @since 4.0.0
    public delegate void SbGetOpenChannelHandler(SbOpenChannel inOpenChannel, bool inIsFromCache, SbError inError);

    /// @since 4.0.0
    public delegate void SbGroupChannelCallbackHandler(SbGroupChannel inGroupChannel, SbError inError);

    /// @since 4.0.0
    public delegate void SbGroupChannelChangeLogsHandler(IReadOnlyList<SbGroupChannel> inGroupChannels, IReadOnlyList<string> inDeletedChannelUrls, bool inHasMore, string inToken, SbError inError);

    /// @since 4.0.0
    public delegate void SbGroupChannelListHandler(IReadOnlyList<SbGroupChannel> inGroupChannels, SbError inError);

    /// @since 4.0.0
    public delegate void SbMemberListHandler(IReadOnlyList<SbMember> inMembers, SbError inError);

    /// @since 4.0.0
    public delegate void SbMessageChangeLogHandler(IReadOnlyList<SbBaseMessage> inUpdatedMessages, IReadOnlyList<long> inDeletedMessageIds, bool inHasMore, string inToken, SbError inError);

    /// @since 4.0.0
    public delegate void SbMessageListHandler(IReadOnlyList<SbBaseMessage> inMessages, SbError inError);

    /// @since 4.0.0
    public delegate void SbMetaCountersHandler(IReadOnlyDictionary<string, int> inMetaCounters, SbError inError);

    /// @since 4.0.0
    public delegate void SbMetaDataHandler(IReadOnlyDictionary<string, string> inMetaData, SbError inError);

    /// @since 4.0.0
    public delegate void SbMultiProgressHandler(string inRequestId, ulong inBytesSent, ulong inTotalBytesSent, ulong inTotalBytesExpectedToSend);

    /// @since 4.0.0
    public delegate void SbMuteInfoHandler(bool inIsMuted, string inDescription, long inStartAt, long inEndAt, long inRemainingDuration, SbError inError);

    /// @since 4.0.0
    public delegate void SbSnoozePeriodHandler(bool inEnabled, long inStartTimestamp, long inEndTimestamp, SbError inError);

    /// @since 4.0.0
    public delegate void SbOpenChannelCallbackHandler(SbOpenChannel inOpenChannel, SbError inError);

    /// @since 4.0.0
    public delegate void SbOpenChannelListHandler(IReadOnlyList<SbOpenChannel> inOpenChannels, SbError inError);

    /// @since 4.0.0
    public delegate void SbPushTemplateHandler(string inTemplateName, SbError inError);

    /// @since 4.0.0
    public delegate void SbPushTokenRegistrationStatusHandler(SbPushTokenRegistrationStatus inRegistrationStatus, SbError inError);

    /// @since 4.0.0
    public delegate void SbPushTokensHandler(IReadOnlyList<string> inPushTokens, SbPushTokenType inPushTokenType, bool inHasMore, string inToken, SbError inError);

    /// @since 4.0.0
    public delegate void SbReactionEventHandler(SbReactionEvent inReactionEvent, SbError inError);

    /// @since 4.0.0
    public delegate void SbRemoveFailedMessagesHandler(IReadOnlyList<string> inRequestIds, SbError inError);

    /// @since 4.0.0
    public delegate void SbThreadedMessageListHandler(SbBaseMessage inParentMessage, IReadOnlyList<SbBaseMessage> inMessages, SbError inError);

    /// @since 4.0.0
    public delegate void SbUserHandler(SbUser inUser, SbError inError);

    /// @since 4.0.0
    public delegate void SbUserListHandler(IReadOnlyList<SbUser> inUsers, SbError inError);

    /// @since 4.0.0
    public delegate void SbUserMessageHandler(SbUserMessage inUserMessage, SbError inError);

    /// @since 4.0.0
    public delegate void SbUnreadMessageCountHandler(int inGroupChannelCount, int inFeedChannelCount, SbError inError);

    /// @since 4.0.0
    public delegate void SbUnreadItemCountHandler(IReadOnlyDictionary<SbUnreadItemKey, int> inCounts, SbError inError);
}