using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
	private Player playerScript;

	private void Awake()
	{
		playerScript = GetComponent<Player>();
	}

	public void Movement(InputAction.CallbackContext context)
	{
		playerScript.GetMovementInput(context.ReadValue<Vector2>());
	}

	public void Jump(InputAction.CallbackContext context)
	{
		playerScript.GetJumpInput(context.performed, context.canceled);
	}

	public void Escape(InputAction.CallbackContext context)
	{
		GameEvents.Instance.PauseButtonClicked();
	}
}
