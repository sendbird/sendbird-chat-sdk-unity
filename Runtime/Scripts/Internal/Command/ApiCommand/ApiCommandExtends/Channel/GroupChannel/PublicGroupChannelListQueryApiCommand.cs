// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class PublicGroupChannelListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            private const string CHANNEL_URLS = "channel_urls";
            private const string CUSTOM_TYPES = "custom_types";
            private const string CUSTOM_TYPE_STARTSWITH = "custom_type_startswith";
            private const string DISTINCT_MODE = "distinct_mode";
            private const string LIMIT = "limit";
            private const string METADATA_KEY = "metadata_key";
            private const string METADATA_ORDER_KEY = "metadata_order_key";
            private const string METADATA_VALUES = "metadata_values";
            private const string METADATA_VALUE_STARTSWITH = "metadata_value_startswith";
            private const string NAME_CONTAINS = "name_contains";
            private const string ORDER = "order";
            private const string PUBLIC_MEMBERSHIP_MODE = "public_membership_mode";
            private const string PUBLIC_MODE = "public_mode";
            private const string SHOW_DELIVERY_RECEIPT = "show_delivery_receipt";
            private const string SHOW_EMPTY = "show_empty";
            private const string SHOW_FROZEN = "show_frozen";
            private const string SHOW_MEMBER = "show_member";
            private const string SHOW_METADATA = "show_metadata";
            private const string SHOW_READ_RECEIPT = "show_read_receipt";
            private const string SUPER_MODE = "super_mode";
            private const string TOKEN = "token";

            internal Request(string inToken, SbPublicGroupChannelListQuery inQuery, ResultHandler inResultHandler)
            {
                Url = $"{ChannelTypeToUrlPrefix(SbChannelType.Group)}";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty(PUBLIC_MODE, SbGroupChannelPublicChannelFilter.Public.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty(TOKEN, inToken);
                InsertQueryParamIfNotNullOrEmpty(LIMIT, inQuery.Limit);
                InsertQueryParamIfNotNullOrEmpty(SHOW_READ_RECEIPT, true);
                InsertQueryParamIfNotNullOrEmpty(SHOW_DELIVERY_RECEIPT, true);
                InsertQueryParamIfNotNullOrEmpty(SHOW_MEMBER, true);
                InsertQueryParamIfNotNullOrEmpty(SHOW_EMPTY, inQuery.IncludeEmptyChannel);
                InsertQueryParamIfNotNullOrEmpty(SHOW_FROZEN, inQuery.IncludeFrozenChannel);
                InsertQueryParamIfNotNullOrEmpty(SHOW_METADATA, inQuery.IncludeMetaData);
                InsertQueryParamIfNotNullOrEmpty(DISTINCT_MODE, "all");
                InsertQueryParamIfNotNullOrEmpty(ORDER, inQuery.Order.ToJsonName());

                if (inQuery.Order == SbPublicGroupChannelListOrder.ChannelMetaDataValueAlphabetical)
                    InsertQueryParamIfNotNullOrEmpty(METADATA_ORDER_KEY, inQuery.MetaDataOrderKeyFilter);

                if (string.IsNullOrEmpty(inQuery.MetaDataKeyFilter) == false)
                {
                    InsertQueryParamIfNotNullOrEmpty(METADATA_KEY, inQuery.MetaDataKeyFilter);
                    InsertQueryParamIfNotNullOrEmpty(METADATA_VALUE_STARTSWITH, inQuery.MetaDataValueStartsWithFilter);
                    InsertQueryParamWithListIfNotNullOrEmpty(METADATA_VALUES, inQuery.MetaDataValuesFilter);
                }

                InsertQueryParamWithListIfNotNullOrEmpty(CUSTOM_TYPES, inQuery.CustomTypesFilter);
                InsertQueryParamIfNotNullOrEmpty(CUSTOM_TYPE_STARTSWITH, inQuery.CustomTypeStartsWithFilter);

                InsertQueryParamWithListIfNotNullOrEmpty(CHANNEL_URLS, inQuery.ChannelUrlsFilter);
                InsertQueryParamIfNotNullOrEmpty(NAME_CONTAINS, inQuery.ChannelNameContainsFilter);

                InsertQueryParamIfNotNullOrEmpty(SUPER_MODE, inQuery.SuperChannelFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty(PUBLIC_MEMBERSHIP_MODE, inQuery.PublicMembershipFilter.ToJsonName());
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("next")] internal readonly string token;
            [JsonProperty("channels")] internal readonly List<GroupChannelDto> groupChannelDtos;
        }
    }
}