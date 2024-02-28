using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
	//dependencies
	private Controller2D controller2D;

	//movement
	private Vector2 playerInput;
	[SerializeField]
	private float moveSpeed = 5f;
	private float velocityXSmoothing;

	//smooth time acceleration
	private const float accelerationTimeAirborne = 0.2f;
	private const float accelerationTimeGrounded = 0.1f;

	//jumping
	[SerializeField]
	private float maxJumpHeight;
	[SerializeField]
	private float minJumpHeight;
	[SerializeField]
	private float timeToJumpApex;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private const float slidingJumpY = 8f;
	private const float slidingJumpX = -2f;
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
	//private bool wallSliding;

	//falling down through a platform
	private bool isCommandButtonDown;
	private const float commandButtonHoldDuration = 0.5f;
	private float commandButtonTimer;

	//jump buffer
	private const float jumpBuffer = 2f;
	private float bufferCounter;

	//coyote time
	private const float coyoteTime = 0.1f;
	private float coyoteCounter;

	private float gravity;
	private Vector2 velocity;

	private const int jumpsAllowed = 1;
	private int jumpCounter;

	void Awake()
	{
		controller2D = GetComponent<Controller2D>();
	}
	void Start()
	{
		//math calculations
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
	}

	void Update()
	{
		//manage input direction
		if (playerInput.x > 0)
			inputDirection = 1;
		else if (playerInput.x < 0)
			inputDirection = -1;
		else
			inputDirection = 0;

		CalculateVelocity();
		FallThroughPlatform();
		//HandleWallSliding();

		//manage coyote time
		if (controller2D.info.bottomCollision)
		{
			coyoteCounter = coyoteTime;
			jumpCounter = 0;
		}
		else
			coyoteCounter -= Time.deltaTime;


		//bufferCounter -= Time.deltaTime;


		controller2D.Move(velocity * Time.deltaTime, isCommandButtonDown);

		//top and bottom collisions
		if (controller2D.info.topCollision || controller2D.info.bottomCollision)
			if (controller2D.info.slidingMaxSlope)
				velocity.y += controller2D.info.slopeNormal.y * Mathf.Abs(gravity) * Time.deltaTime;
			else
				velocity.y = 0f;
	}

	public void CalculateVelocity()
	{
		var target = playerInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, target, ref velocityXSmoothing, controller2D.info.bottomCollision ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}

	public void FallThroughPlatform()
	{
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
	}

	//public void HandleWallSliding()
	//{
	//    wallSliding = false;
	//    
	//    if ((controller2D.info.leftCollision || controller2D.info.rightCollision) && !controller2D.info.bottomCollision && velocity.y < 0)
	//    {
	//        wallSliding = true;
	//        wallDirection = (controller2D.info.leftCollision) ? -1 : 1;

	//        if (velocity.y < -maxWallSlidingSpeed && inputDirection == wallDirection)
	//        {
	//            velocity.y = -maxWallSlidingSpeed;
	//        }
	//    }
	//}

	public void GetMovementInput(Vector2 movementInput)
	{
		playerInput = movementInput;
	}

	public void GetJumpInput(bool performed, bool canceled)
	{
		if (performed)
			bufferCounter = jumpBuffer;

		//regular jumping max
		if (bufferCounter > 0f && coyoteCounter > 0f && jumpCounter < jumpsAllowed)
		{
			if (controller2D.info.slidingMaxSlope)
			{
				if (inputDirection != -Mathf.Sign(controller2D.info.slopeNormal.x))
				{
					velocity.y = (maxJumpVelocity + slidingJumpY) * controller2D.info.slopeNormal.y;
					velocity.x = (maxJumpVelocity + slidingJumpX) * controller2D.info.slopeNormal.x;
				}
			}
			else
			{
				velocity.y = maxJumpVelocity;
			}

			controller2D.info.bottomCollision = false;
			bufferCounter = 0f;
			jumpCounter++;
		}
		//regular jumping min
		else if (canceled && velocity.y > minJumpVelocity && jumpCounter < jumpsAllowed /*&& coyoteCounter > 0f*/)
		{
			velocity.y = minJumpVelocity;

			controller2D.info.bottomCollision = false;
			bufferCounter = 0f;
			jumpCounter++;
		}
		////wall jumping
		//else if (bufferCounter > 0f && wallSliding)
		//{
		//	//falling from wall
		//	if (inputDirection == 0)
		//	{
		//		velocity.x = -wallDirection * wallJumpFall.x;
		//		velocity.y = wallJumpFall.y;
		//	}
		//	//climbing wall
		//	else if (wallDirection == inputDirection)
		//	{
		//		velocity.x = -wallDirection * wallJumpClimb.x;
		//		velocity.y = wallJumpClimb.y;
		//	}
		//	//leaping between walls
		//	else if (inputDirection == -wallDirection)
		//	{
		//		velocity.x = -wallDirection * wallJumpLeap.x;
		//		velocity.y = wallJumpLeap.y;
		//	}
		//}

		controller2D.Move(velocity * Time.deltaTime);
	}

	public Vector2 GetPlayerVelocity()
	{
		return velocity;
	}
}
