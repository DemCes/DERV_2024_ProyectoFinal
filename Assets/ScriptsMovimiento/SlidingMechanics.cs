using UnityEngine;
using System.Collections;

public class SlideMechanics : MonoBehaviour
{
    // Configuración de los parámetros del deslizamiento.
    [Header("Slide Settings")]
    public float slideSpeed = 12f; // Velocidad base de deslizamiento.
    public float slideSpeedMultiplier = 1.5f; // Multiplicador de velocidad en pendientes.
    public float slideForce = 400f; // Fuerza inicial del deslizamiento.
    public float slideDrag = 0.2f; // Drag que se aplica durante el deslizamiento.
    public float minSlideSpeed = 2f; // Velocidad mínima para iniciar o continuar el deslizamiento.
    public float maxSlideTime = 1f; // Tiempo máximo que puede durar un deslizamiento.
    public float slopeSlidePower = 1.5f; // Potencia de incremento de velocidad al deslizarse en pendientes.

    // Configuración de controles para activar el deslizamiento.
    [Header("Slide Controls")]
    public KeyCode slideKey = KeyCode.LeftControl; // Tecla para activar el deslizamiento.
    public float slideYScale = 0.5f; // Escala Y a la que cambia el personaje al deslizarse.
    public float slideTransitionSpeed = 10f; // Velocidad de transición de la escala y cámara durante el deslizamiento.

    // Parámetros para detectar la pendiente.
    [Header("Slope Detection")]
    public float maxSlideAngle = 60f; // Ángulo máximo de pendiente para permitir el deslizamiento.
    public float minSlideAngle = 5f; // Ángulo mínimo para aplicar el multiplicador de velocidad.
    public float groundCheckDistance = 0.3f; // Distancia de chequeo para detectar el suelo.

    // Efectos de la cámara durante el deslizamiento.
    [Header("Camera Effects")]
    public float slideFOVIncrease = 10f; // Aumento del campo de visión (FOV) de la cámara al deslizarse.
    public float FOVTransitionSpeed = 10f; // Velocidad de transición del FOV durante el deslizamiento.
    public float cameraSlideYOffset = -0.5f; // Desplazamiento vertical de la cámara cuando el personaje está deslizándose.

    // Referencias necesarias.
    [Header("References")]
    public Transform orientation; // Orientación del personaje (dirección hacia la que se desliza).
    public Camera playerCamera; // Cámara del jugador para aplicar efectos de FOV y posición.
    public Transform cameraHolder; // Objeto padre de la cámara para ajustar su posición al deslizarse.

    // Variables internas
    private Rigidbody rb; // Referencia al Rigidbody del personaje.
    private AdvancedMovements moveScript; // Script de movimiento avanzado del personaje.
    private CapsuleCollider capsuleCollider; // Collider del personaje, para ajustar su altura en el deslizamiento.

    // Valores originales guardados para revertir cambios tras el deslizamiento.
    private float originalHeight; // Altura original del collider.
    private float originalYScale; // Escala Y original del personaje.
    private float originalFOV; // Campo de visión original de la cámara.
    private Vector3 originalCameraPosition; // Posición original de la cámara.
    private Vector3 originalLocalScale; // Escala original del personaje.

    // Estado del deslizamiento.
    private bool isSliding; // Indica si el personaje está deslizándose.
    private float slideTimer; // Temporizador para medir la duración del deslizamiento.
    private Vector3 slideDirection; // Dirección del deslizamiento.
    private bool wasSliding; // Marca si el personaje estaba deslizándose en el último frame.
    private bool isTransitioning; // Indica si se está realizando una transición de deslizamiento.

    // Propiedad pública para acceder al estado de deslizamiento.
    public bool IsSliding => isSliding;
    [Header("Slide Collider Settings")]
    public float slideColliderCenterYOffset = 0.5f;

