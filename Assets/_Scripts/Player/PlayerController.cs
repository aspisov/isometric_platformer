using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveDirection;
    public Rigidbody2D rb;

    public float velocity = 1f;
    public bool isColliding = true;
    public bool jumpIsPressed = false;
    public bool interactIsPressed = false;

    private Vector2 respawnPos;

    public Animator animator;
    DisappearingPlatformBehavior[] disappearingPlatforms;
    public SpriteRenderer sprite;

    BaseState currentState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerFallState fallState = new PlayerFallState();
    public PlayerElevateState elevateState = new PlayerElevateState();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        disappearingPlatforms = FindObjectsOfType<DisappearingPlatformBehavior>();

        currentState = idleState;
        currentState.EnterState(this);
    }

    private void FixedUpdate()
    {
        currentState.UpdateState(this);    

        adjustFacingDirection();
    }

    private void OnMove(InputValue value)
    {
        // adjusting movement direction for isometric prospective
        moveDirection = new Vector2(value.Get<Vector2>().x, value.Get<Vector2>().y / 2).normalized;
    }

    private void OnJump()
    {
        if (currentState != jumpState) {
            jumpIsPressed = true;
        }
    }

    private void OnInteract() {
        interactIsPressed = true;
    }

    private void EndJump() => SwitchState(idleState);

    public void MoveBy(Vector3 distance)
    {
        if (currentState != jumpState) {
            rb.position += new Vector2(distance.x, distance.y);
        }
    }

    public void SetRespawnPosition(Vector2 pos) => respawnPos = pos;

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

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this);
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

    public void Reset()
    {
        transform.position = respawnPos;
        sprite.sortingOrder = 0;
        jumpIsPressed = false;
        SwitchState(idleState);


        // Enable or activate each object found.
        foreach (var obj in disappearingPlatforms)
        {
            obj.Reset();
        }
    }
}