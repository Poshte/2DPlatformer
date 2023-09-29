using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player playerScript;

    void Start()
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
}