    private void Start()
    {
        // Obtiene referencias a componentes necesarios.
        rb = GetComponent<Rigidbody>();
        moveScript = GetComponent<AdvancedMovements>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.center = new Vector3(0, originalHeight*slideColliderCenterYOffset,0);

        // Configura referencias a la cámara y su soporte si no están asignadas.
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("No camera assigned to SlideMechanics and couldn't find main camera!");
            }
        }

        if (cameraHolder == null && playerCamera != null)
        {
            cameraHolder = playerCamera.transform.parent;
        }

        if (orientation == null && moveScript != null)
        {
            orientation = moveScript.orientation;
        }

        // Guarda valores originales.
        originalHeight = capsuleCollider.height;
        originalYScale = transform.localScale.y;
        originalLocalScale = transform.localScale;
        if (playerCamera != null && cameraHolder != null)
        {
            originalFOV = playerCamera.fieldOfView;
            originalCameraPosition = cameraHolder.localPosition;
        }
    }

    private void Update()
    {
        CheckSlideInput(); // Verifica entrada del usuario para iniciar o detener el deslizamiento.
        HandleSlideState(); // Actualiza el estado del deslizamiento y maneja los temporizadores.
        if (playerCamera != null)
        {
            HandleCameraEffects(); // Ajusta los efectos visuales de la cámara durante el deslizamiento.
        }
    }

    private void FixedUpdate()
    {
        if (isSliding)
        {
            HandleSlideMovement(); // Controla la física del deslizamiento en FixedUpdate.
        }
    }

    private void CheckSlideInput()
    {
        // Inicia o detiene el deslizamiento según la entrada del usuario.
        if (!isTransitioning)
        {
            if (Input.GetKeyDown(slideKey) && moveScript.IsGrounded && !isSliding && rb.velocity.magnitude > minSlideSpeed)
            {
                StartSlide(); // Inicia el deslizamiento.
            }
            
            if (Input.GetKeyUp(slideKey) && isSliding)
            {
                StopSlide(); // Detiene el deslizamiento.
            }
        }
    }

    private void StartSlide()
    {
        // Configura parámetros iniciales al iniciar el deslizamiento.
        if (isTransitioning) return;
        
        isSliding = true;
        slideTimer = maxSlideTime;
        wasSliding = false;

        slideDirection = orientation != null ? orientation.forward : transform.forward;
        rb.AddForce(slideDirection * slideForce, ForceMode.Impulse);

        // Cambia la escala del personaje y la posición de la cámara.
        StartCoroutine(SmoothlyChangeScale(slideYScale));
        StartCoroutine(SmoothlyMoveCamera(true));

        rb.drag = slideDrag;
    }

    private void StopSlide()
    {
        // Detiene el deslizamiento y restaura los valores originales.
        if (!isSliding || isTransitioning) return;

        isSliding = false;
        wasSliding = true;

        StartCoroutine(SmoothlyChangeScale(originalYScale));
        StartCoroutine(SmoothlyMoveCamera(false));

        rb.drag = moveScript.groundDrag;
    }

    private IEnumerator SmoothlyChangeScale(float targetYScale)
    {
        // Transición suave de escala del personaje durante el deslizamiento.
        isTransitioning = true;

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(originalLocalScale.x, targetYScale, originalLocalScale.z);
        float initialHeight = capsuleCollider.height;
        float targetHeight = originalHeight * (targetYScale / originalYScale);
        float targetCenterY = targetHeight*slideColliderCenterYOffset;

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * slideTransitionSpeed;
            float t = Mathf.Clamp01(elapsedTime);

            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            capsuleCollider.height = Mathf.Lerp(initialHeight, targetHeight, t);
            capsuleCollider.center = new Vector3(0, targetCenterY / 2f, 0);

            yield return null;
        }

        transform.localScale = targetScale;
        capsuleCollider.height = targetHeight;
        capsuleCollider.center = new Vector3(0, targetCenterY / 2f, 0);

        isTransitioning = false;
    }

    private IEnumerator SmoothlyMoveCamera(bool toSlidePosition)
    {
        // Desplazamiento suave de la cámara al deslizarse.
        if (cameraHolder == null) yield break;

        Vector3 startPos = cameraHolder.localPosition;
        Vector3 targetPos = toSlidePosition ? 
            originalCameraPosition + new Vector3(0, cameraSlideYOffset, 0) : 
            originalCameraPosition;

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * slideTransitionSpeed;
            float t = Mathf.Clamp01(elapsedTime);

            cameraHolder.localPosition = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        cameraHolder.localPosition = targetPos;
    }

    private void HandleSlideState()
    {
        // Actualiza el temporizador del deslizamiento y controla la duración.
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;

            if (slideTimer <= 0 || rb.velocity.magnitude < minSlideSpeed)
            {
                StopSlide();
            }

            CheckSlideObstacles(); // Verifica obstáculos en el camino.
        }
    }

    private void HandleSlideMovement()
    {
        // Controla la física del deslizamiento según el terreno.
        if (orientation == null) return;

        RaycastHit hit;
        bool isOnSlope = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance);

        if (isOnSlope)
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            float currentSlideSpeed = slideSpeed;

            if (slopeAngle > minSlideAngle && slopeAngle < maxSlideAngle)
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(slideDirection, hit.normal).normalized;
                currentSlideSpeed *= slideSpeedMultiplier;

                float slopeFactor = Mathf.Pow(slopeAngle / maxSlideAngle, slopeSlidePower);
                rb.AddForce(slopeDirection * currentSlideSpeed * slopeFactor, ForceMode.Force);
            }

            rb.AddForce(slideDirection * currentSlideSpeed, ForceMode.Force);
        }

        if (rb.velocity.magnitude > slideSpeed * slideSpeedMultiplier)
        {
            rb.velocity = rb.velocity.normalized * slideSpeed * slideSpeedMultiplier;
        }
    }

    private void HandleCameraEffects()
    {
        // Controla los efectos de la cámara durante el deslizamiento.
        if (playerCamera == null) return;

        float targetFOV = isSliding ? originalFOV + slideFOVIncrease : originalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * FOVTransitionSpeed);
    }

    private void CheckSlideObstacles()
    {
        // Detiene el deslizamiento si se encuentra un obstáculo al frente.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, slideDirection, out hit, capsuleCollider.radius + 0.2f))
        {
            StopSlide();
        }
    }

    public void ForceStopSlide()
    {
        // Método para forzar la detención del deslizamiento.
        if (isSliding)
        {
            StopSlide();
        }
    }

    private void OnDisable()
    {
        // Restaura el estado original del personaje y la cámara cuando se desactiva el script.
        if (isSliding)
        {
            StopSlide();
        }

        if (cameraHolder != null)
        {
            cameraHolder.localPosition = originalCameraPosition;
        }
        transform.localScale = originalLocalScale;
        if (capsuleCollider != null)
        {
            capsuleCollider.height = originalHeight;
            capsuleCollider.center = new Vector3(0, originalHeight *slideColliderCenterYOffset, 0);
        }
    }
}
