using UnityEngine;

public class FanController : MonoBehaviour
{
    [Header("Fan")]
    public Transform fanBlades;

    [Header("Temperature Settings")]
    public float temperatureThreshold = 20f;

    [Header("Fan Speed")]
    public float rotationSpeed = 500f;

    private bool fanOn = false;

    void Update()
    {
        if (fanOn && fanBlades != null)
        {
            // Reverse rotation direction
            fanBlades.Rotate(
                Vector3.up,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void SetTemperature(float temperature)
    {
        Debug.Log("Fan Temperature: " + temperature + " °C");

        if (temperature > temperatureThreshold)
        {
            TurnFanOn();
        }
        else
        {
            TurnFanOff();
        }
    }

    private void TurnFanOn()
    {
        if (!fanOn)
        {
            fanOn = true;
            Debug.Log("🌀 Fan: ON");
        }
    }

    private void TurnFanOff()
    {
        if (fanOn)
        {
            fanOn = false;
            Debug.Log("🌀 Fan: OFF");
        }
    }
}