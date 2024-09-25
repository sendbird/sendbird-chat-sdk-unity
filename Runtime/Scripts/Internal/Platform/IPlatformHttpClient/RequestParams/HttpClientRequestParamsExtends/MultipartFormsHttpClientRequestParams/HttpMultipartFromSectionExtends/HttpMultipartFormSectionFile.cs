// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System.IO;

namespace Sendbird.Chat
{
    internal class HttpMultipartFormSectionFile : HttpMultipartFormSectionAbstract
    {
        internal string FileName { get; }
        private readonly string _filePath;

        internal HttpMultipartFormSectionFile(string inName, string inFileName, string inFilePath, string inContentType = MimeType.APPLICATION_OCTET_STREAM) : base(inName, inContentType)
        {
            FileName = inFileName;
            _filePath = inFilePath;
        }

        internal override byte[] GetBytesOfData()
        {
            if (string.IsNullOrEmpty(_filePath) || File.Exists(_filePath) == false)
                return null;

            return File.ReadAllBytes(_filePath);
        }
    }
}