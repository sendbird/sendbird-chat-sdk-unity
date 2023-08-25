// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbBaseChannel
    {
        private readonly string _url;
        private string _coverUrl;
        private SbUser _creator;
        private long _createdAt;
        private string _data;
        private bool _isEphemeral;
        private bool _isFrozen;
        private string _name;
        private string _customType;
        private readonly Dictionary<string, string> _cachedMetaDataByKey = new Dictionary<string, string>();
        private protected readonly SendbirdChatMainContext chatMainContextRef = null;
        internal long LastSyncedChangeLogsTimestamp { get; private set; }

        internal SbBaseChannel(string inChannelUrl, SendbirdChatMainContext inChatMainContext)
        {
            _url = inChannelUrl;
            chatMainContextRef = inChatMainContext;
        }

        internal void ResetFromChannelDto(BaseChannelDto inBaseChannelDto)
        {
            if (inBaseChannelDto == null)
            {
                Logger.Warning(Logger.CategoryType.Channel, "SbBaseChannel::ResetFromChannelDto() Command object is null.");
                return;
            }

            _name = inBaseChannelDto.name;
            _coverUrl = inBaseChannelDto.coverUrl;
            _createdAt = inBaseChannelDto.createdAt;
            _data = inBaseChannelDto.data;
            _isEphemeral = inBaseChannelDto.isEphemeral;
            _isFrozen = inBaseChannelDto.isFrozen;
            _customType = inBaseChannelDto.customType;
            LastSyncedChangeLogsTimestamp = inBaseChannelDto.lastSyncedChangeLogTimeStamp;
            if (inBaseChannelDto.metaData != null && 0 < inBaseChannelDto.metaData.Count)
            {
                InsertAllMetaData(inBaseChannelDto.metaData);
            }

            if (inBaseChannelDto.createdByUserDto != null)
            {
                _creator = new SbUser(inBaseChannelDto.createdByUserDto, chatMainContextRef);
            }

            OnResetFromChannelDto(inBaseChannelDto);
        }

        private protected abstract void OnResetFromChannelDto(BaseChannelDto inBaseChannelDto);

        protected abstract SbRole GetCurrentUserRole();
    }
}