using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorBehavior : MonoBehaviour
{
    PlayerController player;
    CameraFollow mainCamera;
    private bool isColliding = false;
    public bool isMoving = false;
    public float velocity = 0.5f;
    public float stairHeight = 0.01f;

    public Transform startPoint;
    public Transform endPoint;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        transform.position = startPoint.position;
    }

    // Update is called once per frame
    void Update()
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
        // StartCoroutine(mainCamera.ZoomIn());
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
    }
}
