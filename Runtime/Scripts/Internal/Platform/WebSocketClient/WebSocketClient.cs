// 
//  Copyright (c) 2024 Sendbird, Inc.
// 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Sendbird.Chat
{
    internal class WebSocketClient
    {
        internal delegate void ConnectResultDelegate(WsClientConnectResult inConnectResult);
        internal delegate void SendResultDelegate(WsClientSendResult inSendResult);
        internal delegate void CloseResultDelegate(WsClientCloseResult inSendResult);
        internal delegate void ReceiveMessageDelegate(string inMessage);
        internal delegate void ErrorDelegate(WsClientError inError);

        private IWebSocket _webSocket;
        private IWsClientEventListener _eventListeners = null;
        private readonly Queue<Action> _sequenceQueue = new Queue<Action>();
        private readonly BlockingCollection<Action> _callbackBlockingCollection = new BlockingCollection<Action>();
        private readonly BlockingCollection<string> _receivedMessageBlockingCollection = new BlockingCollection<string>();
        private bool _isTerminated = false;
        private readonly object _lockObject = new object();

        public WsClientStateType ClientStateType { get; private set; }

        internal void Initialize(IWsClientEventListener inWsClientEventListener)
        {
            _isTerminated = false;
            _eventListeners = inWsClientEventListener;
            ClientStateType = WsClientStateType.None;
        }

        internal void Terminate()
        {
            _isTerminated = true;
            FlushAllReceivedMessage();

            while (0 < _sequenceQueue.Count)
            {
                _sequenceQueue.Dequeue().Invoke();
            }

            _eventListeners = null;
            ClientStateType = WsClientStateType.None;
        }

        internal void Connect(WsClientConnectParams inWsClientConnectParams, ConnectResultDelegate inResultHandler = null)
        {
            Logger.Info(Logger.CategoryType.WebSocket, "WebSocketClient::Connect Start");

            if (IsInSequenceState() || 0 < _sequenceQueue.Count)
            {
                _sequenceQueue.Enqueue(() => { ConnectInternal(inWsClientConnectParams, inResultHandler); });
            }
            else
            {
                ConnectInternal(inWsClientConnectParams, inResultHandler);
            }
        }

        internal void Send(string inTextMessage, SendResultDelegate inResultHandler = null)
        {
            Logger.Debug(Logger.CategoryType.WebSocket, $"WebSocketClient::Send Message:[{inTextMessage}]");

            _webSocket.Send(inTextMessage, OnSendResult);
            return;

            void OnSendResult(WsClientSendResult inSendResult)
            {
                _callbackBlockingCollection.Add(() =>
                {
                    inResultHandler?.Invoke(inSendResult);
                });
            }
        }

        internal void Close(Action<WsClientCloseResult> inResultHandler = null)
        {
            Logger.Info(Logger.CategoryType.WebSocket, "WebSocketClient::Close");
            if (IsInSequenceState() || 0 < _sequenceQueue.Count)
            {
                _sequenceQueue.Enqueue(() => { CloseInternal(inResultHandler); });
            }
            else
            {
                CloseInternal(inResultHandler);
            }
        }

        internal void Update()
        {
            while (0 < _callbackBlockingCollection.Count)
            {
                _callbackBlockingCollection.Take().Invoke();
            }

            if (!IsInSequenceState() && 0 < _sequenceQueue.Count)
            {
                _sequenceQueue.Dequeue().Invoke();
            }

            FlushAllReceivedMessage();
        }

        private void FlushAllReceivedMessage()
        {
            while (0 < _receivedMessageBlockingCollection.Count)
            {
                string receivedMessage = _receivedMessageBlockingCollection.Take();
                Logger.Debug(Logger.CategoryType.WebSocket, $"Flush received queue\n {receivedMessage}");
                JsonMemoryProfiler.TakeSnapshot("WsClient:BeforeFlushToRouter", receivedMessage.Length * sizeof(char));
                _eventListeners?.OnReceive(receivedMessage);
                JsonMemoryProfiler.TakeSnapshot("WsClient:AfterFlushToRouter");
            }
        }


        private void ConnectInternal(WsClientConnectParams inWsClientConnectParams, ConnectResultDelegate inResultHandler = null)
        {
            if (_isTerminated)
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(new WsClientConnectResult(WsClientConnectResult.ResultType.Terminated)); });
                return;
            }

            if (inWsClientConnectParams == null || string.IsNullOrEmpty(inWsClientConnectParams.Uri))
            {
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(new WsClientConnectResult(WsClientConnectResult.ResultType.InvalidParams)); });
                return;
            }

            Logger.Info(Logger.CategoryType.WebSocket, $"WebSocketClient::ConnectInternal start uri:{inWsClientConnectParams.Uri}");
            ChangeWebsocketStateWithBlocking(WsClientStateType.Connecting);

            _webSocket = PlatformModule.PlatformProvider.CreateWebSocket();
            _webSocket.Connect(inWsClientConnectParams, OnResult, OnReceiveMessage, OnError);
            return;

            void OnResult(WsClientConnectResult inConnectResult)
            {
                if (inConnectResult.resultType == WsClientConnectResult.ResultType.Succeeded)
                {
                    ChangeWebsocketStateWithBlocking(WsClientStateType.Open);
                }
                else
                {
                    ChangeWebsocketStateWithBlocking(WsClientStateType.None);
                }
                
                _callbackBlockingCollection.Add(() =>
                {
                    inResultHandler?.Invoke(inConnectResult);
                });
            }

            void OnReceiveMessage(string inMessage)
            {
                _receivedMessageBlockingCollection.Add(inMessage);
            }

            void OnError(WsClientError inWsClientError)
            {
                if (ClientStateType != WsClientStateType.Closed && ClientStateType != WsClientStateType.Closing)
                {
                    _callbackBlockingCollection.Add(() =>
                    {
                        if (inWsClientError == null)
                        {
                            inWsClientError = new WsClientError(WsClientErrorType.Unknown);
                        }

                        Logger.Warning(Logger.CategoryType.WebSocket, $"WebSocketClient::OnError ErrorType:{inWsClientError.ErrorMessage}");
                        if (_eventListeners != null)
                        {
                            _eventListeners.OnErrorInOpenState(inWsClientError);
                        }
                    });
                }
            }
        }

        private void CloseInternal(Action<WsClientCloseResult> inResultHandler = null)
        {
            Logger.Info(Logger.CategoryType.WebSocket, "WebSocketClient::CloseInternal start");

            if (ClientStateType == WsClientStateType.Closing || ClientStateType == WsClientStateType.Closed)
            {
                Logger.Warning(Logger.CategoryType.WebSocket, $"WebSocketClient::CloseInternal Already closed State:{ClientStateType}");
                CoroutineManager.Instance.CallOnNextFrame(() => { inResultHandler?.Invoke(new WsClientCloseResult(WsClientCloseResult.ResultType.AlreadyClosingOrClosed)); });
                return;
            }

            ChangeWebsocketStateWithBlocking(WsClientStateType.Closing);
            _webSocket.Close(OnResult);
            return;

            void OnResult(WsClientCloseResult inCloseResult)
            {
                ChangeWebsocketStateWithBlocking(WsClientStateType.Closed);
                _callbackBlockingCollection.Add(() =>
                {
                    inResultHandler?.Invoke(inCloseResult);
                });
            }
        }


        private void ChangeWebsocketStateWithBlocking(WsClientStateType inWsStateType)
        {
            lock (_lockObject)
            {
                if (ClientStateType != inWsStateType)
                {
                    Logger.Info(Logger.CategoryType.WebSocket, $"WebSocketClient::ChangeWebsocketState {ClientStateType}->{inWsStateType}");
                    ClientStateType = inWsStateType;
                }
            }
        }

        private bool IsInSequenceState()
        {
            return ClientStateType == WsClientStateType.Connecting || ClientStateType == WsClientStateType.Closing;
        }
    }
}