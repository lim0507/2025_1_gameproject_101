using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollw : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 0.125f;

    
    void Update()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position smoothPosition;

        transform.LookAt(transform.position);
    }
}
