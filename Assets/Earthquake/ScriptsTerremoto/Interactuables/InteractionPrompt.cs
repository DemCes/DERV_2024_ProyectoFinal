using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    [Header("UI referencias")]
    [SerializeField] private GameObject interactionPromptPanel;
    [SerializeField] private TextMeshProUGUI interactionText;
    // Start is called before the first frame update [Header("Interaction Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string interactionMessage = "Press {0} to Interact";

    [Header("Trigger Settings")]
    private bool isPlayerNearby = false;

    private void Start()
    {
        // Validate UI references
        if (interactionPromptPanel == null || interactionText == null)
        {
            Debug.LogError("Interaction prompt UI is not fully configured!");
            return;
        }

        // Ensure prompt is hidden on start
        HideInteractionPrompt();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            ShowInteractionPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            HideInteractionPrompt();
        }
    }

    private void ShowInteractionPrompt()
    {
        if (interactionPromptPanel != null && interactionText != null)
        {
            // Format the interaction text with the specific interact key
            interactionText.text = string.Format(interactionMessage, interactKey.ToString());
            interactionPromptPanel.SetActive(true);
        }
    }

    private void HideInteractionPrompt()
    {
        if (interactionPromptPanel != null)
        {
            interactionPromptPanel.SetActive(false);
        }
    }

    // Optional: If you want to handle the interaction in this script
    //private void Update()
   // {
     //  if (isPlayerNearby && Input.GetKeyDown(interactKey))
      //  {
//            PerformInteraction();
      //  }
   // }

   // private void PerformInteraction()
   // {
        // Generic interaction method - can be overridden or expanded
//Debug.Log("Interacted with object!");
        
        // Example: You might want to call a method on another script
        // GetComponent<SomeOtherScript>().Interact();
    //}
}
