using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    PlayerController player;
    CameraFollow mainCamera;
    ElevatorBehavior elevator;
    float timer = 0;

    private bool isMovingToNextScene = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingToNextScene) {
            timer += Time.deltaTime;
            Vector3 distance = new Vector3(0, 0.1f, 0);

            player.transform.position = Vector3.Slerp(player.transform.position, player.transform.position + distance, 3f * Time.deltaTime);
            elevator.transform.position = Vector3.Slerp(elevator.transform.position, elevator.transform.position + distance, 3f * Time.deltaTime);
            if (timer > 5) {
                SceneManager.LoadScene("Floor 1");
            }
        }
    }

    void FixedUpdate()
    {
        
    }

    public void NextScene(ElevatorBehavior elevator) {
        StartCoroutine(mainCamera.ZoomIn());
        isMovingToNextScene = true;
        this.elevator = elevator;
        player.SwitchState(player.elevateState);
    }
}
