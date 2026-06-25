using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    private InputSystem_Actions _inputActions;

    public InputAction Move;
    public InputAction Look;
    public InputAction Jump;
    public InputAction Attack;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        // Move Code
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        // Look Code
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        // Jump Code
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        // Look Code
    }
}
