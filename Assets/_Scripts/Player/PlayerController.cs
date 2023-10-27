using System.ComponentModel.Design;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveDirection;
    public Rigidbody2D rb;
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


    public float mass = 0.3f;
    public float drag = 800;
    public Vector2 velocity = Vector2.zero;
    private Vector2 path = Vector2.zero;
    private float velMax = 1f;
    private float forceX, forceY;
    private Vector2 timeDragged;
    private float velXInitial, velYInitial;


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
        moveDirection = value.Get<Vector2>();
        // moveDirection = new Vector2(value.Get<Vector2>().x, value.Get<Vector2>().y / 2).normalized;
        forceX = moveDirection.x;
        forceY = moveDirection.y;
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

    public void MoveBy(Vector2 distance)
    {
        rb.position += new Vector2(distance.x, distance.y);
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

    public void MovementLogic() {
        CalculateMovementX(drag);
        CalculateMovementY(drag);
        CorrectVelocitiesAndPaths();
        PerformMovement();
    }

    public void CalculateMovementX(float resistance) {
        if (Math.Abs(velocity.x) < 0.1 && forceX == 0 || velocity.x * forceX < 0) {
            velocity.x = 0;
        }

        if (forceX == 0 && velocity.x != 0) {
            timeDragged.x += Time.fixedDeltaTime;
            path.x = CalculatePathWithDrag(velXInitial, timeDragged.x, resistance);
            velocity.x = path.x / Time.fixedDeltaTime;
            return;
        }

        timeDragged.x = 0;
        velocity.x += forceX / mass * Time.fixedDeltaTime / 2;
        path.x = velocity.x * Time.fixedDeltaTime;
        velocity.x += forceX / mass * Time.fixedDeltaTime / 2;
        velXInitial = velocity.x;
    }

    public void CalculateMovementY(float resistance) {
        if (Math.Abs(velocity.y) < 0.1 && forceY == 0 || velocity.y * forceY < 0) {
            velocity.y = 0;
        }

        if (forceY == 0 && velocity.y != 0) {
            timeDragged.y += Time.fixedDeltaTime;
            path.y = CalculatePathWithDrag(velYInitial, timeDragged.y, resistance);
            velocity.y = path.y / Time.fixedDeltaTime;
            return;
        }

        timeDragged.y = 0;
        velocity.y += forceY / mass * Time.fixedDeltaTime / 2;
        path.y = velocity.y * Time.fixedDeltaTime;
        velocity.y += forceY / mass * Time.fixedDeltaTime / 2;
        velYInitial = velocity.y;
    }

    private float CalculatePathWithDrag(float v0, float time_dragged, float drag) {
        return mass * v0 / drag * (float)(Math.Pow(Math.E, -drag / mass * time_dragged) -
               (float)Math.Pow(Math.E, -drag / mass * (time_dragged + Time.fixedDeltaTime)));
    }

    public void CorrectVelocitiesAndPaths() {
        float velocity_sum = (float)Math.Sqrt(Math.Pow(velocity.x, 2) + Math.Pow(velocity.y, 2));
        if (velocity_sum <= velMax) return;
        float k = velMax / velocity_sum;
        velocity *= k;
        path *= k;
        if (forceX != 0) {
            path.y /= 2;
        }
    }

    public void PerformMovement() {
        StopLogic(path);
    }

    public void StopLogic(Vector2 stopPoint) {
        MoveBy(stopPoint);
        path = Vector2.zero;
    }
}