using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCollision : MonoBehaviour
{
    [SerializeField] private Transform cameraPosition;
    [SerializeField] private Transform newCameraPosition;
    [SerializeField] private Barra_Vida barra_Vida;
    private Transform originalcameraPosition;
    public GameObject cameraHolder;
    public GameObject jugador;
    private bool isPlayerCameraEnabled = true;
    public KeyCode interactKey = KeyCode.E; // Key to press for interaction
    private bool isPlayerNearby = false; // Tracks if the player is in range
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entro" );
        if (other.CompareTag("Player")) // Ensure the player is tagged appropriately
        {
            isPlayerNearby = true;
            Debug.Log("Player is near the interactable object.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player left the interactable area.");
        }
    }
    private void Start() {
        if(cameraHolder == null || jugador == null)
        {
            Debug.LogError("No se encontro jugador ni cameraholder.");
            return;
        }
        originalcameraPosition = cameraPosition;
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Debug.Log("Interacted with the object!");
        if(isPlayerCameraEnabled)
        {
            DisablePlayerMovement();
            Safe_Zone_Manager.Instance.PlayerEnteredSafeZone();
            cameraPosition.position = newCameraPosition.position;
        }
        else
        {
            EnablePlayerMovement();
            Safe_Zone_Manager.Instance.PlayerExitedSafeZone();
            cameraPosition.position = originalcameraPosition.position;

        }
        
        
    }
    private void DisablePlayerMovement()
    {
        CHolderPosition script = cameraHolder.GetComponent<CHolderPosition>();
        AdvancedMovements scriptMovimiento = jugador.GetComponent<AdvancedMovements>();
        if (script != null && scriptMovimiento != null)
        {
            script.enabled = false;
            scriptMovimiento.enabled = false;
            isPlayerCameraEnabled = false;
        }
        else
        {
            Debug.LogError("No encontro el script");
        }
    }
    private void EnablePlayerMovement()
    {
        CHolderPosition script = cameraHolder.GetComponent<CHolderPosition>();
        AdvancedMovements scriptMovimiento = jugador.GetComponent <AdvancedMovements>();
        if (script != null && scriptMovimiento != null)
        {
            script.enabled = true;
            scriptMovimiento.enabled = true;
            isPlayerCameraEnabled = true;
        }
        else
        {
            Debug.LogError("No encontro el script");
        }
    }
}
