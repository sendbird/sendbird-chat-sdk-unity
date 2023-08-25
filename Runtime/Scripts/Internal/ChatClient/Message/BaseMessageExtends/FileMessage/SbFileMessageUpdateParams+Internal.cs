// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbFileMessageUpdateParams : SbBaseMessageUpdateParams
    {
        private SbFileMessageUpdateParams(SbFileMessageUpdateParams inOtherParams) : base(inOtherParams) { }

        internal override SbBaseMessageUpdateParams Clone()
        {
            return new SbFileMessageUpdateParams(this);
        }
    }
}