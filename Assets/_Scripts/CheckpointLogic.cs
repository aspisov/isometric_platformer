using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckpointLogic : MonoBehaviour
{
    private bool isActive = false;
    public Light2D light;
    public GameObject particles;
    private PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive && other.tag == "Player") {
            isActive = true;
            light.enabled = true;
            particles.SetActive(true);
            player.SetRespawnPosition(transform.position);
            Debug.Log("player entered Checkpoint");
        }
    }
}
