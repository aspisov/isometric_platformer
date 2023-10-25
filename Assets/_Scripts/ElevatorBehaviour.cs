using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBehavior : MonoBehaviour
{
    GameManagerScript manager;
    PlayerController player;
    private bool isCollidingWithPlayer = false;
    public float stairHeight = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.interactIsPressed && isCollidingWithPlayer) {
            manager.NextScene(this);
            player.interactIsPressed = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            player.rb.position += Vector2.up * stairHeight;
            isCollidingWithPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") {
            player.rb.position += Vector2.down * stairHeight;
            isCollidingWithPlayer = false;
        }
    }
}
