using UnityEngine;

public class EarthquakeShake : MonoBehaviour
{
    [Header("Earthquake Shake Parametros")]
    [Tooltip("Intensidad")]
    [Range(0f, 10f)]
    public float intensity = 5f;

    [Tooltip("Duracion del terremoto")]
    [Range(0f, 10f)]
    public float duration = 3f;

    [Tooltip("Frecuencia de las sacudidas")]
    [Range(1f, 50f)]
    public float frequency = 25f;

    [Tooltip("Tipo de sacudida")]
    public ShakeType shakeType = ShakeType.Realistic;

    [Header("Advanced Settings")]
    [Tooltip("Randomness factor para mas realismo")]
    [Range(0f, 1f)]
    public float randomnessFactor = 0.5f;

    [Tooltip("Deescalar ")]
    public bool decayOverTime = true;

    // Enum to define different shake types
    public enum ShakeType
    {
        Realistic,
        Violent,
        Subtle
    }

    private Vector3 originalPosition;
    private float shakeTimer;
    private float peakIntensity;

    private void Start()
    {
        // Ensure we're attached to a camera
        if (GetComponent<Camera>() == null)
        {
            Debug.LogWarning("EarthquakeCameraShake script should be attached to a Camera!");
        }
    }

    public void TriggerEarthquake()
    {
        // Store original camera position
        originalPosition = transform.localPosition;
        
        // Set initial shake parameters
        shakeTimer = duration;
        peakIntensity = intensity;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // Reduce shake timer
            shakeTimer -= Time.deltaTime;

            // Calculate decay factor if enabled
            float decayFactor = decayOverTime 
                ? Mathf.Clamp01(shakeTimer / duration) 
                : 1f;

            // Adjust intensity based on decay
            float currentIntensity = peakIntensity * decayFactor;

            // Generate shake based on type
            Vector3 shakeOffset = GenerateShakeOffset(currentIntensity);

            // Apply shake to camera position
            transform.localPosition = originalPosition + shakeOffset;

            // Reset position when shake is complete
            if (shakeTimer <= 0)
            {
                transform.localPosition = originalPosition;
            }
        }
    }

    private Vector3 GenerateShakeOffset(float currentIntensity)
    {
        // Different shake patterns based on shake type
        switch (shakeType)
        {
            case ShakeType.Realistic:
                return GenerateRealisticShake(currentIntensity);
            case ShakeType.Violent:
                return GenerateViolentShake(currentIntensity);
            case ShakeType.Subtle:
                return GenerateSubtleShake(currentIntensity);
            default:
                return GenerateRealisticShake(currentIntensity);
        }
    }

    private Vector3 GenerateRealisticShake(float currentIntensity)
    {
        // Create more natural, less predictable shake
        float x = Mathf.PerlinNoise(Time.time * frequency, 0) * 2 - 1;
        float y = Mathf.PerlinNoise(0, Time.time * frequency) * 2 - 1;
        float z = Mathf.PerlinNoise(Time.time * frequency, Time.time * frequency) * 2 - 1;

        // Apply randomness and intensity
        return new Vector3(
            x * currentIntensity * randomnessFactor, 
            y * currentIntensity * randomnessFactor, 
            z * currentIntensity * randomnessFactor * 0.5f  // Less z-axis movement
        );
    }

    private Vector3 GenerateViolentShake(float currentIntensity)
    {
        // More extreme, rapid movement
        return new Vector3(
            Random.Range(-1f, 1f) * currentIntensity * 1.5f,
            Random.Range(-1f, 1f) * currentIntensity * 1.5f,
            Random.Range(-0.5f, 0.5f) * currentIntensity
        );
    }

    private Vector3 GenerateSubtleShake(float currentIntensity)
    {
        // Gentler, more controlled shake
        return new Vector3(
            Mathf.Sin(Time.time * frequency) * currentIntensity * 0.5f,
            Mathf.Cos(Time.time * frequency) * currentIntensity * 0.5f,
            0
        );
    }

    // Optional method to simulate aftershocks
    public void TriggerAfterShock(float aftershockIntensity = -1f)
    {
        // If no intensity specified, use a reduced version of main shake
        float finalIntensity = aftershockIntensity >= 0 
            ? aftershockIntensity 
            : intensity * Random.Range(0.3f, 0.7f);

        // Trigger a shorter, potentially less intense shake
        intensity = finalIntensity;
        duration *= Random.Range(0.5f, 0.8f);
        TriggerEarthquake();
    }
}