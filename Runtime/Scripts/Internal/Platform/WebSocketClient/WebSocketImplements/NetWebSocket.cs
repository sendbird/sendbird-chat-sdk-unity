using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sendbird.Chat
{
    internal class NetWebSocket : IWebSocket
    {
        private readonly ClientWebSocket _webSocket;
        private CancellationTokenSource _connectCancellationTokenSource = null;
        private CancellationTokenSource _receiveCancellationTokenSource = null;
        private readonly object _lockObject = new object();

        internal NetWebSocket()
        {
            _webSocket = new ClientWebSocket();
        }

        void IWebSocket.Connect(WsClientConnectParams inClientConnectParams, WebSocketClient.ConnectResultDelegate inResultHandler,
                                     WebSocketClient.ReceiveMessageDelegate inReceiveMessageHandler, WebSocketClient.ErrorDelegate inErrorHandler)
        {
            ConnectAsync(inClientConnectParams, OnResult);
            return;
            void OnResult(WsClientConnectResult inConnectResult)
            {
                inResultHandler?.Invoke(inConnectResult);
                if (inConnectResult.resultType == WsClientConnectResult.ResultType.Succeeded)
                {
                    ReceiveAsync(inReceiveMessageHandler, inErrorHandler);
                }
            }
        }

        void IWebSocket.Send(string inMessage, WebSocketClient.SendResultDelegate inResultHandler)
        {
            SendAsync(inMessage, inResultHandler);
        }

        void IWebSocket.Close(WebSocketClient.CloseResultDelegate inResultHandler)
        {
            CloseAsync(inResultHandler);
        }

        private async void ConnectAsync(WsClientConnectParams inClientConnectParams, WebSocketClient.ConnectResultDelegate inResultHandler)
        {
            if (_webSocket == null || inClientConnectParams == null)
            {
                inResultHandler?.Invoke(new WsClientConnectResult(WsClientConnectResult.ResultType.InvalidParams));
                return;
            }


            Uri uri = new Uri(inClientConnectParams.Uri);
            foreach (KeyValuePair<string, string> keyValuePair in inClientConnectParams.CustomHeaders)
            {
                _webSocket.Options.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
            }

            WsClientConnectResult.ResultType resultType = WsClientConnectResult.ResultType.Succeeded;
            string resultMessage = string.Empty;
            int nativeErrorCode = WsClientConnectResult.UNKNOWN_NATIVE_ERROR_CODE;
            try
            {
                lock (_lockObject)
                {
                    _connectCancellationTokenSource = new CancellationTokenSource();
                }

                await _webSocket.ConnectAsync(uri, _connectCancellationTokenSource.Token);
            }
            catch (Exception exception)
            {
                resultType = WsClientConnectResult.ResultType.CaughtException;
                if (_connectCancellationTokenSource == null || _connectCancellationTokenSource.IsCancellationRequested)
                    resultType = WsClientConnectResult.ResultType.Canceled;

                if (exception is WebSocketException webSocketException)
                {
                    nativeErrorCode = webSocketException.NativeErrorCode;
                }

                resultMessage = exception.Message;
            }
            finally
            {
                DisposeConnectCancellationTokenWithBlocking(inWithCancel: false);
                inResultHandler?.Invoke(new WsClientConnectResult(resultType, resultMessage, nativeErrorCode));
            }
        }

        private async void SendAsync(string inTextMessage, WebSocketClient.SendResultDelegate inResultHandler = null)
        {
            WsClientSendResult.ResultType resultType = WsClientSendResult.ResultType.Succeeded;
            string resultMessage = string.Empty;
            int nativeErrorCode = WsClientSendResult.UNKNOWN_NATIVE_ERROR_CODE;
            try
            {
                if (_webSocket != null)
                {
                    byte[] encodedBytes = Encoding.UTF8.GetBytes(inTextMessage);
                    ArraySegment<byte> buffer = new ArraySegment<byte>(encodedBytes, 0, encodedBytes.Length);
                    await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            catch (Exception exception)
            {
                resultType = WsClientSendResult.ResultType.CaughtException;
                resultMessage = exception.Message;
                if (exception is WebSocketException webSocketException)
                {
                    nativeErrorCode = webSocketException.ErrorCode;
                }
            }
            finally
            {
                inResultHandler?.Invoke(new WsClientSendResult(resultType, resultMessage, nativeErrorCode));
            }
        }


        async void ReceiveAsync(WebSocketClient.ReceiveMessageDelegate inReceiveMessageHandler, WebSocketClient.ErrorDelegate inErrorHandler)
        {
            if (_webSocket == null)
            {
                inErrorHandler?.Invoke(new WsClientError(WsClientErrorType.SocketClosed));
                return;
            }

            ArraySegment<byte> buffer = WebSocket.CreateClientBuffer(1024, 1024);
            if (buffer.Array == null)
            {
                inErrorHandler?.Invoke(new WsClientError(WsClientErrorType.CreateReceiveBufferFailed));
                return;
            }

            lock (_lockObject)
            {
                _receiveCancellationTokenSource = new CancellationTokenSource();
            }

            WsClientError wsClientError = null;
            while (_webSocket?.State == WebSocketState.Open)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    WebSocketReceiveResult webSocketReceiveResult = null;
                    do
                    {
                        try
                        {
                            webSocketReceiveResult = await _webSocket.ReceiveAsync(buffer, _receiveCancellationTokenSource.Token);
                            memoryStream.Write(buffer.Array, buffer.Offset, webSocketReceiveResult.Count);
                        }
                        catch (Exception exception)
                        {
                            if (exception is WebSocketException webSocketException)
                            {
                                wsClientError = new WsClientError(WsClientErrorType.CaughtException, webSocketException.Message, webSocketException.ErrorCode);
                            }
                            else
                            {
                                wsClientError = new WsClientError(WsClientErrorType.CaughtException, exception.Message);
                            }

                            break;
                        }
                    } while (!webSocketReceiveResult.EndOfMessage && _webSocket?.State == WebSocketState.Open);

                    if (webSocketReceiveResult == null || wsClientError != null)
                    {
                        break;
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);

                    if (webSocketReceiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
                        {
                            Task<string> readToEndTask = streamReader.ReadToEndAsync();
                            await readToEndTask;
                            
                            inReceiveMessageHandler(readToEndTask.Result);
                        }
                    }
                    else if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        wsClientError = new WsClientError(WsClientErrorType.ReceivedClose);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("NetWebSocket::ReceiveAsync result is binary");
                    }
                }
            }

            if (wsClientError == null)
            {
                wsClientError = new WsClientError(WsClientErrorType.SocketClosed);
            }

            inErrorHandler?.Invoke(wsClientError);
        }

        private async void CloseAsync(WebSocketClient.CloseResultDelegate inResultHandler)
        {
            DisposeConnectCancellationTokenWithBlocking(inWithCancel: true);
            DisposeReceiveCancellationTokenWithBlocking(inWithCancel: true);

            if (_webSocket == null || _webSocket.State != WebSocketState.Open)
            {
                inResultHandler?.Invoke(new WsClientCloseResult(WsClientCloseResult.ResultType.AlreadyClosingOrClosed));
                return;
            }

            WsClientCloseResult.ResultType resultType = WsClientCloseResult.ResultType.Succeeded;
            string resultMessage = string.Empty;
            int nativeErrorCode = WsClientCloseResult.UNKNOWN_NATIVE_ERROR_CODE;
            try
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
            }
            catch (Exception exception)
            {
                resultType = WsClientCloseResult.ResultType.CaughtException;
                resultMessage = exception.Message;
                if (exception is WebSocketException webSocketException)
                {
                    nativeErrorCode = webSocketException.ErrorCode;
                }
            }
            finally
            {
                inResultHandler?.Invoke(new WsClientCloseResult(resultType, resultMessage, nativeErrorCode));
            }
        }

        void IDisposable.Dispose()
        {
            DisposeConnectCancellationTokenWithBlocking(inWithCancel: false);
            DisposeReceiveCancellationTokenWithBlocking(inWithCancel: false);
            _webSocket?.Dispose();
        }

        private void DisposeConnectCancellationTokenWithBlocking(bool inWithCancel)
        {
            if (_connectCancellationTokenSource == null)
                return;

            lock (_lockObject)
            {
                if (_connectCancellationTokenSource.IsCancellationRequested == false && inWithCancel)
                    _connectCancellationTokenSource.Cancel();

                _connectCancellationTokenSource.Dispose();
                _connectCancellationTokenSource = null;
            }
        }

        private void DisposeReceiveCancellationTokenWithBlocking(bool inWithCancel)
        {
            if (_receiveCancellationTokenSource == null)
                return;

            lock (_lockObject)
            {
                if (_receiveCancellationTokenSource.IsCancellationRequested == false && inWithCancel)
                    _receiveCancellationTokenSource.Cancel();

                _receiveCancellationTokenSource.Dispose();
                _receiveCancellationTokenSource = null;
            }
        }
    }
}