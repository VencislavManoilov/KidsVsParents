using System.Dynamic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;

public class server : MonoBehaviour
{
    public GameObject player;
    private GameObject[] players;

    public GameObject playerPrefab;

    private float x1;
    private float y1;
    private float z1;

    private bool spawn = false;

    private WebSocket ws;

    private void Start()
    {
        ws = new WebSocket("ws://localhost:3000");
        players = new GameObject[4];

        ws.OnMessage += HandleWebSocketMessage;


        ws.Connect();
    }

    void HandleWebSocketMessage(object sender, MessageEventArgs e) {
        dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(e.Data);

        UpdatePlayersPositions(data.clientsPositions);
        UpdateMyPosition(data.myPos);
        Debug.Log(data.clientsPositions);
    }

    void UpdateMyPosition(dynamic positionData) {
        if(positionData != null && positionData.Count == 3) {
            x1 = positionData[0];
            y1 = positionData[1];
            z1 = positionData[2];
            spawn = true;
        }
    }

    // THIS FUNCTION NEVER RUNS!!!!
    // FIX FIX FIX
    void UpdatePlayersPositions(dynamic playersPos) {
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
        if (ws != null && ws.IsAlive)
        {
            Debug.Log("Bye WebSocket!");
            ws.Close();
        }
    }

    void Update() {
        if(spawn) {
            player.transform.position = new Vector3(x1, y1, z1);
            spawn = false;
        }
    }
}