using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float sensX = 400f;
    public float sensY = 400f;
    public float multiplier = 0.01f;
    public float maxYAngle = 90f;
    
    [Header("Camera Position")]
    public Transform cameraPosition;
    public float cameraSmoothness = 10f;
    
    [Header("Head Bob")]
    public float bobFrequency = 5f;
    public float bobHorizontalAmplitude = 0.1f;
    public float bobVerticalAmplitude = 0.1f;
    public float headBobSmoothing = 10f;
    [Range(0, 1)] public float headBobMoveSpeed = 0.5f;
    private float defaultYPos = 0;
    private float headBobTimer;
    
    [Header("Camera Lean")]
    public float maxLeanAngle = 10f;
    public float leanSpeed = 8f;
    private float currentLean;
    
    [Header("Wall Run")]
    public float wallRunTilt = 15f;
    public float wallRunTiltSmoothing = 10f;
    private float currentWallRunTilt;
    
    [Header("References")]
    public Transform orientation;
    private WallMechanics wallMechanics;
    private AdvancedMovements moveScript;
    
    // Camera rotation
    private float xRotation;
    private float yRotation;
    
    // Mouse input
    private float mouseX;
    private float mouseY;
    
    // Movement state
    private bool isMoving;
    private Vector3 targetBobPosition;
    private Vector3 initialCameraPos;

    private void Start()
    {
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Get component references
        wallMechanics = GetComponentInParent<WallMechanics>();
        moveScript = GetComponentInParent<AdvancedMovements>();
        
        // Store initial positions
        defaultYPos = cameraPosition.localPosition.y;
        initialCameraPos = cameraPosition.localPosition;
        
        // Ensure we start with zero rotation
        xRotation = 0f;
        yRotation = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        GetInput();
        RotateCamera();
        HandleHeadBob();
        HandleCameraLean();
        HandleWallRunTilt();
        UpdateCameraPosition();
    }

    private void GetInput()
    {
        // Get mouse input
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        
        // Scale input by sensitivity and multiplier
        mouseX *= sensX * multiplier;
        mouseY *= sensY * multiplier;
        
        // Check if player is moving for head bob
        isMoving = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).magnitude > 0.1f;
    }

    private void RotateCamera()
    {
        // Calculate new rotation
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxYAngle, maxYAngle);
        
        // Apply rotation to camera and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, currentWallRunTilt + currentLean);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void HandleHeadBob()
    {
        if (isMoving && moveScript.IsGrounded)
        {
            // Calculate head bob
            headBobTimer += Time.deltaTime * bobFrequency * (moveScript.IsRunning ? 1.2f : 1f);
            targetBobPosition = initialCameraPos + new Vector3(
                Mathf.Cos(headBobTimer) * bobHorizontalAmplitude,
                Mathf.Sin(headBobTimer * 2) * bobVerticalAmplitude,
                0);
        }
        else
        {
            // Reset head bob
            headBobTimer = 0;
            targetBobPosition = initialCameraPos;
        }
        
        // Smooth the head bob movement
        cameraPosition.localPosition = Vector3.Lerp(
            cameraPosition.localPosition, 
            targetBobPosition, 
            Time.deltaTime * headBobSmoothing
        );
    }

    private void HandleCameraLean()
    {
        // Calculate lean based on horizontal input
        float targetLean = 0;
        if (moveScript.IsGrounded && !wallMechanics.IsWallRunning)
        {
            targetLean = -Input.GetAxisRaw("Horizontal") * maxLeanAngle;
        }
        
        // Smooth the lean
        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);
    }

    private void HandleWallRunTilt()
    {
        float targetWallRunTilt = 0;
        
        // Apply wall run tilt
        if (wallMechanics.IsWallRunning)
        {
            targetWallRunTilt = wallMechanics.IsWallLeft ? -wallRunTilt : wallRunTilt;
        }
        
        // Smooth the wall run tilt
        currentWallRunTilt = Mathf.Lerp(
            currentWallRunTilt, 
            targetWallRunTilt, 
            Time.deltaTime * wallRunTiltSmoothing
        );
    }

    private void UpdateCameraPosition()
    {
        // Smoothly move camera to target position
        transform.position = Vector3.Lerp(
            transform.position,
            cameraPosition.position,
            Time.deltaTime * cameraSmoothness
        );
    }

    // Call this when you need to reset camera rotation (e.g., after death or respawn)
    public void ResetCamera()
    {
        xRotation = 0f;
        yRotation = orientation.rotation.eulerAngles.y;
        currentLean = 0f;
        currentWallRunTilt = 0f;
        cameraPosition.localPosition = initialCameraPos;
    }
}