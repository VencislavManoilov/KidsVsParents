using System;
using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;

public class server : MonoBehaviour
{
    public GameObject player;
    private GameObject[] players;

    public GameObject playerPrefab;

    private WebSocket ws;

    void Start()
    {
        ws = new WebSocket("ws://localhost:3000");

        ws.OnMessage += (sender, e) =>
        {
            dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(e.Data);
            // The program doesn't go here for some reason
            if(data.myPos) {
                // player.transform.position = new Vector3(data.myPos[0], data.myPos[1], data.myPos[2]);
                player.transform.position = new Vector3(2, 3, 4);
            }

            if(data.clientsPositions) {
                foreach(GameObject player in players) {
                    Destroy(player);
                }

                foreach(Vector3 position in data.clientsPositions) {
                    players[players.Length] = Instantiate(playerPrefab, position, playerPrefab.transform.rotation);
                }
            }
        };

        ws.Connect();
    }

    void Update()
    {
        // You can send messages in the Update method or as needed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Replace "Hello WebSocket!" with your message
            ws.Send("Hello from Unity");
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