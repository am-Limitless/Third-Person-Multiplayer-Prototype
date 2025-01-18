using UnityEngine;

public class InputManager : MonoBehaviour
{
    // PlayerInput instance to manage input actions
    private PlayerInput playerInput;
    private AnimatorManager animatorManager;

    // Movement input values
    public Vector2 MovementInput;
    private float moveAmount;
    public float VerticalInput;
    public float HorizontalInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void OnEnable()
    {
        // Initialize PlayerInput if not already done
        if (playerInput == null)
        {
            playerInput = new PlayerInput();

            // Subscribe to the Move action's performed event
            playerInput.Player.Move.performed += context => MovementInput = context.ReadValue<Vector2>();
            playerInput.Player.Move.canceled += context => MovementInput = Vector2.zero; // Reset when input stops
        }

        // Enable the input system
        playerInput.Enable();
    }

    private void OnDisable()
    {
        // Disable the input system
        playerInput.Disable();
    }

    private void Update()
    {
        // Update vertical and horizontal input values every frame
        //HandleMovementInput();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        //HandleJumpingInput
        //HandleActionInput
    }

    private void HandleMovementInput()
    {
        VerticalInput = MovementInput.y;
        HorizontalInput = MovementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }
}
