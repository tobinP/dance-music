using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleMan : MonoBehaviour
{
    public static SingleMan Instance { get; private set; }
    public SocketMan SocketMan { get; private set; }
    public bool sendMessage = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("&&& SingleMan Destroyed!!!!!!!!!!!!!!!!!!!");
            Destroy(this);
            return;
        }
        Instance = this;
        SocketMan = SocketMan.Instance;
    }

    void Update() 
    {
        if (sendMessage)
        {
            sendMessage = false;
        }
    }
}
