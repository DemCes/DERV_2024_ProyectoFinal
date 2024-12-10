using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Botiquin : MonoBehaviour
{
    [SerializeField] private Barra_Vida barra_Vida;
    public KeyCode interactKey = KeyCode.E; // Key to press for interaction
    private bool isPlayerNearby = false; // Tracks if the player is in range
    public float maxHealing = 500f; // Adjusted to a more reasonable value
    public float minHealing = 100f; // Adjusted to a more reasonable value
    public bool isOneTimeUse = true; // Optional: make the first aid kit usable only once
    private bool hasBeenUsed = false; // Track if the first aid kit has been used

    void Start()
    {
        if (barra_Vida == null)
        {
            Debug.LogError("Barra de Vida is not assigned to the first aid kit!");
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
        float healingAmount = Random.Range(minHealing, maxHealing);
        
        // Apply healing
        barra_Vida.Heal(healingAmount);
        
        // Log the healing amount
        Debug.Log($"Healed for {healingAmount} health");
        
        // Mark as used if it's a one-time use first aid kit
        if (isOneTimeUse)
        {
            hasBeenUsed = true;
            // Optional: Disable or destroy the object after use
            gameObject.SetActive(false);
        }
    }
}