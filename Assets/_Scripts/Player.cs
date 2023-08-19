using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
    //dependencies
    private Controller2D controller;

    private Vector3 velocity;

	//movement
	private Vector2 horizontalInput;
    [SerializeField]
	private float moveSpeed = 5f;
	private float moveSmoothing;

    //smooth time acceleration
    private float accelerationTimeAirborne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;

	//jumping
	[SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float timeToJumpApex;
    private float jumpVelocity;
    private float gravity;

	// Start is called before the first frame update
	void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        Debug.Log("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.collisionDetector.topCollision || controller.collisionDetector.bottomCollision)
            velocity.y = 0f;

        var target = horizontalInput.x * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x,
            target,
            ref moveSmoothing,
            (controller.collisionDetector.bottomCollision) ? accelerationTimeGrounded : accelerationTimeAirborne);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

	public void Movement(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>();
    }

	public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.collisionDetector.bottomCollision)
        {
            velocity.y = jumpVelocity;
            controller.Move(velocity * Time.deltaTime);
        }
    }

}
