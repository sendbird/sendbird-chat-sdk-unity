using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Sendbird.Chat
{
    internal static class JsNativeWebSocketBridge
    {
        private delegate void WebSocketOpenCallback(int inClientId);
        private delegate void WebSocketMessageCallback(int inClientId, string inMessage);
        private delegate void WebSocketErrorCallback(int inClientId);
        private delegate void WebSocketCloseCallback(int inClientId);

        [DllImport("__Internal", EntryPoint = "SendbirdWebSocketBridge_CreateWebSocketClient")]
        private static extern int CreateWebSocketClientByNative();

        [DllImport("__Internal", EntryPoint = "SendbirdWebSocketBridge_DeleteWebSocketClient")]
        private static extern void DeleteWebSocketClientByNative(int inClientId);

        [DllImport("__Internal", EntryPoint = "SendbirdWebSocketBridge_RegisterCallbacks")]
        private static extern void RegisterCallbacksByNative(WebSocketOpenCallback inOnOpen, WebSocketMessageCallback inOnMessage,
                                                             WebSocketErrorCallback inOnError, WebSocketCloseCallback inOnClose);

        [DllImport("__Internal", EntryPoint = "SendbirdWebSocketBridge_Connect")]
        private static extern void ConnectByNative(int inClientId, string inUri, string inEncodedProtocols);

        [DllImport("__Internal", EntryPoint = "SendbirdWebSocketBridge_Send")]
        private static extern void SendByNative(int inClientId, string inMessage);

        [DllImport("__Internal", EntryPoint = "SendbirdWebSocketBridge_Close")]
        private static extern void CloseByNative(int inClientId);

        [AOT.MonoPInvokeCallback(typeof(WebSocketOpenCallback))]
        public static void OnWebSocketOpen(int inClientId)
        {
            if (_nativeWebSocketsByClientId.TryGetValue(inClientId, out JsWebSocket webSocket))
            {
                webSocket.OnWebSocketConnected();
            }
            else
            {
                Logger.Warning(Logger.CategoryType.WebSocket, $"JsNativeWebSocketBridge::OnWebSocketOpen invalid client id:{inClientId}");
            }
        }

        [AOT.MonoPInvokeCallback(typeof(WebSocketMessageCallback))]
        public static void OnWebSocketMessage(int inClientId, string inMessage)
        {
            if (_nativeWebSocketsByClientId.TryGetValue(inClientId, out JsWebSocket webSocket))
            {
                webSocket.OnWebSocketMessage(inMessage);
            }
            else
            {
                Logger.Warning(Logger.CategoryType.WebSocket, $"JsNativeWebSocketBridge::OnWebSocketMessage invalid client id:{inClientId}");
            }
        }

        [AOT.MonoPInvokeCallback(typeof(WebSocketErrorCallback))]
        public static void OnWebSocketError(int inClientId)
        {
            if (_nativeWebSocketsByClientId.TryGetValue(inClientId, out JsWebSocket webSocket))
            {
                webSocket.OnWebSocketError();
            }
            else
            {
                Logger.Warning(Logger.CategoryType.WebSocket, $"JsNativeWebSocketBridge::OnWebSocketError invalid client id:{inClientId}");
            }
        }

        [AOT.MonoPInvokeCallback(typeof(WebSocketCloseCallback))]
        public static void OnWebSocketClose(int inClientId)
        {
            if (_nativeWebSocketsByClientId.TryGetValue(inClientId, out JsWebSocket webSocket))
            {
                webSocket.OnWebSocketClosed();
            }
            else
            {
                Logger.Warning(Logger.CategoryType.WebSocket, $"JsNativeWebSocketBridge::OnWebSocketClose invalid client id:{inClientId}");
            }
        }

        private static readonly Dictionary<int, JsWebSocket> _nativeWebSocketsByClientId = new Dictionary<int, JsWebSocket>();

        static JsNativeWebSocketBridge()
        {
            RegisterCallbacksByNative(JsNativeWebSocketBridge.OnWebSocketOpen, JsNativeWebSocketBridge.OnWebSocketMessage,
                                      JsNativeWebSocketBridge.OnWebSocketError, JsNativeWebSocketBridge.OnWebSocketClose);
        }

        internal static JsWebSocket CreateWebSocketClient()
        {
            int clientId = CreateWebSocketClientByNative();
            DeleteWebSocketClientIfExist(clientId);
            JsWebSocket nativeWebSocket = new JsWebSocket(clientId);
            _nativeWebSocketsByClientId.AddIfNotContains(clientId, nativeWebSocket);
            return nativeWebSocket;
        }

        internal static void DeleteWebSocketClientIfExist(int inClientId)
        {
            if (_nativeWebSocketsByClientId.ContainsKey(inClientId))
            {
                DeleteWebSocketClientByNative(inClientId);
                _nativeWebSocketsByClientId.Remove(inClientId);
            }
        }

        internal static void Connect(int inClientId, string inUri, string inEncodedProtocols)
        {
            ConnectByNative(inClientId, inUri, inEncodedProtocols);
        }

        internal static void Send(int inClientId, string inMessage)
        {
            SendByNative(inClientId, inMessage);
        }

        internal static void Close(int inClientId)
        {
            CloseByNative(inClientId);
        }
    }
}