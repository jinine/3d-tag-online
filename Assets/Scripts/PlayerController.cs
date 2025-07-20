using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private InputSystem_Actions inputActions;
    private Vector2 movementInput;
    private float verticalVelocity;
    private bool isSprinting = false;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => movementInput = Vector2.zero;
        inputActions.Player.Jump.performed += _ => Jump();
        inputActions.Player.Sprint.performed += _ => isSprinting = true;
        inputActions.Player.Sprint.canceled += _ => isSprinting = false;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * currentSpeed * Time.deltaTime);

        verticalVelocity += gravity * Time.deltaTime;
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        move.y = verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    private void Jump()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
