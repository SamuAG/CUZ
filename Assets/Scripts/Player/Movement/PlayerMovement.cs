using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonControllerRigidbody : MonoBehaviour
{
    public float speed = 6.0f;
    public float runSpeed = 12.0f;
    public float crouchSpeed = 3.0f;
    public float mouseSensitivity = 100.0f;
    public float jumpForce = 5.0f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask ceilingMask;
    
    private Rigidbody rb;
    private Transform playerCamera;
    private bool isGrounded;
    private bool isCrouching = false;
    private bool isSprinting = false;
    private float sensitivity = 0.05f;

    private float xRotation = 0f;
    

    [SerializeField] private InputManagerSO input;
    private Vector3 movimiento;
    private Vector2 cameraInput;
    private bool hasStoppedCrouching;

    private void OnEnable()
    {
        input.OnMovementStarted += MovementStarted;
        input.OnMovementCanceled += MovementCanceled;
        input.OnJumpStarted += JumpStarted;
        input.OnCrouchStarted += Crouch;
        input.OnCrouchCanceled += TryStandUp;
        input.OnCameraPerformed += CameraPerformed;
        input.OnCameraCanceled += CameraCanceled;
        input.OnSprintStarted += SprintStarted;
        input.OnSprintCanceled += SprintCanceled;
    }

    private void OnDisable()
    {
        input.OnMovementStarted -= MovementStarted;
        input.OnMovementCanceled -= MovementCanceled;
        input.OnJumpStarted -= JumpStarted;
        input.OnCrouchStarted -= Crouch;
        input.OnCrouchCanceled -= TryStandUp;
    }

    private void MovementStarted(Vector2 direction)
    {
        movimiento = new Vector3(direction.x, 0, direction.y);
    }

    private void MovementCanceled()
    {
        movimiento = Vector3.zero;
    }

    private void JumpStarted()
    {
        if (isGrounded && !isCrouching)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CameraPerformed(Vector2 delta)
    {
        cameraInput = delta * sensitivity;
        
    }

    private void CameraCanceled()
    {
        cameraInput = Vector2.zero;
    }

    private void SprintStarted()
    {
        isSprinting = true;
    }

    private void SprintCanceled()
    {
        isSprinting = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerCamera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        
        float mouseX = cameraInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = cameraInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        
        Vector3 move = transform.right * movimiento.x + transform.forward * movimiento.z;
        float currentSpeed = isCrouching ? crouchSpeed : (isSprinting ? runSpeed : speed);

        Vector3 velocity = move * currentSpeed;

        
        float verticalVelocity = rb.velocity.y;

        
        if (!isGrounded)
        {
            verticalVelocity = Mathf.Clamp(rb.velocity.y, -10f, 1f); //Limitamos la velocidad de salto
        }


        velocity.y = verticalVelocity;
        rb.velocity = velocity;

        if (hasStoppedCrouching)
        {
            TryStandUp();
        }
    }

    void Crouch()
    {
        hasStoppedCrouching = false;
        isCrouching = true;
        transform.localScale = new Vector3(1, 0.5f, 1);
        playerCamera.localPosition = new Vector3(0, 0.5f, 0);
    }

    void TryStandUp()
    {
        hasStoppedCrouching = true;
        RaycastHit hit;
        if (!Physics.SphereCast(transform.position, 0.5f, Vector3.up, out hit, 1f, ceilingMask))
        {
            StandUp();
        }
    }

    void StandUp()
    {
        hasStoppedCrouching = false;
        isCrouching = false;
        transform.localScale = new Vector3(1, 1, 1);
        playerCamera.localPosition = new Vector3(0, 1f, 0);
    }
}
