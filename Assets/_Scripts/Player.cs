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
	private Vector2 horizontalInput;

	[SerializeField]
	private float moveSpeed = 5f;
	private float velocityXSmoothing;

	//smooth time acceleration
	private float accelerationTimeAirborne = 0.2f;
	private float accelerationTimeGrounded = 0.1f;

	//jumping
	[SerializeField]
	private float maxJumpHeight;
	[SerializeField]
	private float minJumpHeight;
	[SerializeField]
	private float timeToJumpApex;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private float gravity;

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
	private readonly float wallStickBuffer = 1f;
	private float wallStick;

	private Vector3 velocity;

	// Start is called before the first frame update
	void Start()
	{
		controller = GetComponent<Controller2D>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		Debug.Log("Gravity: " + gravity + " Jump Velocity: " + maxJumpVelocity);
	}

	// Update is called once per frame
	void Update()
	{
		var target = horizontalInput.x * moveSpeed;

		velocity.x = Mathf.SmoothDamp(velocity.x,
			target,
			ref velocityXSmoothing,
			(controller.collisionDetector.bottomCollision) ? accelerationTimeGrounded : accelerationTimeAirborne);


		wallSliding = false;

		if (horizontalInput.x > 0)
		{
			inputDirection = 1;
		}
		else if (horizontalInput.x < 0)
		{
			inputDirection = -1;
		}
		else
		{
			inputDirection = 0;
		}

		if ((controller.collisionDetector.leftCollision || controller.collisionDetector.rightCollision) && !controller.collisionDetector.bottomCollision && velocity.y < 0)
		{
			wallSliding = true;
			wallDirection = (controller.collisionDetector.leftCollision) ? -1 : 1;

			if (velocity.y < -maxWallSlidingSpeed && inputDirection == wallDirection)
			{
				velocity.y = -maxWallSlidingSpeed;
			}

			//	if ()
			//	{
			//		velocity.x = 0f;
			//		velocityXSmoothing = 0f;

			//		if (inputDirection != 0 && inputDirection != wallDirection)
			//		{
			//			wallUnstick -= Time.deltaTime;
			//		}
			//		else
			//		{
			//			wallUnstick = wallStickTime;
			//		}
			//	}
			//	else
			//	{
			//		wallUnstick = wallStickTime;
			//	}
		}
		if (wallSliding)
		{
			wallStick = wallStickBuffer;
		}
		else
		{
			wallStick = -Time.deltaTime;
		}

		if (controller.collisionDetector.topCollision || controller.collisionDetector.bottomCollision)
			velocity.y = 0f;
		
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}

	public void Movement(InputAction.CallbackContext context)
	{
		horizontalInput = context.ReadValue<Vector2>();
	}

	public void Jump(InputAction.CallbackContext context)
	{
		//regular jumping max
		if (context.performed && controller.collisionDetector.bottomCollision)
		{
			velocity.y = maxJumpVelocity;
		}
		else if (context.canceled && velocity.y > minJumpVelocity)
		{
			velocity.y = minJumpVelocity;
		}
		//wall jumping
		else if (context.performed && wallSliding/*(controller.collisionDetector.leftCollision || controller.collisionDetector.rightCollision)*/)
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
