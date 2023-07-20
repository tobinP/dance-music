using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftListener : MonoBehaviour
{
    private Transform transform;
    private float oldY;
    private SocketMan socketMan;
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
        socketMan = SingleMan.Instance.SocketMan;
    }

    void Update()
    {
        if (oldY != transform.position.y)
        {
            oldY = transform.position.y;
            socketMan.Send(oldY);
        }
        
    }
}
