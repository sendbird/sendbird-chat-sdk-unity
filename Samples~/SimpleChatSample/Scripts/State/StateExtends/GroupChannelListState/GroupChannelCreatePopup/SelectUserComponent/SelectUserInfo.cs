// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat.Sample
{
    public class SelectUserInfo
    {
        public string UserId { get; }
        public bool IsSelected { get; set; }

        public SelectUserInfo(string inUserId, bool inIsSelected)
        {
            UserId = inUserId;
            IsSelected = inIsSelected;
        }
    }
}