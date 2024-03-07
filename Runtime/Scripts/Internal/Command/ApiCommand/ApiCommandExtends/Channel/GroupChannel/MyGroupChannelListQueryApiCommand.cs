// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class MyGroupChannelListQueryApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            private const string CHANNEL_URLS = "channel_urls";
            private const string CUSTOM_TYPES = "custom_types";
            private const string CUSTOM_TYPE_STARTSWITH = "custom_type_startswith";
            private const string DISTINCT_MODE = "distinct_mode";
            private const string HIDDEN_MODE = "hidden_mode";
            private const string LIMIT = "limit";
            private const string MEMBERS_EXACTLY_IN = "members_exactly_in";
            private const string MEMBERS_INCLUDE_IN = "members_include_in";
            private const string MEMBERS_NICKNAME = "members_nickname";
            private const string MEMBERS_NICKNAME_CONTAINS = "members_nickname_contains";
            private const string MEMBERS_NICKNAME_STARTSWITH = "members_nickname_startswith";
            private const string MEMBER_STATE_FILTER = "member_state_filter";
            private const string METADATA_KEY = "metadata_key";
            private const string METADATA_ORDER_KEY = "metadata_order_key";
            private const string METADATA_VALUES = "metadata_values";
            private const string METADATA_VALUE_STARTSWITH = "metadata_value_startswith";
            private const string NAME_CONTAINS = "name_contains";
            private const string ORDER = "order";
            private const string PUBLIC_MODE = "public_mode";
            private const string QUERY_TYPE = "query_type";
            private const string SEARCH_FIELDS = "search_fields";
            private const string SEARCH_QUERY = "search_query";
            private const string SHOW_DELIVERY_RECEIPT = "show_delivery_receipt";
            private const string SHOW_EMPTY = "show_empty";
            private const string SHOW_MEMBER = "show_member";
            private const string SHOW_FROZEN = "show_frozen";
            private const string SHOW_METADATA = "show_metadata";
            private const string SHOW_READ_RECEIPT = "show_read_receipt";
            private const string SUPER_MODE = "super_mode";
            private const string TOKEN = "token";
            private const string UNREAD_FILTER = "unread_filter";
            private const string INCLUDE_CHAT_NOTIFICATION = "include_chat_notification";
            internal Request(string inUserId, string inToken, SbGroupChannelListQuery inQuery, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/my_group_channels";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                InsertQueryParamIfNotNullOrEmpty(TOKEN, inToken);
                InsertQueryParamIfNotNullOrEmpty(LIMIT, inQuery.Limit);
                InsertQueryParamIfNotNullOrEmpty(SHOW_READ_RECEIPT, true);
                InsertQueryParamIfNotNullOrEmpty(SHOW_DELIVERY_RECEIPT, true);
                InsertQueryParamIfNotNullOrEmpty(SHOW_MEMBER, true);
                InsertQueryParamIfNotNullOrEmpty(SHOW_EMPTY, inQuery.IncludeEmptyChannel);
                InsertQueryParamIfNotNullOrEmpty(SHOW_FROZEN, inQuery.IncludeFrozenChannel);
                InsertQueryParamIfNotNullOrEmpty(SHOW_METADATA, inQuery.IncludeMetaData);
                InsertQueryParamIfNotNullOrEmpty(INCLUDE_CHAT_NOTIFICATION, inQuery.IncludeChatNotification);
                InsertQueryParamIfNotNullOrEmpty(DISTINCT_MODE, "all");
                InsertQueryParamIfNotNullOrEmpty(ORDER, inQuery.Order.ToJsonName());

                if (inQuery.Order == SbGroupChannelListOrder.ChannelMetaDataValueAlphabetical)
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
                
                InsertQueryParamIfNotNullOrEmpty(MEMBER_STATE_FILTER, inQuery.MyMemberStateFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty(MEMBERS_NICKNAME_CONTAINS, inQuery.NicknameContainsFilter);
                InsertQueryParamIfNotNullOrEmpty(MEMBERS_NICKNAME_STARTSWITH, inQuery.NicknameStartsWithFilter);
                InsertQueryParamIfNotNullOrEmpty(MEMBERS_NICKNAME, inQuery.NicknameExactMatchFilter);
                
                InsertQueryParamWithListIfNotNullOrEmpty(MEMBERS_EXACTLY_IN, inQuery.UserIdsExactFilter);
                if (inQuery.UserIdsIncludeFilter != null && 0 < inQuery.UserIdsIncludeFilter.Count)
                {
                    InsertQueryParamIfNotNullOrEmpty(QUERY_TYPE, inQuery.QueryType.ToJsonName());
                    InsertQueryParamWithListIfNotNullOrEmpty(MEMBERS_INCLUDE_IN, inQuery.UserIdsIncludeFilter);
                }

                InsertQueryParamIfNotNullOrEmpty(SEARCH_QUERY, inQuery.SearchQuery);
                if (inQuery.SearchFields != null && 0 < inQuery.SearchFields.Count)
                {
                    StringBuilder searchFieldsStringBuilder = new StringBuilder(30);
                    for (int index = 0; index < inQuery.SearchFields.Count; index++)
                    {
                        if (0 < index)
                            searchFieldsStringBuilder.Append(",");

                        searchFieldsStringBuilder.Append(inQuery.SearchFields[index].ToJsonName());
                    }

                    InsertQueryParamIfNotNullOrEmpty(SEARCH_FIELDS, searchFieldsStringBuilder.ToString());
                }

                InsertQueryParamIfNotNullOrEmpty(SUPER_MODE, inQuery.SuperChannelFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty(PUBLIC_MODE, inQuery.PublicChannelFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty(HIDDEN_MODE, inQuery.ChannelHiddenStateFilter.ToJsonName());
                InsertQueryParamIfNotNullOrEmpty(UNREAD_FILTER, inQuery.UnreadChannelFilter.ToJsonName());
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