// 
//  Copyright (c) 2022 Sendbird, Inc.
// 

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Sendbird.Chat
{
    internal sealed class UploadFileApiCommand
    {
        internal sealed class Request : ApiCommandAbstract.PostRequest
        {
            internal override string RequestId { get; }
            private readonly SbMultiProgressHandler _progressHandler = null;
            private ulong _prevUploadedBytes = 0;

            internal Request(string inChannelUrl, string inRequestId, string inSaveFileName, SbFileInfo inUploadFileInfo, List<SbThumbnailSize> inThumbnailSizes,
                             ResultHandler inResultHandler, SbMultiProgressHandler inMultiProgressHandler = null)
            {
                Url = $"{STORAGE_PREFIX_URL}/file";
                ResponseType = typeof(Response);
                resultHandler = inResultHandler;
                RequestId = inRequestId;
                _progressHandler = inMultiProgressHandler;

                if (string.IsNullOrEmpty(inChannelUrl) == false && inUploadFileInfo != null && inUploadFileInfo.IsExists())
                {
                    AddMultipartFormSectionDataIfNotNullOrEmpty("channel_url", inChannelUrl);
                    AddMultipartFormSectionFileIfExists("file", inSaveFileName, inUploadFileInfo.FullPath);
                    if (inThumbnailSizes != null && 0 < inThumbnailSizes.Count)
                    {
                        for (int index = 0; index < inThumbnailSizes.Count; index++)
                        {
                            AddMultipartFormSectionDataIfNotNullOrEmpty($"thumbnail{index + 1}", $"{inThumbnailSizes[index].MaxWidth},{inThumbnailSizes[index].MaxHeight}");
                        }
                    }
                }
            }

            internal override void OnUploadProgress(ulong inUploadedBytes, ulong inTotalBytes)
            {
                if (_progressHandler != null)
                {
                    ulong bytesSent = inUploadedBytes - _prevUploadedBytes;
                    _progressHandler.Invoke(RequestId, bytesSent, inUploadedBytes, inTotalBytes);
                }

                _prevUploadedBytes = inUploadedBytes;
            }
        }

        [Serializable]
        internal sealed class Response : ApiCommandAbstract.Response
        {
            [JsonProperty("url")] internal readonly string url;
            [JsonProperty("thumbnails")] internal readonly List<ThumbnailDto> thumbnailDtos;
            [JsonProperty("require_auth")] internal readonly bool requireAuth;
            [JsonProperty("file_size")] internal readonly int fileSize;
        }
    }
}