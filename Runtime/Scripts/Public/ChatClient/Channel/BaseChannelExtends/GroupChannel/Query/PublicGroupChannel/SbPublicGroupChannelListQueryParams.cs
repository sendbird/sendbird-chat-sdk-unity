// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    /// <summary>
    /// Params for creating a PublicGroupChannelListQuery object.
    /// </summary>
    /// @since 4.0.0
    public partial class SbPublicGroupChannelListQueryParams
    {
        /// <summary>
        /// Result order of channels. Refer to SbPublicGroupChannelListOrder.
        /// </summary>
        /// @since 4.0.0
        public SbPublicGroupChannelListOrder Order { get => _order; set => _order = value; }

        /// <summary>
        /// Super channel filter. Refer to SbGroupChannelSuperChannelFilter.
        /// </summary>
        /// @since 4.0.0
        public SbGroupChannelSuperChannelFilter SuperChannelFilter { get => _superChannelFilter; set => _superChannelFilter = value; }

        /// <summary>
        /// Checks whether query result includes empty channels. (channels without messages).
        /// </summary>
        /// @since 4.0.0
        public bool IncludeEmptyChannel { get => _includeEmptyChannel; set => _includeEmptyChannel = value; }

        /// <summary>
        /// Checks whether query result includes frozen channels.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeFrozenChannel { get => _includeFrozenChannel; set => _includeFrozenChannel = value; }

        /// <summary>
        /// The maximum number of GroupChannels per page.
        /// </summary>
        /// @since 4.0.0
        public int Limit { get => _limit; set => _limit = value; }

        /// <summary>
        /// Membership filter.
        /// </summary>
        /// @since 4.0.0
        public SbPublicGroupChannelMembershipFilter PublicMembershipFilter { get => _publicMembershipFilter; set => _publicMembershipFilter = value; }

        /// <summary>
        /// A channel name filter. GroupChannel list containing the passed channel name will be returned. If you pass name such as "abc", then the returned channel list will be containing name like "abc". It will return null if channel name filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public string ChannelNameContainsFilter { get => _channelNameContainsFilter; set => _channelNameContainsFilter = value; }

        /// <summary>
        /// List of channel URL filter. It will return null if channel URL filter hasn't been set before. GroupChannel list containing only and exactly the passed GroupChannel URLs will be returned.
        /// </summary>
        /// @since 4.0.0
        public List<string> ChannelUrlsFilter { get => _channelUrlsFilter; set => _channelUrlsFilter = value; }

        /// <summary>
        /// List of custom type filter. GroupChannel list containing only and exactly the passed custom types will be returned. It will return null if custom types filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public List<string> CustomTypesFilter { get => _customTypesFilter; set => _customTypesFilter = value; }

        /// <summary>
        /// A filter to return channels that start with the specified Custom Type. It will return null if custom type starts with filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public string CustomTypeStartsWithFilter { get => _customTypeStartsWithFilter; set => _customTypeStartsWithFilter = value; }

        /// <summary>
        /// Whether to include channel metadata on fetch.
        /// </summary>
        /// @since 4.0.0
        public bool IncludeMetaData { get => _includeMetaData; set => _includeMetaData = value; }

        /// <summary>
        /// Works exclusively with MetaDataValueStartsWithFilter.
        /// </summary>
        /// @since 4.0.0
        public List<string> MetaDataValuesFilter => _metaDataValuesFilter;

        /// <summary>
        /// The metadataKey set with either MetaDataValuesFilter or MetaDataValueStartsWithFilter.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataKeyFilter => _metaDataKeyFilter;

        /// <summary>
        /// Meta data order key filter. It will return null if meta data order key filter hasn't been set before.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataOrderKeyFilter { get => _metaDataOrderKeyFilter; set => _metaDataOrderKeyFilter = value; }

        /// <summary>
        /// Works exclusively with MetaDataValuesFilter.
        /// </summary>
        /// @since 4.0.0
        public string MetaDataValueStartsWithFilter => _metaDataValueStartsWithFilter;

        /// <summary>
        /// Sets GroupChannel meta data filter. GroupChannel list containing only and exactly the passed GroupChannel meta data will be returned. If this is set, it will reset the filter set from MetaDataValueStartsWithFilter.
        /// </summary>
        /// <param name="inMetaDataKey"></param>
        /// <param name="inMetaDataValues"></param>
        /// @since 4.0.0
        public void SetMetaDataValuesFilter(string inMetaDataKey, List<string> inMetaDataValues)
        {
            SetMetaDataValuesFilterInternal(inMetaDataKey, inMetaDataValues);
        }

        /// <summary>
        /// Sets GroupChannel meta data filter. GroupChannel list containing passed GroupChannel meta data key with values that starts with the passed on value will be returned. If this is set, it will reset the filter set from MetaDataValuesFilter.
        /// </summary>
        /// <param name="inMetaDataKey"></param>
        /// <param name="inMetaDataValueStartsWith"></param>
        /// @since 4.0.0
        public void SetMetaDataValueStartsWithFilter(string inMetaDataKey, string inMetaDataValueStartsWith)
        {
            SetMetaDataValueStartsWithFilterInternal(inMetaDataKey, inMetaDataValueStartsWith);
        }
    }
}