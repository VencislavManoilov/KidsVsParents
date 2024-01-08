const WebSocket = require('ws');

const server = new WebSocket.Server({ port: 3000 });

server.on('connection', (websocket) => {
    console.log('Client connected');

    // Listen for messages from the client
    websocket.on('message', (message) => {
        console.log(`Received message: ${message}`);

        // Send a response back to the client
        websocket.send(`Echo: ${message}`);
    });

    // Handle disconnection
    websocket.on('close', () => {
        console.log('Client disconnected');
    });
});

console.log('WebSocket server started at ws://localhost:3000');