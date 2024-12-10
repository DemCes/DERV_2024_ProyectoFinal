using UnityEngine;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
    public Slider batterySlider;
    public Light flashlight;

    public float maxBattery = 100f;
    private float currentBattery;

    public float drainRate = 5f;      // How quickly battery drains when on
    public float chargeRate = 10f;    // How quickly battery charges when collecting battery pickups

    void Start()
    {
        currentBattery = maxBattery;
        batterySlider.maxValue = maxBattery;
        batterySlider.value = currentBattery;
    }

    void Update()
    {
        // Drain battery when flashlight is on
        if (flashlight.enabled)
        {
            currentBattery -= drainRate * Time.deltaTime;
            
            // Turn off flashlight if battery is depleted
            if (currentBattery <= 0)
            {
                currentBattery = 0;
                flashlight.enabled = false;
            }
        }

        UpdateBatteryBar();
    }

    public void ChargeBattery(float amount)
    {
        currentBattery += amount;
        if (currentBattery > maxBattery) currentBattery = maxBattery;
    }

    private void UpdateBatteryBar()
    {
        batterySlider.value = currentBattery;
    }

    // Optional: Method to toggle flashlight
    public void ToggleFlashlight()
    {
        if (currentBattery > 0)
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}