// 
//  Copyright (c) 2022 Sendbird, Inc.
// 


namespace Sendbird.Chat
{
    internal abstract class HttpMultipartFormSectionAbstract
    {
        internal string Name { get; }
        internal string ContentType { get; }

        internal HttpMultipartFormSectionAbstract(string inName, string inContentType)
        {
            Name = inName;
            ContentType = inContentType;
        }

        internal abstract byte[] GetBytesOfData();

    }
}