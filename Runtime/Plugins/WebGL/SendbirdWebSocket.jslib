const SendbirdWebSocketBridge = {
    $bridgeContext: {
        clients: {},
        clientIdCounter: 0,
        onOpenCallback: null,
        onMessageCallback: null,
        onErrorCallback: null,
        onCloseCallback: null,
        invokeOnOpenCallback: function (clientId) {
            if (bridgeContext.onOpenCallback) {
                Module.dynCall_vi(bridgeContext.onOpenCallback, clientId);
            }
        },

        invokeOnMessageCallback: function (clientId, stringPtr) {
            if (bridgeContext.onMessageCallback) {
                Module.dynCall_vii(bridgeContext.onMessageCallback, clientId, stringPtr);
            }
        },

        invokeOnErrorCallback: function (clientId) {
            if (bridgeContext.onErrorCallback) {
                Module.dynCall_vi(bridgeContext.onErrorCallback, clientId);
            }
        },

        invokeOnCloseCallback: function (clientId) {
            if (bridgeContext.onCloseCallback) {
                Module.dynCall_vi(bridgeContext.onCloseCallback, clientId);
            }
        },
    },

    SendbirdWebSocketBridge_RegisterCallbacks: function (onOpen, onMessage, onError, onClose) {
        bridgeContext.onOpenCallback = onOpen;
        bridgeContext.onMessageCallback = onMessage;
        bridgeContext.onErrorCallback = onError;
        bridgeContext.onCloseCallback = onClose;
    },

    // Create a new WebSocket client and return its unique ID
    SendbirdWebSocketBridge_CreateWebSocketClient: function () {
        if (bridgeContext.clientIdCounter >= Number.MAX_SAFE_INTEGER) {
            bridgeContext.clientIdCounter = 0;
        }

        const clientId = bridgeContext.clientIdCounter++;
        bridgeContext.clients[clientId] = {
            clientId: clientId,
            webSocket: null
        };
        return clientId;
    },

    // Delete the WebSocket client by ID
    SendbirdWebSocketBridge_DeleteWebSocketClient: function (clientId) {
        if (bridgeContext.clients[clientId] != null) {
            delete bridgeContext.clients[clientId];
        } else {
            console.warn('DeleteWebSocketClient WebSocket client not found with ID: ' + clientId);
        }
    },

    SendbirdWebSocketBridge_Connect: function (clientId, uriStringPtr, encodedProtocolsStringPtr) {
        const client = bridgeContext.clients[clientId];
        if (!client) {
            console.error('SendbirdWebSocketBridge_Connect client not found with ID: ' + clientId);
            bridgeContext.invokeOnErrorCallback(clientId);
            return;
        }

        if (client.webSocket != null && client.webSocket.readyState === WebSocket.OPEN) {
            console.warn('WebSocket is already connected.');
            return;
        }

        console.warn('SendbirdWebSocketBridge_Connect protocols: ' + UTF8ToString(encodedProtocolsStringPtr));
        client.webSocket = new WebSocket(UTF8ToString(uriStringPtr), UTF8ToString(encodedProtocolsStringPtr));

        client.webSocket.onopen = () => {
            console.log('WebSocket connection opened for client: ' + client.clientId);
            bridgeContext.invokeOnOpenCallback(client.clientId);
        };

        client.webSocket.onmessage = (event) => {
            console.log('WebSocket message received: ', event.data);
            if (typeof event.data === 'string') {
                var lengthBytes = lengthBytesUTF8(event.data) + 1;
                var stringPtr = _malloc(lengthBytes);
                stringToUTF8(event.data, stringPtr, lengthBytes);
                bridgeContext.invokeOnMessageCallback(client.clientId, stringPtr);
            } else {
                console.warn("WebSocket received invalid message type:", (typeof event.data));
            }
        };

        client.webSocket.onerror = () => {
            console.error('WebSocket error for client: ' + client.clientId);
            bridgeContext.invokeOnErrorCallback(client.clientId);
        };

        client.webSocket.onclose = () => {
            console.log('WebSocket connection closed for client: ' + client.clientId);
            bridgeContext.invokeOnCloseCallback(client.clientId);
        };
    },

    SendbirdWebSocketBridge_Send: function (clientId, messageStringPtr) {
        const client = bridgeContext.clients[clientId];
        if (!client) {
            bridgeContext.invokeOnErrorCallback(clientId);
            console.error('SendbirdWebSocketBridge_Send client not found with ID: ' + clientId);
            return;
        }

        if (client.webSocket != null && client.webSocket.readyState === WebSocket.OPEN) {
            client.webSocket.send(UTF8ToString(messageStringPtr));
        } else {
            console.error('WebSocket is not open. Cannot send message for client: ' + client.clientId);
            bridgeContext.invokeOnErrorCallback(clientId);
        }
    },

    SendbirdWebSocketBridge_Close: function (clientId) {
        const client = bridgeContext.clients[clientId];
        if (!client) {
            console.warn('SendbirdWebSocketBridge_Send client not found with ID: ' + clientId);
            bridgeContext.invokeOnErrorCallback(clientId);
            return;
        }

        if (client.webSocket != null && client.webSocket.readyState !== WebSocket.CLOSING && client.webSocket.readyState !== WebSocket.CLOSED) {
            client.webSocket.Close();
        } else {
            console.warn('Close WebSocket client not found with ID: ' + clientId);
            bridgeContext.invokeOnErrorCallback(clientId);
        }
    },
};

autoAddDeps(SendbirdWebSocketBridge, '$bridgeContext');
mergeInto(LibraryManager.library, SendbirdWebSocketBridge);