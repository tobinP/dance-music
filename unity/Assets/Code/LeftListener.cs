using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftListener : MonoBehaviour
{
    public bool isSending = false;
    public float oldY;
    private Transform transform;
    private SocketMan socketMan;
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
        socketMan = SingleMan.Instance.SocketMan;
    }

    void Update()
    {
        if (!isSending) return;

        if (oldY != transform.position.y)
        {
            oldY = transform.position.y;
            // Debug.Log($"&&& sending: {oldY}");
            socketMan.Send(oldY);
        }
        
    }
}
