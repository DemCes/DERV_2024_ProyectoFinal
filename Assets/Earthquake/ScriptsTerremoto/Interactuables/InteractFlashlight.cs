using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractFlashlight : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E; // Key to press for interaction
    private bool isPlayerNearby = false; // Tracks if the player is in range
     public bool isOneTimeUse = true; // Optional: make the first aid kit usable only once
    private bool hasBeenUsed = false; // Track if the first aid kit has been used
    public Light flashlight;

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
        flashlight.enabled = true;
        
        if (isOneTimeUse)
        {
            hasBeenUsed = true;
            // Optional: Disable or destroy the object after use
            gameObject.SetActive(false);
        }
    }
}
