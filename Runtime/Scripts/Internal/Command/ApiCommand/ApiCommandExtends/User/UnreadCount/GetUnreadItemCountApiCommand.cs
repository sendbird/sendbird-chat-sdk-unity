// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

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

        internal sealed class Response : ApiCommandAbstract.Response
        {
            internal Dictionary<SbUnreadItemKey, int> CountByUnreadItemKey { get; private set; }

            internal override void OnResponseAfterDeserialize(string inJsonString)
            {
                if (string.IsNullOrEmpty(inJsonString))
                    return;

                Dictionary<string, int> countByItemKey;
                using (JsonTextReader reader = JsonStreamingPool.CreateReader(inJsonString))
                {
                    reader.Read();
                    countByItemKey = JsonStreamingHelper.ReadStringIntDictionary(reader);
                }

                if (countByItemKey != null && 0 < countByItemKey.Count)
                {
                    CountByUnreadItemKey = new Dictionary<SbUnreadItemKey, int>();
                    foreach (KeyValuePair<string, int> keyValuePair in countByItemKey)
                    {
                        SbUnreadItemKey itemKey = SbUnreadItemKeyExtension.JsonNameToType(keyValuePair.Key);
                        CountByUnreadItemKey.AddIfNotContains(itemKey, keyValuePair.Value);
                    }
                }
            }
        }
    }
}