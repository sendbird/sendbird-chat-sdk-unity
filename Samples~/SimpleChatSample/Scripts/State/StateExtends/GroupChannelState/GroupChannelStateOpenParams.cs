// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat.Sample
{
    public class GroupChannelStateOpenParams : StateAbstract.OpenParamsAbstract
    {
        public GroupChannelStateOpenParams(SbGroupChannel inGroupChannel)
        {
            GroupChannel = inGroupChannel;
        }
        public SbGroupChannel GroupChannel { get; }
    }
}