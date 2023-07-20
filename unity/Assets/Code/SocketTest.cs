using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class TestObj
{
    public string name;
    public float xVal;
}

public class SocketTest : MonoBehaviour
{
    WebSocket websocket;
    string jsonString;

    async void Start()
    {
        var testObj = new TestObj { name = "test name", xVal = 5 };
        jsonString = JsonUtility.ToJson(testObj);
        Debug.Log(jsonString);

        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };
        InvokeRepeating("SendWebSocketMessage", 0.0f, 1.0f);

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(jsonString);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
