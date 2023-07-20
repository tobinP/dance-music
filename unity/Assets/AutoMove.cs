using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    Transform transform;
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        Debug.Log("&&& transform: " + transform.position.y);
        
    }
}
