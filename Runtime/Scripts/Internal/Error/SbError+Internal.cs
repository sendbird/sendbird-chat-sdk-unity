// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbError
    {
        internal SbError(SbErrorCode inErrorCode, string inErrorMessage = "")
        {
            ErrorCode = inErrorCode;
            ErrorMessage = inErrorMessage;
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = ErrorCode.ToDescriptionString();
            }
        }
        
        internal SbError(int inErrorCode, string inErrorMessage = "")
        {
            ErrorCode = (SbErrorCode)inErrorCode;
            ErrorMessage = inErrorMessage;
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = ErrorCode.ToDescriptionString();
            }
        }
    }
}