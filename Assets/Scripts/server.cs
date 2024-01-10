using System.Dynamic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;

public class server : MonoBehaviour
{
    public GameObject player;
    private GameObject[] players;

    public GameObject playerPrefab;

    private WebSocket ws;

    private void Start()
    {
        ws = new WebSocket("ws://localhost:3000");
        players = new GameObject[4];

        ws.OnMessage += HandleWebSocketMessage;


        ws.Connect();
    }

    private void HandleWebSocketMessage(object sender, MessageEventArgs e) {
        dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(e.Data);

        UpdateMyPosition(data.myPos);
        UpdatePlayerPositions(data.clientsPositions);
    }

    private void UpdateMyPosition(dynamic positionData) {
        if(positionData != null && positionData.Count == 3) {
            player.transform.position = new Vector3(positionData[0], positionData[1], positionData[2]);
        }
    }

    private void UpdatePlayerPositions(dynamic playersPos) {
        if(playersPos != null && players != null && playersPos.clientsPositions != null) {
            if (players.Length < playersPos.clientsPositions.Length) {
                players = new GameObject[playersPos.clientsPositions.Length];
            }

            for (int i = 0; i < playersPos.clientsPositions.Length; i++) {
                Vector3 position = playersPos.clientsPositions[i];
                players[i] = Instantiate(playerPrefab, position, playerPrefab.transform.rotation);
            }
        }
    }

    private void OnDestroy()
    {
        // Make sure to close the WebSocket connection when the script is destroyed
        if (ws != null && ws.IsAlive)
        {
            Debug.Log("Bye WebSocket!");
            ws.Close();
        }
    }
}