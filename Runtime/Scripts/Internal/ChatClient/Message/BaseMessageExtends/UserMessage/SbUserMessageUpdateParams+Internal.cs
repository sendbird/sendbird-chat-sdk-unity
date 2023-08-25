// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbUserMessageUpdateParams : SbBaseMessageUpdateParams
    {
        private string _message = null;
        private string _mentionedMessageTemplate = null;

        private SbUserMessageUpdateParams(SbUserMessageUpdateParams inOtherParams) : base(inOtherParams)
        {
            if (inOtherParams != null)
            {
                _message = inOtherParams._message;
                _mentionedMessageTemplate = inOtherParams._mentionedMessageTemplate;
            }
        }

        internal override SbBaseMessageUpdateParams Clone()
        {
            return new SbUserMessageUpdateParams(this);
        }
    }
}