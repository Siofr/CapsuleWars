using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputHandler : MonoBehaviour
{
    private InputSystem_Actions _inputActions;

    private InputAction MOVE;
    private InputAction LOOK;
    private InputAction JUMP;
    private InputAction ATTACK;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        MOVE = _inputActions.Player.Move;
        MOVE.Enable();
        MOVE.performed += OnMovePerformed;

        LOOK = _inputActions.Player.Look;
        LOOK.Enable();
        LOOK.performed += OnLookPerformed;

        JUMP = _inputActions.Player.Jump;
        JUMP.Enable();
        JUMP.performed += OnJumpPerformed;

        ATTACK = _inputActions.Player.Attack;
        ATTACK.Enable();
        ATTACK.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        MOVE.Disable();
        LOOK.Disable();
        JUMP.Disable();
        ATTACK.Disable();
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
        // Attack Code
    }
}
