using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float velocity = 0.5f;
    public float downtime = 0.7f; // seconds

    private float timer = 0;
    public int startingPoint; // starting point index
    public Transform[] points;
    private PlayerController player;

    private Vector3 previousPos;

    private bool playerIsOn = false;
    private int i;

    private void Start()
    {
        transform.position = points[startingPoint].position;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.2f)
        {
            if (timer < downtime)
            {
                timer += Time.fixedDeltaTime;
                return;
            }
            i++;
            timer = 0;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        
        previousPos = transform.position;
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, velocity * Time.fixedDeltaTime);

        if (playerIsOn)
        {
            player.MoveBy(transform.position - previousPos);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIsOn = true;
            Debug.Log("player on");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIsOn = false;
            Debug.Log("player off");
        }
    }
}
