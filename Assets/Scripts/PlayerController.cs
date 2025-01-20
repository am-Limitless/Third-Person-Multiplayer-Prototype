using Mirror;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 6.0f;
    //[SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float animationPlayTransition = 0.15f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction jumpAction;

    private Animator animator; // Reference to Animator
    private int jumpAnimation;
    private float currentSpeed;

    public override void OnStartLocalPlayer()
    {
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
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        animator = GetComponentInChildren<Animator>(); // Assign the Animator
        jumpAnimation = Animator.StringToHash("Jumping");
        currentSpeed = walkSpeed; // Default to walking speed
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        // Check if player is grounded
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Get movement input
        Vector2 input = moveAction.ReadValue<Vector2>();
        float inputMagnitude = input.magnitude; // Magnitude of joystick input

        // Adjust speed based on input magnitude
        if (inputMagnitude > 0.5f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, runSpeed, Time.deltaTime * 10); // Smoothly increase speed
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, walkSpeed, Time.deltaTime * 10); // Smoothly decrease speed
        }

        // Update animation parameters
        animator.SetFloat("Horizontal", input.x); // Update Animator's Horizontal parameter
        animator.SetFloat("Vertical", input.y); // Update Animator's Vertical parameter

        // Move player
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * currentSpeed);

        // Handle jumping
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotate player to face the camera direction
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}