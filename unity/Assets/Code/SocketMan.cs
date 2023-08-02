using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class SocketMan
{
    public static SocketMan Instance
    {
        get { return instance; }
    }
    private static readonly SocketMan instance = new SocketMan();
    private WebSocket websocket;
    private string jsonString;

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static SocketMan() { }

    private SocketMan()
    {
        var testObj = new TestObj { name = "SocketMan", xVal = 5 };
        jsonString = JsonUtility.ToJson(testObj);
        Debug.Log("&&& SocketMan");
        websocket = new WebSocket("ws://192.168.0.52:9090");

        websocket.OnOpen += () =>
        {
            Debug.Log("&&& Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("&&& Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("&&& Connection closed!");
        };
        websocket.Connect();

        Send();
    }

    public void Send()
    {
        if (websocket.State == WebSocketState.Open)
        {
            Debug.Log("&&& sending");
            websocket.SendText(jsonString);
        }
    }

    public void Send(float yVal)
    {
        if (websocket.State == WebSocketState.Open)
        {
            var testObj = new TestObj { name = "SocketMan", xVal = yVal };
            jsonString = JsonUtility.ToJson(testObj);
            // Debug.Log("&&& sending:", yVal);
            websocket.SendText(jsonString);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}
