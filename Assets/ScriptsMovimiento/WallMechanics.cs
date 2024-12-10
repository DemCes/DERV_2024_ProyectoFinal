using UnityEngine;
using System.Collections;

public class WallMechanics : MonoBehaviour
{
    [Header("Wall Running")]
    public float wallRunForce = 200f;  // Reduced from 1000f
    public float wallRunGravity = 1f;
    public float wallRunJumpForce = 6f;
    public float maxWallRunTime = 0.7f;
    private float wallRunTimer;
    public float wallStickForce = 20f;  // Reduced from 100f
    
    [Header("Wall Detection")]
    public float wallCheckDistance = 0.7f;
    public LayerMask wallLayer;
    public float minJumpHeight = 1.5f;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    [Header("Wall Jump")]
    public float wallJumpUpForce = 7f;  // Reduced from 12f
    public float wallJumpSideForce = 8f;  // Reduced from 10f
    public float exitWallTime = 0.2f;
    private float exitWallTimer;

    [Header("Camera Effects")]
    public float wallRunTilt = 15f;
    public float tiltSpeed = 15f;
    private float tilt;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;

    [Header("References")]
    public Transform orientation;
    private AdvancedMovements moveScript;
    private Rigidbody rb;
    public Camera cam;

    // States
    private bool isWallRunning;
    private bool isWallLeft;
    private bool isWallRight;
    private bool exitingWall;
    private Vector3 lastWallNormal;

    // Public properties to access states
    public bool IsWallRunning => isWallRunning;
    public bool IsWallLeft => isWallLeft;
    public bool IsWallRight => isWallRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveScript = GetComponent<AdvancedMovements>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckForWall();
        
        // Only allow wall running if not grounded
        if (!moveScript.IsGrounded)
        {
            WallRunInput();
        }
        else
        {
            StopWallRun();
        }
        
        ExitWallRun();

        // Wall jump input - only allow when actually wall running
        if (Input.GetKeyDown(jumpKey) && isWallRunning)
        {
            WallJump();
        }

        // Camera tilt
        if (isWallRunning)
        {
            tilt = Mathf.Lerp(tilt, isWallLeft ? -wallRunTilt : wallRunTilt, Time.deltaTime * tiltSpeed);
        }
        else
        {
            tilt = Mathf.Lerp(tilt, 0, Time.deltaTime * tiltSpeed);
        }

        if (cam != null)
        {
            cam.transform.localRotation = Quaternion.Euler(0, 0, tilt);
        }
    }

    private void FixedUpdate()
    {
        if (isWallRunning)
        {
            WallRunMovement();
        }
    }

    private void CheckForWall()
    {
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, wallLayer);
        isWallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, wallLayer);
    }

    private void WallRunInput()
    {
        // Get input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Start wall run only if moving forward
        if ((isWallLeft || isWallRight) && verticalInput > 0 && !exitingWall)
        {
            Debug.Log("no esta Corriendo");
            if (!isWallRunning)
                StartWallRun();
        }
        else if (isWallRunning)
        {
            Debug.Log("esta corriendo");
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        isWallRunning = true;
        wallRunTimer = maxWallRunTime;

        // Store the wall normal for proper jumping direction
        lastWallNormal = isWallLeft ? leftWallHit.normal : rightWallHit.normal;

        // Reset y velocity to prevent sticking
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }

    private void StopWallRun()
    {
        if (isWallRunning)
        {
            isWallRunning = false;
            exitingWall = true;
            exitWallTimer = exitWallTime;
            rb.useGravity = true;
        }
    }

    private void WallRunMovement()
    {
        // Wall run gravity
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        // Get the wall run direction
        Vector3 wallNormal = isWallLeft ? leftWallHit.normal : rightWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up);

        if (Vector3.Dot(wallForward, orientation.forward) < 0)
            wallForward = -wallForward;

        // Forward force
        rb.AddForce(wallForward * wallRunForce * Time.deltaTime, ForceMode.Force);

        // Stick to wall - use a gentler force
        rb.AddForce(-wallNormal * wallStickForce * Time.deltaTime, ForceMode.Force);

        // Reduce timer
        wallRunTimer -= Time.deltaTime;
        if (wallRunTimer <= 0)
        {
            StopWallRun();
        }
    }

    private void WallJump()
    {
        StopWallRun();
        
        // Calculate jump direction using the stored wall normal
        Vector3 jumpDirection = lastWallNormal * wallJumpSideForce + Vector3.up * wallJumpUpForce;
        
        // Reset velocity and add jump force
        rb.velocity = Vector3.zero;
        rb.AddForce(jumpDirection, ForceMode.Impulse);
        
        // Add some forward momentum if moving forward
        if (verticalInput > 0)
        {
            rb.AddForce(orientation.forward * wallJumpSideForce * 0.5f, ForceMode.Impulse);
        }
    }

    private void ExitWallRun()
    {
        if (exitingWall)
        {
            if (exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }
            else
            {
                exitingWall = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize wall detection rays
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, -orientation.right * wallCheckDistance);
        Gizmos.DrawRay(transform.position, orientation.right * wallCheckDistance);
    }
}