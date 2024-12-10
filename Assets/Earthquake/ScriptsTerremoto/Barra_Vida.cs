using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Barra_Vida : MonoBehaviour
{
    public Slider healthSlider;

    private float maxHealth = 5000f;
    private float currentHealth;

    public string GameOverScene = "";
    public Image damageOverlay;
    public float overlayFadeDuration = 0.5f;
    public float damageCooldown = 1f;

    private Coroutine damageCoroutine; // Reference to the active coroutine

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
        if (Safe_Zone_Manager.Instance.IsEarthquakeActive && !Safe_Zone_Manager.Instance.IsPlayerSafe) // Simulate damage
        {
            TakeDamage(1f); // Decrease health by 10
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (damageOverlay != null)
        {
            // Stop the currently running coroutine if it exists
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }

            // Start a new coroutine and keep a reference to it
            //damageCoroutine = StartCoroutine(FadeDamageOverlay());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            UpdateHealthBar();
        }
    }

    public void Heal(float healing)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = currentHealth; // Update the slider's value
    }

    private void Die()
    {
        SceneManager.LoadScene("PantallaPrincipal");
    }

    private IEnumerator FadeDamageOverlay()
    {
        // Set the overlay to full opacity and make it visible
        damageOverlay.color = new Color(1f, 0f, 0f, 0.5f);
        damageOverlay.gameObject.SetActive(true);

        float elapsedTime = 0f;

        // Gradually fade the overlay
        while (elapsedTime < overlayFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.7f, 0f, elapsedTime / overlayFadeDuration);
            damageOverlay.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        // Completely hide the overlay
        damageOverlay.gameObject.SetActive(false);

        // Reset coroutine reference to null
        damageCoroutine = null;
    }
}
