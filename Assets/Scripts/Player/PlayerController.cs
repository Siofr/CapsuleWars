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
    private Vector3 velocity;

    private Vector2 look;
    private float xRotation;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject playerBody;

    [SerializeField] private float attackCooldown = 1.0f;
    private float currentAttackCooldown;

    private Transform[] respawnPoints;

    public BulletTrail bulletTrail;


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
        }
    }

    void Update()
    {
        if (!IsOwner)
        {
            Debug.Log("I am not the owner :(");
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

    public void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PerformJump();
        }
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
