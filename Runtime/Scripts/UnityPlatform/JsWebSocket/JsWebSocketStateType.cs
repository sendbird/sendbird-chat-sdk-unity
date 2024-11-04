#if UNITY_WEBGL && !UNITY_EDITOR
namespace Sendbird.Chat
{
    internal enum JsWebSocketStateType
    {
        None,
        Connecting,
        Open,
        Closing,
        Closed,
    }
}

#endif //#if UNITY_WEBGL && !UNITY_EDITOR