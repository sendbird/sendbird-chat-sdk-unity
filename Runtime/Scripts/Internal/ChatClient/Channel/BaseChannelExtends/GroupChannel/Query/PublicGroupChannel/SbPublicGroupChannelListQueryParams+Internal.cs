// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbPublicGroupChannelListQueryParams
    {
        private string _channelNameContainsFilter = null;
        private List<string> _channelUrlsFilter = null;
        private List<string> _customTypesFilter = null;
        private string _customTypeStartsWithFilter = null;
        private bool _includeEmptyChannel = true;
        private bool _includeFrozenChannel = true;
        private bool _includeMetaData = true;
        private int _limit = SendbirdChatMainContext.QUERY_DEFAULT_LIMIT;
        private SbPublicGroupChannelMembershipFilter _publicMembershipFilter = SbPublicGroupChannelMembershipFilter.All;
        private string _metaDataKeyFilter = null;
        private string _metaDataOrderKeyFilter = null;
        private List<string> _metaDataValuesFilter = null;
        private string _metaDataValueStartsWithFilter = null;
        private SbPublicGroupChannelListOrder _order = SbPublicGroupChannelListOrder.Chronological;
        private SbGroupChannelSuperChannelFilter _superChannelFilter = SbGroupChannelSuperChannelFilter.All;

        private void SetMetaDataValuesFilterInternal(string inMetaDataKey, List<string> inMetaDataValues)
        {
            _metaDataValueStartsWithFilter = null;
            _metaDataKeyFilter = inMetaDataKey;
            _metaDataValuesFilter = inMetaDataValues;
        }

        private void SetMetaDataValueStartsWithFilterInternal(string inMetaDataKey, string inMetaDataValueStartsWith)
        {
            _metaDataValuesFilter = null;
            _metaDataValueStartsWithFilter = inMetaDataValueStartsWith;
            _metaDataKeyFilter = inMetaDataKey;
        }
    }
}