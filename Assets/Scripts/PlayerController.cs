using Mirror;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float animationPlayTransition = 0.15f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Animator animator;

    private Vector3 playerVelocity;
    private Transform cameraTransform;

    private bool groundedPlayer;
    private float currentSpeed;

    private InputAction moveAction;
    private InputAction jumpAction;

    private int jumpAnimation;
    
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        GameObject.FindGameObjectWithTag("PlayerFollowCamera").GetComponent<CinemachineCamera>().Follow = transform.GetChild(0).transform;
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        jumpAnimation = Animator.StringToHash("Jumping");
        currentSpeed = walkSpeed;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        HandleMovement();
        HandleJumping();
        ApplyGravity();
        RotatePlayer();
        SyncMovementWithServer();
    }

    private void HandleMovement()
    {
        groundedPlayer = controller.isGrounded; // Check if player is grounded

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        AdjustSpeed(input.magnitude);
        UpdateAnimationParameters(input);
        MovePlayer(input);
    }

    private void AdjustSpeed(float inputMagnitude)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, inputMagnitude > 0.5f ? runSpeed : walkSpeed, Time.deltaTime * 10);
    }
    private void UpdateAnimationParameters(Vector2 input)
    {
        // Update animation parameters
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
    }

    private void MovePlayer(Vector2 input)
    {
        Vector3 move = input.x * cameraTransform.right.normalized + input.y * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * currentSpeed);
    }

    private void HandleJumping()
    {
        // Handle jumping
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }
    }

    private void ApplyGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        // Rotate player to face the camera direction
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void SyncMovementWithServer()
    {
        // Send movement and rotation to the server
        CmdSendMovement(transform.position, transform.rotation);
    }

    [Command]
    private void CmdSendMovement(Vector3 position, Quaternion rotation)
    {
        // Update on the server
        transform.position = position;
        transform.rotation = rotation;

        // Broadcast to clients
        RpcUpdateMovement(position, rotation);
    }

    [ClientRpc]
    private void RpcUpdateMovement(Vector3 position, Quaternion rotation)
    {
        if (isLocalPlayer) return; // Ignore updates for the local player
        transform.position = position;
        transform.rotation = rotation;
    }
}