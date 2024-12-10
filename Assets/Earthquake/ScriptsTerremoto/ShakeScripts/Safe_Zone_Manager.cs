using UnityEngine;
using System.Collections;
using TMPro;
public class Safe_Zone_Manager : MonoBehaviour
{
    public static Safe_Zone_Manager Instance { get; private set; }

    [SerializeField] private Barra_Vida barraVida;  // Reference to health bar
    [SerializeField] private EarthquakeShake cameraShake; // Reference to camera shake logic
    [SerializeField] private float minTime = 5;
    [SerializeField] private float maxTime = 10;
    [SerializeField] private TextMeshProUGUI earthquakeTimerText;

    private Coroutine damageCoroutine;
    private Coroutine earthquakeCoroutine;
    public bool IsEarthquakeActive { get; private set; } = false;
    public bool IsPlayerSafe { get; private set; } = false;
    private float currentEarthquakeCountdown = 0f;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // Validate references
        if (barraVida == null || cameraShake == null)
        {
            Debug.LogError("SafeZoneManager is missing references!");
        }
        else
        {
            StartCoroutine(EarthquakeSequence());
        }
    }

    private IEnumerator EarthquakeSequence()
    {
        while (true)
        {
            currentEarthquakeCountdown = Random.Range(minTime,maxTime);
            IsEarthquakeActive = false;
            while(currentEarthquakeCountdown > 0)
            {
                UpdateEarthquakeTimerUI();
                currentEarthquakeCountdown -= Time.deltaTime;
                yield return null;
            }

            SimulateEarthquake();

            IsEarthquakeActive = true;

            if(!IsPlayerSafe)
            {
                StartDamage();
            }
            yield return new WaitForSeconds(cameraShake.duration);
            IsEarthquakeActive = false;
        }
    }
    private void UpdateEarthquakeTimerUI()
    {
        if(earthquakeTimerText != null)
        {
            earthquakeTimerText.text = $"Terremoto: {currentEarthquakeCountdown :F0}";
        }
    }
    public void PlayerEnteredSafeZone()
    {
        IsPlayerSafe = true;
        Debug.Log("Player entered a safe zone.");
        StopDamage();
    }

    public void PlayerExitedSafeZone()
    {
        IsPlayerSafe = false;
        Debug.Log("Player left the safe zone.");
    }

    public void SimulateEarthquake()
    {
        if (cameraShake != null)
        {
            cameraShake.duration = Random.Range(1f, cameraShake.duration); // Randomize duration
            cameraShake.TriggerEarthquake();
        }
        else
        {
            Debug.LogWarning("Camera shake is not assigned!");
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while (IsPlayerSafe == false) // Apply damage while the player is unsafe
        {
            barraVida.TakeDamage(1); // Adjust damage amount as needed
            yield return new WaitForSeconds(1); // Apply damage every 1 second
        }

        // Reset coroutine reference when it ends
        damageCoroutine = null;
    }

    public void StartDamage()
    {
        if (damageCoroutine == null) // Avoid starting multiple coroutines
        {
            damageCoroutine = StartCoroutine(ApplyDamageOverTime());
        }
    }

    private void StopDamage()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }
     private IEnumerator RandomEarthQuake()
    {
        while (true) // Keep running indefinitely
        {
            // Wait for a random amount of time
            float randomTime = Random.Range(minTime, maxTime);
            IsEarthquakeActive = false;
            yield return new WaitForSeconds(randomTime);

            // Trigger the function
            // Continuously check player's safe status and apply damage if necessary
            SimulateEarthquake();
            IsEarthquakeActive = true;
             if (IsPlayerSafe == false)
            {
                StartDamage();
            }
        }
    }
    public void StopEarthquakeSequence()
    {
        if (earthquakeCoroutine != null)
        {
            StopCoroutine(earthquakeCoroutine);
            earthquakeCoroutine = null;
        }
    }
}
