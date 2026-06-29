using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerSensitivity;
    [SerializeField] private float jumpVelocity;

    private Vector2 move;
    private Vector3 playerMovement;

    private Vector2 look;
    private float xRotation;

    private InputAction MOVE;
    private InputAction ATTACK;
    private InputAction LOOK;
    private InputAction JUMP;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject playerBody;

    [SerializeField] private float attackCooldown = 1.0f;
    private float currentAttackCooldown;

    private Transform[] respawnPoints;

    public BulletTrail bulletTrail;

    private InputSystem_Actions inputActions;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Debug.Log(GetComponent<NetworkObject>().OwnerClientId);

        respawnPoints = PopulateRespawns();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Respawn();

        if (!IsOwner)
        {
            playerCamera.enabled = false;
            return;
        }

        AssignInputs();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (!IsOwner) return;

        MOVE.Disable();
        LOOK.Disable();
        ATTACK.Disable();
        JUMP.Disable();
    }

    void Update()
    {
        if (!IsOwner)
        {
            Debug.Log("I am not the owner of: " + this.gameObject.name);
            return;
        }

        PerformMove();

        if (currentAttackCooldown > 0) currentAttackCooldown -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (!IsOwner)
        {
            return;
        }

        PerformLook();
    }

    private void AssignInputs()
    {
        inputActions = new InputSystem_Actions();

        MOVE = inputActions.Player.Move;
        MOVE.Enable();
        MOVE.performed += OnMove;
        MOVE.canceled += OnMoveCanceled;

        LOOK = inputActions.Player.Look;
        LOOK.Enable();
        LOOK.performed += OnLook;
        LOOK.canceled += OnLookCanceled;

        ATTACK = inputActions.Player.Attack;
        ATTACK.Enable();
        ATTACK.performed += OnAttack;

        JUMP = inputActions.Player.Jump;
        JUMP.Enable();
        JUMP.performed += OnJump;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
    }

    public void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        move = new Vector2(0, 0);
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }

    public void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        look = new Vector2(0, 0);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        Respawn();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;

        if (currentAttackCooldown > 0) return;

        currentAttackCooldown = attackCooldown;

        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, 1000))
        {
            IDamageable d;

            if (hit.transform.TryGetComponent<IDamageable>(out d))
            {
                d.Damage(1);
            }

            bulletTrail.DrawBulletTrail(transform.position, hit.point, 0.1f);
        }
        else bulletTrail.DrawBulletTrail(transform.position, playerCamera.transform.forward * 1000, 0.1f);
    }

    public void PerformJump()
    {
        if (characterController.isGrounded) playerMovement.y = jumpVelocity;
    }

    public void PerformMove()
    {
        playerMovement = new Vector3(move.x, 0f, move.y);

        if (!characterController.isGrounded) playerMovement.y -= 9.8f * Time.deltaTime;
        else playerMovement.y = -1f;

        Vector3 MoveVector = transform.TransformDirection(playerMovement);

        characterController.Move(MoveVector * playerSpeed * Time.deltaTime);
    }

    public void PerformLook()
    {
        xRotation -= look.y * playerSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(0f, look.x * playerSensitivity, 0f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    public void Respawn()
    {
        int respawnSize = respawnPoints.Length - 1;

        TeleportPlayer(respawnPoints[Random.Range(0, respawnSize)].position);
    }

    private Transform[] PopulateRespawns()
    {
        return GameObject.FindGameObjectWithTag("Respawn").transform.GetComponentsInChildren<Transform>();
    }

    private void TeleportPlayer(Vector3 receiverPosition)
    {
        characterController.enabled = false;
        transform.position = receiverPosition;
        characterController.enabled = true;
    }
}
