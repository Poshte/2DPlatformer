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
    private readonly float wallStickBuffer = 0.25f;
    private float wallStick;

    private float gravity;
    private Vector3 velocity;

    //falling down through a platform
    private bool isCommandButtonDown;
    private readonly float commandButtonHoldDuration = 1f;
    private float commandButtonTimer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();

        //math calculations
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        Debug.Log("Gravity: " + gravity + " Jump Velocity: " + maxJumpVelocity);
    }

    // Update is called once per frame
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

        //buffer for leaping between walls
        if (wallSliding)
        {
            wallStick = wallStickBuffer;
        }
        else
        {
            wallStick -= Time.deltaTime;
        }

        //top and bottom collisions
        if (controller.collisionDetector.topCollision || controller.collisionDetector.bottomCollision)
            velocity.y = 0f;

        //falling down through a platform
        Debug.Log("down command: " + isCommandButtonDown);
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

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, isCommandButtonDown);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //regular jumping max
        if (context.performed && controller.collisionDetector.bottomCollision)
        {
            velocity.y = maxJumpVelocity;
        }
        //regular jumping min
        else if (context.canceled && velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
        //wall jumping
        else if (context.performed && wallSliding)
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
        }

        //leaping between walls
        if (context.performed && wallStick > 0 && inputDirection == -wallDirection)
        {
            velocity.x = -wallDirection * wallJumpLeap.x;
            velocity.y = wallJumpLeap.y;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
