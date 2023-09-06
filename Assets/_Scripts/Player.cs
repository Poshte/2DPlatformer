using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    //dependencies
    private Controller2D controller;

    //movement
    private Vector2 playerInput;
    [SerializeField]
    private float moveSpeed = 5f;
    private float velocityXSmoothing;

    //smooth time acceleration
    private readonly float accelerationTimeAirborne = 0.2f;
    private readonly float accelerationTimeGrounded = 0.1f;

    //jumping
    [SerializeField]
    private float maxJumpHeight;
    [SerializeField]
    private float minJumpHeight;
    [SerializeField]
    private float timeToJumpApex;
    private float maxJumpVelocity;
    private float minJumpVelocity;

    //wall jumping
    [SerializeField]
    private float maxWallSlidingSpeed;
    [SerializeField]
    private Vector2 wallJumpFall;
    [SerializeField]
    private Vector2 wallJumpClimb;
    [SerializeField]
    private Vector2 wallJumpLeap;
    private int wallDirection;
    private int inputDirection;
    private bool wallSliding;

    //falling down through a platform
    private bool isCommandButtonDown;
    private readonly float commandButtonHoldDuration = 1f;
    private float commandButtonTimer;

    //jump buffer
    private readonly float jumpBuffer = 0.5f;
    private float bufferCounter;

    //coyote time
    private readonly float coyoteTime = 0.1f;
    private float coyoteCounter;

    private float gravity;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        //math calculations
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        Debug.Log("Gravity: " + gravity + " Jump Velocity: " + maxJumpVelocity);
    }

    void Update()
    {
        //horizontal input and velocity
        var target = playerInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x,
            target,
            ref velocityXSmoothing,
            (controller.collisionDetector.bottomCollision) ? accelerationTimeGrounded : accelerationTimeAirborne);

        //wall sliding prerequisites
        wallSliding = false;
        if (playerInput.x > 0)
        {
            inputDirection = 1;
        }
        else if (playerInput.x < 0)
        {
            inputDirection = -1;
        }
        else
        {
            inputDirection = 0;
        }

        //wall sliding logic
        if ((controller.collisionDetector.leftCollision || controller.collisionDetector.rightCollision) && !controller.collisionDetector.bottomCollision && velocity.y < 0)
        {
            wallSliding = true;
            wallDirection = (controller.collisionDetector.leftCollision) ? -1 : 1;

            if (velocity.y < -maxWallSlidingSpeed && inputDirection == wallDirection)
            {
                velocity.y = -maxWallSlidingSpeed;
            }
        }

        //top and bottom collisions
        if (controller.collisionDetector.topCollision || controller.collisionDetector.bottomCollision)
            velocity.y = 0f;

        //falling down through a platform
        if (playerInput.y == -1)
        {
            commandButtonTimer += Time.deltaTime;
            if (commandButtonTimer >= commandButtonHoldDuration)
            {
                isCommandButtonDown = true;
            }
        }
        else
        {
            isCommandButtonDown = false;
            commandButtonTimer = 0f;
        }

        //manage coyote time
        if (controller.collisionDetector.bottomCollision)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, isCommandButtonDown);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //manage buffer
        if (context.performed)
            bufferCounter = jumpBuffer;
        else
            bufferCounter -= Time.deltaTime;

        //regular jumping max
        if (bufferCounter > 0f && coyoteCounter > 0f)
        {
            velocity.y = maxJumpVelocity;
            bufferCounter = 0f;
        }
        //regular jumping min
        else if (context.canceled && velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
        //wall jumping
        else if (bufferCounter > 0f && wallSliding)
        {
            //falling from wall
            if (inputDirection == 0)
            {
                velocity.x = -wallDirection * wallJumpFall.x;
                velocity.y = wallJumpFall.y;
            }
            //climbing wall
            else if (wallDirection == inputDirection)
            {
                velocity.x = -wallDirection * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            //leaping between walls
            else if (inputDirection == -wallDirection)
            {
                velocity.x = -wallDirection * wallJumpLeap.x;
                velocity.y = wallJumpLeap.y;
            }
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
