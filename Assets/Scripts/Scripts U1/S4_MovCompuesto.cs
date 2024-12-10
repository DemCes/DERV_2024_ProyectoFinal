using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S4_MovCompuesto : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] float sensX = 400f;
    [SerializeField] float sensY = 400f;
    [SerializeField] float maxYAngle = 90f;

    [Header("Movement Settings")]
    [SerializeField] float velocidad_movimiento = 10f;
    [SerializeField] float checkDistance = 0.5f; // Distancia para verificar colisiones

    [Header("References")]
    [SerializeField] Transform playerCamera;

    // Variables de rotación
    private float xRotation;
    private float yRotation;
    private bool isInPlayArea = false;
    private bool isPlayerScene;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleCameraRotation();
        HandleMovement();
    }
    private void HandleCameraRotation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxYAngle, maxYAngle);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void HandleMovement()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movement += transform.forward;
        if (Input.GetKey(KeyCode.S))
            movement -= transform.forward;
        if (Input.GetKey(KeyCode.A))
            movement -= transform.right;
        if (Input.GetKey(KeyCode.D))
            movement += transform.right;

        if (movement != Vector3.zero)
        {
            // Calcular la nueva posición potencial
            Vector3 newPosition = transform.position + movement.normalized * velocidad_movimiento * Time.deltaTime;

            // Verificar si la nueva posición está dentro del área jugable
            if (IsPositionInPlayArea(newPosition))
            {
                transform.position = newPosition;
            }
        }
    }

    private bool IsPositionInPlayArea(Vector3 position)
    {
        // Hacer un overlap sphere en la nueva posición para verificar si está dentro del área jugable
        Collider[] hitColliders = Physics.OverlapSphere(position, 0.1f);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("PlayArea"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayArea"))
        {
            isInPlayArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayArea"))
        {
            isInPlayArea = false;
        }
    }
}