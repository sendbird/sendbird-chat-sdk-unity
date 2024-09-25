// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.Text;

namespace Sendbird.Chat
{
    internal class HttpMultipartFormSectionData : HttpMultipartFormSectionAbstract
    {
        internal string Data { get; }

        internal HttpMultipartFormSectionData(string inName, string inData, string inContentType = MimeType.TEXT_PLAIN) : base(inName, inContentType)
        {
            Data = inData;
        }

        internal override byte[] GetBytesOfData()
        {
            if (string.IsNullOrEmpty(Data))
                return null;

            return Encoding.UTF8.GetBytes(Data);
        }
    }
}