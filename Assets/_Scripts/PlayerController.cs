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
    [SerializeField] private float airControl;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private int jumpCount;
    private readonly int doubleJump = 2;
    private Rigidbody rb;
    private CinemachineCamera freeLookCamera;
    private bool isDashing;

    private RaycastHit rightHit;
    private RaycastHit leftHit;
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
            Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);

            if (horizontalVelocity.magnitude > maxSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            }

            rb.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.y);
        }

    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, freeLookCamera.transform.rotation.eulerAngles.y, 0);
    }

    private bool IsTouchingGround() => Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f, groundLayer);
    // private bool IsTouchingRightWall() => Physics.Raycast(transform.position, transform.right, out rightHit, 1.1f, wallLayer);
    // private bool IsTouchingLeftWall() => Physics.Raycast(transform.position, -transform.right, out leftHit, 1.1f, wallLayer);
    private void MovePlayer(Vector2 dirn)
    {
        Vector3 direction = new Vector3(dirn.x, 0f, dirn.y) ;
        // Vector3 worldDirection = transform.TransformDirection(localDirection);
        Quaternion rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        Vector3 reorientedDirection = rotation * direction;
        if (IsTouchingGround())
        {
            rb.AddForce(reorientedDirection * acceleration);
        }
        else
        {
            rb.AddForce(reorientedDirection * acceleration * airControl);
        }

    }

    private void Jump()
    {
        Debug.Log("Should Jump");
        Vector3 jumpDir = Vector3.up;
        if (IsTouchingGround())
        {
            jumpCount = 0;
        }
        // if(IsTouchingRightWall())
        // {
        //     jumpCount = 0;
        //     jumpDir = -transform.right + rightHit.collider.transform.up + transform.forward*2;
        // }
        // if(IsTouchingLeftWall())
        // {
        //     jumpCount = 0;
        //     jumpDir = transform.right + leftHit.collider.transform.up + transform.forward*2;
        // }

        if(jumpCount < doubleJump)
        {
            rb.AddForce(jumpDir * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }

    }

    private void Dash()
    {
        isDashing = true;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        // if (IsTouchingLeftWall())
        // {
        //     Debug.DrawRay(transform.position, -transform.right * 1.1f, Color.green);
        // }
        // else
        // {
        //     Debug.DrawRay(transform.position, -transform.right * 1.1f, Color.red);
        // }

        // if (IsTouchingRightWall())
        // {
        //     Debug.DrawRay(transform.position, transform.right * 1.1f, Color.green);
        // }
        // else
        // {
        //     Debug.DrawRay(transform.position, transform.right * 1.1f, Color.red);
        // }

    }
}
