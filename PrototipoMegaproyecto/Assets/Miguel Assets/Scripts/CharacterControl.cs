using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class CharacterControl : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput playerInput;
    private CharacterAnimation characterAnim;
    private Vector3 playerVelocity;
    private Transform cameraTransform;
    private bool groundedPlayer;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 2f;
    float speedLimit = 0.5f;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction aimAction;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        characterAnim = GetComponent<CharacterAnimation>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        aimAction = playerInput.actions["Aim"];
        aimAction.performed += _ => characterAnim.enableAimLayer();
        aimAction.canceled += _ => characterAnim.disableAimLayer();
        cameraTransform = Camera.main.transform;
        
    }

    void Update()
    {
        

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();

        bool forwardPressed = input.y > 0.0f ? true : false;
        bool runPressed = false;
        bool rightPressed = input.x > 0.0f ? true : false;
        bool leftPressed = input.x < 0.0f ? true : false;
        bool backPressed = input.y < 0.0f ? true : false;
        speedLimit = runPressed ? 2.0f : 0.5f;
        characterAnim.changeVelocity(forwardPressed, backPressed, leftPressed, rightPressed, runPressed, speedLimit);
        characterAnim.lockOrResetVelocity(forwardPressed, backPressed, leftPressed, rightPressed, runPressed, speedLimit);

        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
