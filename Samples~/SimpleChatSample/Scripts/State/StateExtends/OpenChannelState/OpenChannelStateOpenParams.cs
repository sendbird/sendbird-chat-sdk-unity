// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat.Sample
{
    public class OpenChannelStateOpenParams : StateAbstract.OpenParamsAbstract
    {
        public OpenChannelStateOpenParams(SbOpenChannel inOpenChannel)
        {
            OpenChannel = inOpenChannel;
        }
        public SbOpenChannel OpenChannel { get; }
    }
}