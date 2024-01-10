const WebSocket = require('ws');
const uniqid = require('uniqid');

const server = new WebSocket.Server({ port: 3000 });

let players = []

server.on('connection', (websocket) => {
    websocket.id = uniqid();
    console.log('Client connected');
    websocket.position = [x = randomInteger(0, 20) - 10, y = randomInteger(0, 20) - 10, z = randomInteger(0, 20) - 10];
    players.push({ clientId: websocket.id, position: websocket.position });
    websocket.send(JSON.stringify({ myPos: websocket.position }));

    // Listen for messages from the client
    websocket.on('message', (message) => {
        console.log(`Received message: ${message}`);
    });

    // Handle disconnection
    websocket.on('close', () => {
        removeClient(websocket.id);
        console.log('Client disconnected');
    });
});

function updatePosition() {
    // console.log(server.clients);
    if(server.clients.size > 0) {
        server.clients.forEach((client) => {
            if (client.readyState === WebSocket.OPEN) {
                const otherClientsPositions = players
                    .filter(pos => pos.clientId !== client.id)
                    .map(pos => pos.position);
                console.log("My position: " + players.filter(pos => pos.clientId === client.id).map(pos => pos.position) + "; Others position: " + otherClientsPositions);
                // console.log(otherClientsPositions);
                client.send(JSON.stringify({ clientsPositions: otherClientsPositions }));
            }
        });
    }
}


setInterval(updatePosition, 5000);

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