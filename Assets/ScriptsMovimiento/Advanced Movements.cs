using System.Collections;
using UnityEngine;

public class AdvancedMovements : MonoBehaviour
{
   // Movimiento
    [Header("Movimiento")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 5f;
    public float groundDrag = 6f;

    // Control de salto
    private bool readyToJump = true;
    public float jumpCooldown = 0.25f;

    // Detección de suelo
    [Header("Detección de Suelo")]
    public float playerHeight = 2f;
    public LayerMask groundLayer;
    private bool isGrounded;
    
    // Public property to access isGrounded
    public bool IsGrounded => isGrounded;
    
    // Public property to check if running
    public bool IsRunning => Input.GetKey(KeyCode.LeftShift);
    // Escala inicial
    private float initialScaleY;
    private float initialScaleMagnitude;

    // Entrada
    private float horizontalInput;
    private float verticalInput;
    
    // Dirección de movimiento
    private Vector3 moveDirection;

    // Referencias
    public Transform orientation;
    private Rigidbody rb;

    void Start()
    {
        // Inicialización de Rigidbody y configuración inicial
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Guardar escala inicial
        initialScaleY = transform.localScale.y;
        initialScaleMagnitude = transform.localScale.magnitude;

        // Calcular la altura inicial del jugador en función de su escala
        playerHeight = GetComponent<Collider>().bounds.size.y;
    }

    void Update()
    {
        // Comprobar si el personaje está en el suelo
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        // Arrastrar del suelo
        rb.drag = isGrounded ? groundDrag : 0;

        // Obtener la entrada del usuario
        GetInput();

        // Control de la velocidad en función de la escala
        SpeedControl();

        // Saltar si se cumple la condición
        if (isGrounded && readyToJump && Input.GetKey(KeyCode.Space))
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        // Entrada de movimiento
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        if (GetComponent<SlideMechanics>().IsSliding)
        return;
        // Dirección de movimiento basada en orientación y entrada
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Determinar la velocidad de movimiento en función de la escala y si está corriendo o caminando
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        float adjustedSpeed = currentSpeed * (transform.localScale.magnitude / initialScaleMagnitude);

        // Aplicar fuerza de movimiento
        if (isGrounded)
            rb.AddForce(moveDirection.normalized * adjustedSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * adjustedSpeed * 5f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        // Limitar la velocidad en el plano horizontal en función de la escala
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        float maxSpeed = (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed) * (transform.localScale.magnitude / initialScaleMagnitude);

        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        // Ajustar la fuerza de salto en función de la escala
        float adjustedJumpForce = jumpForce * (transform.localScale.y / initialScaleY);
        
        // Aplicar fuerza de salto
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * adjustedJumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        // Restablecer la capacidad de saltar
        readyToJump = true;
    }
}

