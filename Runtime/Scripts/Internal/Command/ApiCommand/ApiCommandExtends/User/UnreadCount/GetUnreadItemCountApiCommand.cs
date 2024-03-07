// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Sendbird.Chat
{
    internal sealed class GetUnreadItemCountApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.GetRequest
        {
            internal Request(string inUserId, List<SbUnreadItemKey> inItemKeys, ResultHandler inResultHandler)
            {
                inUserId = WebUtility.UrlEncode(inUserId);
                Url = $"{USERS_PREFIX_URL}/{inUserId}/unread_item_count";

                ResponseType = typeof(Response);
                resultHandler = inResultHandler;

                List<string> keys = inItemKeys.Select(inUnreadItemKey => inUnreadItemKey.ToJsonName()).ToList();
                InsertQueryParamWithListIfNotNullOrEmpty("item_keys", keys);
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            private Dictionary<string, int> _countByItemKey;
            internal Dictionary<SbUnreadItemKey, int> CountByUnreadItemKey { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                _countByItemKey = NewtonsoftJsonExtension.DeserializeObjectIgnoreException<Dictionary<string, int>>(inJsonString);
                if (_countByItemKey != null && 0 < _countByItemKey.Count)
                {
                    CountByUnreadItemKey = new Dictionary<SbUnreadItemKey, int>();
                    foreach (KeyValuePair<string, int> keyValuePair in _countByItemKey)
                    {
                        SbUnreadItemKey itemKey = SbUnreadItemKeyExtension.JsonNameToType(keyValuePair.Key);
                        CountByUnreadItemKey.AddIfNotContains(itemKey, keyValuePair.Value);
                    }
                }
            }
        }
    }
}