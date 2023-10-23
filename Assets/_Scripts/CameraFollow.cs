using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    float cameraVelocity = 5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
        transform.position = Vector3.Slerp(transform.position, targetPosition, cameraVelocity * Time.fixedDeltaTime);
    }
}
