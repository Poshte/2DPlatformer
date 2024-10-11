using Assets._Scripts.BaseInfos;
using FMOD.Studio;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : NPC, ITalkable, IWalkable
{
	#region Fields
	//dependencies
	private Controller2D controller2D;

	//movement
	[SerializeField] private float moveSpeed = 5f;
	private Vector2 playerInput;
	private float velocityXSmoothing;

	//smooth time acceleration
	private const float accelerationTimeAirborne = 0.2f;
	private const float accelerationTimeGrounded = 0.1f;

	//jumping
	[SerializeField] private float maxJumpHeight;
	[SerializeField] private float minJumpHeight;
	[SerializeField] private float timeToJumpApex;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private const float slidingJumpY = 8f;
	private const float slidingJumpX = -2f;
	private int inputDirection;

	//wall jumping
	[SerializeField] private float maxWallSlidingSpeed;
	[SerializeField] private Vector2 wallJumpFall;
	[SerializeField] private Vector2 wallJumpClimb;
	[SerializeField] private Vector2 wallJumpLeap;

	//falling down through a platform
	private bool isCommandButtonDown;
	private const float commandButtonHoldDuration = 0.1f;
	private float commandButtonTimer;

	//jump buffer
	private const float jumpBuffer = 2f;
	private float bufferCounter;

	//coyote time
	private const float coyoteTime = 0.1f;
	private float coyoteCounter;

	private float gravity;
	private Vector2 velocity;

	//behavior
	[SerializeField] private DialogueText dialogueText;
	[SerializeField] private DialogueController dialogueController;

	//sounds
	private EventInstance playerFootsteps;
	#endregion

	#region MonoBehaviors 
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

		//get sound instances
		playerFootsteps = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.PlayerFootstepsSound);
	}

	void Update()
	{
		ManageInputDirection();

		CalculateVelocity();
		
		FallThroughPlatform();
		
		ManageCoyoteTime();
		
		MovePlayer();
		
		UpdatePlayerSounds();
		
		ManageTopAndBottomCollisions();
	}
	#endregion

	#region Methods
	private void ManageTopAndBottomCollisions()
	{
		if (controller2D.info.topCollision || controller2D.info.bottomCollision)
			if (controller2D.info.slidingMaxSlope)
				velocity.y += controller2D.info.slopeNormal.y * Mathf.Abs(gravity) * Time.deltaTime;
			else
				velocity.y = 0f;
	}

	private void MovePlayer()
	{
		controller2D.Move(velocity * Time.deltaTime, isCommandButtonDown);
	}

	private void ManageCoyoteTime()
	{
		if (controller2D.info.bottomCollision)
		{
			coyoteCounter = coyoteTime;
		}
		else
			coyoteCounter -= Time.deltaTime;
	}

	private void ManageInputDirection()
	{
		if (playerInput.x > 0)
			inputDirection = 1;
		else if (playerInput.x < 0)
			inputDirection = -1;
		else
			inputDirection = 0;
	}

	private void CalculateVelocity()
	{
		var target = playerInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, target, ref velocityXSmoothing, controller2D.info.bottomCollision ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}

	private void FallThroughPlatform()
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

	public void GetMovementInput(Vector2 movementInput)
	{
		if (!base.isActiveAndEnabled)
			return;

		playerInput = movementInput;
	}

	public void GetJumpInput(bool performed, bool canceled)
	{
		if (!base.isActiveAndEnabled)
			return;

		if (performed)
			bufferCounter = jumpBuffer;

		//regular jumping max
		if (bufferCounter > 0f && coyoteCounter > 0f)
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
		}
		//regular jumping min
		else if (canceled && velocity.y > minJumpVelocity /*&& coyoteCounter > 0f*/)
		{
			velocity.y = minJumpVelocity;

			controller2D.info.bottomCollision = false;
			bufferCounter = 0f;
		}

		controller2D.Move(velocity * Time.deltaTime);
	}

	public Vector2 GetPlayerVelocity()
	{
		return velocity;
	}

	private void OnEnable()
	{
		velocity.x = 0f;
		velocity.y = 0f;

		playerFootsteps.stop(STOP_MODE.IMMEDIATE);
	}

	private void OnDisable()
	{
		velocity.x = 0f;
		velocity.y = 0f;
		playerFootsteps.stop(STOP_MODE.IMMEDIATE);
	}

	private void OnDestroy()
	{
		velocity.x = 0f;
		velocity.y = 0f;
		playerFootsteps.stop(STOP_MODE.IMMEDIATE);
	}

	private void UpdatePlayerSounds()
	{
		if (this.gameObject.CompareTag(Constants.Tags.Clone))
		{
			playerFootsteps.stop(STOP_MODE.IMMEDIATE);
			return;
		}

		if (
			!controller2D.info.leftCollision &&
			!controller2D.info.rightCollision &&
			playerInput.x != 0 &&
			controller2D.info.bottomCollision)
		{
			playerFootsteps.getPlaybackState(out PLAYBACK_STATE playbackState);

			if (playbackState == PLAYBACK_STATE.STOPPED)
			{
				playerFootsteps.start();
			}
		}
		else
		{
			playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
		}
	}
	#endregion

	#region Behaviors
	public override void Interact()
	{
		Talk(dialogueText);
	}

	public void Walk()
	{
		throw new System.NotImplementedException();
	}

	public void Talk(DialogueText dialogueText)
	{
		dialogueController.DisplayNextParagraph(dialogueText);
	}
	#endregion
}
