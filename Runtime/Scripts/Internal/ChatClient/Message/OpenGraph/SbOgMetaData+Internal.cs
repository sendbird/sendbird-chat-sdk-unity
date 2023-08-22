// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbOgMetaData
    {
        private string _description;
        private SbOgImage _defaultImage;
        private string _title;
        private string _url;

        private SbOgMetaData(SbOgMetaData inOgMetaData)
        {
            if (inOgMetaData != null)
            {
                _description = inOgMetaData._description;
                _title = inOgMetaData._title;
                _url = inOgMetaData._url;
                _defaultImage = inOgMetaData._defaultImage?.Clone();
            }
        }

        internal SbOgMetaData(OgMetaDataDto inOgMetaDataDto)
        {
            if (inOgMetaDataDto != null)
            {
                _description = inOgMetaDataDto.description;
                _defaultImage = new SbOgImage(inOgMetaDataDto.image);
                _title = inOgMetaDataDto.title;
                _url = inOgMetaDataDto.url;
            }
        }

        internal SbOgMetaData Clone()
        {
            return new SbOgMetaData(this);
        }
    }
}