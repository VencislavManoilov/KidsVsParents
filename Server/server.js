const WebSocket = require('ws');

const server = new WebSocket.Server({ port: 3000 });

let players = []

server.on('connection', (websocket) => {
    console.log('Client connected');
    websocket.position = [x = randomInteger(-10, 10), y = randomInteger(-10, 10), z = randomInteger(-10, 10)];
    players.push({ clientId: websocket._socket.remoteAddress, position: websocket.position });
    websocket.send(JSON.stringify({ myPos: websocket.position }));

    // Listen for messages from the client
    websocket.on('message', (message) => {
        console.log(`Received message: ${message}`);
    });

    // Handle disconnection
    websocket.on('close', () => {
        removeClient(websocket._socket.remoteAddress);
        console.log('Client disconnected');
    });
});

// function updatePosition() {
//     server.clients.forEach((client) => {
//         if (client.readyState === WebSocket.OPEN) {
//             client.send(/* Here I want to send stringified object that has one variable that is an array of all clients positions exept the current client. */);
//         }
//     });
// }

function updatePosition() {
    server.clients.forEach((client) => {
        if (client.readyState === WebSocket.OPEN) {
            const otherClientsPositions = players.filter(pos => pos.clientId !== client._socket.remoteAddress);
            client.send(JSON.stringify({ clientsPositions: otherClientsPositions }));
        }
    });
}

setInterval(updatePosition, 1000);

console.log('WebSocket server started at ws://localhost:3000');

function removeClient(clientId) {
    const index = players.findIndex(pos => pos.clientId === clientId);
    if (index !== -1) {
        players.splice(index, 1);
    }
}

function randomInteger(min, max) {
    return Math.floor(Math.random() * max) + min;
}