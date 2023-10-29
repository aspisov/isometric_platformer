using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorBehavior : MonoBehaviour
{
    PlayerController player;
    CameraFollow mainCamera;
    WhiteScreenController fog;
    private bool isColliding = false;
    public bool isMoving = false;
    public float velocity = 0.5f;
    public float stairHeight = 0.01f;

    public Transform startPoint;
    public Transform endPoint;

    public enum ElevatorType {
        toPlatform, fromPlatform
    }

    public ElevatorType type;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPoint.position;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        fog = GameObject.FindGameObjectWithTag("WhiteFog").GetComponent<WhiteScreenController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving) {
            MoveToNextLevel();
            return;
        }

        if (player.interactIsPressed && isColliding) {
            ActivateElevator();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            player.rb.position += Vector2.up * stairHeight;
            isColliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") {
            player.rb.position += Vector2.down * stairHeight;
            isColliding = false;
        }
    }

    void ActivateElevator() {
        isMoving = true;
        player.interactIsPressed = false;
        if (type == ElevatorType.fromPlatform) {
            StartCoroutine(mainCamera.ZoomIn());    
        }
        player.SwitchState(player.elevateState);
    }

    void MoveToNextLevel() {
        if (Vector2.Distance(transform.position, endPoint.position) < 0.02f)
        {
            if (SceneManager.GetActiveScene().name == "Floor 0") {
                SceneManager.LoadScene("Floor 1");
            }
            player.SwitchState(player.idleState);
            isMoving = false;
            return;
        }

        Vector3 previousPos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, endPoint.position, velocity * Time.deltaTime);
        player.transform.position += transform.position - previousPos;
        fog.SetOpacity(CalculatePercentageComplete());
    }

    float CalculatePercentageComplete() {
        float wholePath = Math.Abs(endPoint.position.y - startPoint.position.y);
        float completePath = Math.Abs(transform.position.y - startPoint.position.y);
        if (type == ElevatorType.fromPlatform) {
            return completePath / wholePath;
        }
        return 1 - completePath / wholePath;
    }
}
