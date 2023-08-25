// 
//  Copyright (c) 2023 Sendbird, Inc.
// 

namespace Sendbird.Chat
{
    public partial class SbOgImage
    {
        private string _alt;
        private int _height;
        private int _width;
        private string _secureUrl;
        private string _type;
        private string _url;

        private SbOgImage(SbOgImage inOgImage)
        {
            if (inOgImage != null)
            {
                _alt = inOgImage._alt;
                _height = inOgImage._height;
                _width = inOgImage._width;
                _secureUrl = inOgImage._secureUrl;
                _type = inOgImage._type;
                _url = inOgImage._url;
            }
        }

        internal SbOgImage(OgImageDto inOgImageDto)
        {
            if (inOgImageDto != null)
            {
                _alt = inOgImageDto.alt;
                _height = inOgImageDto.height;
                _width = inOgImageDto.width;
                _secureUrl = inOgImageDto.secureUrl;
                _type = inOgImageDto.type;
                _url = inOgImageDto.url;
            }
        }

        internal SbOgImage Clone()
        {
            return new SbOgImage(this);
        }
    }
}