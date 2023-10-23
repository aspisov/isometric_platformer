using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDirection;
    Rigidbody2D rb;

    public float velocity = 1f;

    public enum State { idle, run, jump, fall };
    public State currentState = State.idle;

    private bool isColliding = true;

    private float fallTimer;
    private Vector2 respawnPos;

    Animator animator;
    DisappearingPlatformBehavior[] disappearingPlatforms;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        disappearingPlatforms = FindObjectsOfType<DisappearingPlatformBehavior>();
    }

    private void FixedUpdate()
    {
        if (currentState == State.fall)
        {
            fallLogic();
            return;
        }
        
        rb.position += moveDirection * velocity * Time.fixedDeltaTime;

        // fixing animations
        if (currentState == State.run && moveDirection == Vector2.zero) {
            newState(State.idle);
        } else if (currentState == State.idle && moveDirection != Vector2.zero) {
            newState(State.run);
        }

        adjustFacingDirection();
        checkForCollisions();

    }

    private void OnMove(InputValue value)
    {
        // adjusting movement direction for isometric prospective
        moveDirection = new Vector2(value.Get<Vector2>().x, value.Get<Vector2>().y / 2).normalized;
    }

    private void OnJump()
    {
        if (currentState != State.fall) {
            newState(State.jump);
        }
    }

    public void MoveBy(Vector3 distance)
    {
        if (currentState != State.jump)
        {
            rb.position += new Vector2(distance.x, distance.y);
        }
    }

    public void SetRespawnPosition(Vector2 pos) {
        respawnPos = pos;
    }

    private void adjustFacingDirection()
    {
        if (moveDirection.x > 0)
        {
            sprite.flipX = false;
        }
        else if (moveDirection.x < 0)
        {
            sprite.flipX = true;
        }
    }

    private void checkForCollisions()
    {
        if (!isColliding && currentState != State.jump)
        {
            newState(State.fall);
            // animator.Play("idle");
            sprite.sortingOrder = -2;
        }
    }

    private void newState(State state)
    {
        if (currentState == state) return;

        currentState = state;
        switch (state) {
            case State.idle:
                animator.Play("idle");
                break;
            case State.run:
                animator.Play("run");
                break;
            case State.jump:
                animator.Play("jump");
                break;
            case State.fall:
                animator.Play("idle");
                break;
        }
    }

    private void fallLogic()
    {
        fallTimer += Time.fixedDeltaTime;
        transform.position -= new Vector3(0, 0.05f, 0);
        if (fallTimer >= 0.5)
        {
            Reset();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground") {
            isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground") {
            isColliding = false;
        }
    }

    private void Reset()
    {
        fallTimer = 0;
        transform.position = respawnPos;
        sprite.sortingOrder = 0;
        newState(State.idle);


        // Enable or activate each object found.
        foreach (var obj in disappearingPlatforms)
        {
            obj.Reset();
        }
    }
}
