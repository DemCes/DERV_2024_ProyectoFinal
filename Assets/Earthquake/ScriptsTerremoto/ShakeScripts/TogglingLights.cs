using UnityEngine;

public class TogglingLights: MonoBehaviour
{
    [Header("Light Settings")]
    public Light[] lights; // List of lights to toggle
    public float minToggleInterval = 0.5f; // Minimum time between toggles
    public float maxToggleInterval = 2f; // Maximum time between toggles

    private void Start()
    {
        // Start the light toggling coroutine
        StartCoroutine(ToggleLightsRandomly());
    }

    private System.Collections.IEnumerator ToggleLightsRandomly()
    {
        while (true)
        {
            // Wait for a random interval
            float waitTime = Random.Range(minToggleInterval, maxToggleInterval);
            yield return new WaitForSeconds(waitTime);

            // Pick a random light from the list
            if (lights.Length > 0)
            {
                Light randomLight = lights[Random.Range(0, lights.Length)];

                // Toggle the light's enabled state
                if (randomLight != null)
                {
                    randomLight.enabled = !randomLight.enabled;
                }
            }
        }
    }
}
