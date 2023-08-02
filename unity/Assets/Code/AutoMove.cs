using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    [Range(0.0f, 5.0f)]
    public float speed;
    private Transform transform;
    private int direction = 1;
    private int offset = 1;

    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        float newY = Mathf.Sin(Time.time * speed) + offset;
        transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
