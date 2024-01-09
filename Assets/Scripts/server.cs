using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;

public class server : MonoBehaviour
{

    private WebSocket ws;

    void Start()
    {
        ws = new WebSocket("ws://localhost:3000");

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Received: " + e.Data);
            // Handle the received message here
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