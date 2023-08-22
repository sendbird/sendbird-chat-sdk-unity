// 
//  Copyright (c) 2023 Sendbird, Inc.
// 


using System;

namespace Sendbird.Chat
{
    internal sealed class ConnectionPingManager
    {
        private CommandRouter _commandRouterRef = null;
        private double _pingInterval = NetworkConfig.DEFAULT_PING_INTERVAL;
        private double _pongTimeout = NetworkConfig.DEFAULT_PONG_TIMEOUT;
        private bool _isSendAblePing = false;
        private double _lastSentOrReceivedTime = 0;
        private bool _isWaitingForPongTimeout = false;
        private double _pongWaitingStartTime = 0;
        private bool _isActiveSdk = true;
        private Action _onPongTimeoutHandler = null;

        internal void Initialize(CommandRouter inCommandRouter, Action inPongTimeoutHandler)
        {
            _commandRouterRef = inCommandRouter;
            _onPongTimeoutHandler = inPongTimeoutHandler;
        }

        internal void Terminate()
        {
            _commandRouterRef = null;
        }

        internal void Update()
        {
            SendPingIfAble();
            CheckPongTimeout();
        }

        internal void OnChangeConnectionState(ConnectionStateInternalType inChangedStateType, ConnectionStateAbstract.ParamsAbstract inChangedStateParams)
        {
            if (inChangedStateType == ConnectionStateInternalType.Connected)
            {
                if (inChangedStateParams is ConnectedConnectionState.Params connectedStateParams && connectedStateParams.LogiWsReceiveCommand != null)
                {
                    _pingInterval = connectedStateParams.LogiWsReceiveCommand.pingInterval;
                    _pongTimeout = connectedStateParams.LogiWsReceiveCommand.pongTimeout;

                    if (_pingInterval <= 0)
                        _pingInterval = NetworkConfig.DEFAULT_PING_INTERVAL;

                    if (_pongTimeout <= 0)
                        _pongTimeout = NetworkConfig.DEFAULT_PONG_TIMEOUT;
                }

                StartPing();
            }
            else if (inChangedStateType == ConnectionStateInternalType.Disconnected)
            {
                StopPing();
            }
        }

        internal void OnReceiveWsEventCommand()
        {
            _lastSentOrReceivedTime = GetCurrentTime();
            StopPongTimer();
        }

        internal void OnChangeSdkActiveState(bool inIsActive)
        {
            _isActiveSdk = inIsActive;
        }

        private void StartPing()
        {
            _isSendAblePing = true;
            _lastSentOrReceivedTime = long.MinValue;
            SendPingIfAble();
        }

        private void StopPing()
        {
            _isSendAblePing = false;
        }

        private void SendPingIfAble()
        {
            if (_isSendAblePing == false || _commandRouterRef == null)
                return;
            
            double currentTime = GetCurrentTime();
            double diff = currentTime - _lastSentOrReceivedTime;
            if (diff <= _pingInterval)
                return;

            _lastSentOrReceivedTime = GetCurrentTime();
            PingWsSendCommand pingWsSendCommand = new PingWsSendCommand(_isActiveSdk);
            _commandRouterRef.SendWsCommand(pingWsSendCommand);
            StartPongTimer();
        }

        private void StartPongTimer()
        {
            _isWaitingForPongTimeout = true;
            _pongWaitingStartTime = GetCurrentTime();
        }

        private void StopPongTimer()
        {
            _isWaitingForPongTimeout = false;
        }

        private void CheckPongTimeout()
        {
            if (_isWaitingForPongTimeout)
            {
                double currentTime = GetCurrentTime();
                double diff = currentTime - _pongWaitingStartTime;
                if (diff <= _pongTimeout)
                    return;

                _isWaitingForPongTimeout = false;
            }
        }
        
        private double GetCurrentTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 0.001;
        }
    }
}