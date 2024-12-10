using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pilas : MonoBehaviour
{
    [SerializeField] private FlashlightBattery linterna;
    public KeyCode interactKey = KeyCode.E; // Key to press for interaction
    private bool isPlayerNearby = false; // Tracks if the player is in range
    public float maxCharge = 75f; // Adjusted to a more reasonable value
    public float minCharge = 25f; // Adjusted to a more reasonable value
    public bool isOneTimeUse = true; // Optional: make the first aid kit usable only once
    private bool hasBeenUsed = false; // Track if the first aid kit has been used
    // Start is called before the first frame update
    void Start()
    {
        if (linterna == null)
        {
            Debug.LogWarning("Linterna no asignada");
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Player is near the first aid kit.");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Player left the first aid kit area.");
        }
    }

    private void Update()
    {
        // Check if player is nearby, can interact, and the first aid kit is available
        if (isPlayerNearby && Input.GetKeyDown(interactKey) && CanInteract())
        {
            Interact();
        }
    }

    private bool CanInteract()
    {
        // If it's a one-time use, check if it hasn't been used before
        return !isOneTimeUse || !hasBeenUsed;
    }

    private void Interact()
    {
        // Calculate healing amount
        float chargingAmount = Random.Range(minCharge, maxCharge);
        
        // Apply healing
        linterna.ChargeBattery(chargingAmount);
        
        // Log the healing amount
        Debug.Log($"Cargado por {chargingAmount}");
        
        // Mark as used if it's a one-time use first aid kit
        if (isOneTimeUse)
        {
            hasBeenUsed = true;
            // Optional: Disable or destroy the object after use
            gameObject.SetActive(false);
        }
    }
}
