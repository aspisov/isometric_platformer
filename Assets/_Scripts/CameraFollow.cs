using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera mainCamera;

    public Transform target;
    float cameraVelocity = 5f;

    public float targetOrthographicSize = 0.5f; // Adjust the target orthographic size.
    public float zoomSpeed = 1.0f; // Adjust the zoom speed.

    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, -10);
        transform.position = Vector3.Slerp(transform.position, targetPosition, cameraVelocity * Time.fixedDeltaTime);
    }

    public IEnumerator ZoomIn() {
        float initialOrthographicSize = mainCamera.orthographicSize;

        while (mainCamera.orthographicSize > targetOrthographicSize)
        {
            mainCamera.orthographicSize -= zoomSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
