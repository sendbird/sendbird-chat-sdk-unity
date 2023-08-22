// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbThreadInfoUpdateEvent
    {
        private readonly string _channelUrl;
        private readonly SbChannelType _channelType;
        private readonly long _targetMessageId;
        private readonly SbThreadInfo _threadInfo;


        internal SbThreadInfoUpdateEvent(string inChannelUrl, SbChannelType inChannelType, long inTargetMessageId, ThreadInfoDto inThreadInfoDto, SendbirdChatMainContext inChatMainContext)
        {
            _channelUrl = inChannelUrl;
            _channelType = inChannelType;
            _targetMessageId = inTargetMessageId;
            if (inThreadInfoDto != null)
                _threadInfo = new SbThreadInfo(inThreadInfoDto, inChatMainContext);
        }
    }
}