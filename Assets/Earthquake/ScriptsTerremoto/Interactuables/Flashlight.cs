using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlightLight; // Reference to the flashlight light
    public KeyCode toggleKey = KeyCode.F; // Key to toggle the flashlight
    private bool isPickedUp = false; // Tracks if the flashlight is picked up
    private bool isOn = false; // Tracks if the flashlight is on

    void Start()
    {
        if (flashlightLight != null)
        {
            flashlightLight.enabled = false; // Ensure flashlight is off initially
        }
    }

    void Update()
    {
        // If the flashlight is picked up, allow toggling
        if (isPickedUp && Input.GetKeyDown(toggleKey))
        {
            isOn = !isOn;
            flashlightLight.enabled = isOn;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the flashlight's trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("Press 'E' to pick up the flashlight.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PickUpFlashlight(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You left the flashlight.");
        }
    }

    private void PickUpFlashlight(GameObject player)
    {
        Debug.Log("Flashlight picked up!");
        isPickedUp = true;

        // Attach the flashlight to the player
        transform.SetParent(player.transform);
        transform.localPosition = new Vector3(0.3f, -0.5f, 0.5f); // Adjust position relative to the player
        transform.localRotation = Quaternion.identity;

        // Disable the collider so the flashlight can't be picked up again
        GetComponent<Collider>().enabled = false;
    }
}
