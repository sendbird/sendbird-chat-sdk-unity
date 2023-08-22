// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

using System.Collections.Generic;

namespace Sendbird.Chat
{
    public partial class SbMessageSearchQueryParams
    {
        internal SbMessageSearchQueryParams Clone()
        {
            SbMessageSearchQueryParams queryParams = new SbMessageSearchQueryParams();
            {
                queryParams.Keyword = Keyword;
                queryParams.ChannelUrl = ChannelUrl;
                queryParams.ChannelCustomType = ChannelCustomType;
                queryParams.Reverse = Reverse;
                queryParams.ExactMatch = ExactMatch;
                queryParams.MessageTimestampFrom = MessageTimestampFrom;
                queryParams.MessageTimestampTo = MessageTimestampTo;
                queryParams.Order = Order;
                queryParams.AdvancedQuery = AdvancedQuery;
                queryParams.Limit = Limit;

                if (TargetFields != null && 0 < TargetFields.Count)
                {
                    queryParams.TargetFields = new List<string>(TargetFields);
                }
            }

            return queryParams;
        }
    }
}