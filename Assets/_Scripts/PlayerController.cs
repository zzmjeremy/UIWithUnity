using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] private float frictionCoeff;
    [SerializeField] private LayerMask groundLayer;
    private int jumpCount;
    private readonly int doubleJump = 2;
    private Rigidbody rb;
    private CinemachineCamera freeLookCamera;
    private bool isDashing;

    private void Awake()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJump.AddListener(Jump);
        inputManager.OnDash.AddListener(Dash);
        rb = GetComponent<Rigidbody>();
        freeLookCamera = FindAnyObjectByType<CinemachineCamera>();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            if (IsTouchingGround())
            {
                isDashing = false;
            }
        }
        else
        {
            Vector3 velocity = rb.linearVelocity;
            float clampedX = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
            float clampedZ = Mathf.Clamp(velocity.z, -maxSpeed, maxSpeed);
            rb.linearVelocity = new Vector3(clampedX, velocity.y, clampedZ);
        }
        
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            Vector3 friction = -rb.linearVelocity.normalized * (frictionCoeff * Time.deltaTime);
            friction = new Vector3(friction.x, 0f, friction.z);
            rb.linearVelocity += friction;

            if (Vector3.Dot(rb.linearVelocity, rb.linearVelocity + friction) < 0) rb.linearVelocity = Vector3.zero;
        }
        
    }

    private void Update()
    {
        Quaternion rot = Quaternion.Euler(0, freeLookCamera.transform.rotation.eulerAngles.y, 0);
        //transform.forward = freeLookCamera.transform.forward;
        transform.rotation = rot;
    }

    private bool IsTouchingGround() => Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f, groundLayer);

    private void MovePlayer(Vector2 dirn)
    {
        Vector3 localDirection = new Vector3(dirn.x, 0f, dirn.y);
        Vector3 worldDirection = transform.TransformDirection(localDirection);
        rb.AddForce(worldDirection * acceleration);
    }

    private void Jump()
    {
        if (IsTouchingGround())
        {
            jumpCount = 0;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
        else if(jumpCount < doubleJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }
        
    }

    private void Dash()
    {
        isDashing = true;
        rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);
    }
}
