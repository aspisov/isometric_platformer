using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatformBehavior : MonoBehaviour
{
    Animator animator;

    public float duration = 3f; // seconds
    private float timer = 0;
    private bool stepped = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (stepped)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= duration)
            {
                Hide();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            stepped = true;
            animator.Play("disappear");
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        gameObject.SetActive(true);
        timer = 0;
        stepped = false;
        animator.Play("idle");
    }
}
