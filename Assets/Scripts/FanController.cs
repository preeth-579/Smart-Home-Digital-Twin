using UnityEngine;

public class FanController : MonoBehaviour
{
    [Header("Fans")]
    public Transform[] fanBlades;

    [Header("Temperature Settings")]
    public float temperatureThreshold = 20f;

    [Header("Fan Speed")]
    public float rotationSpeed = 500f;

    private bool fanOn = false;

    private void Update()
    {
        if (fanOn && fanBlades != null)
        {
            foreach (Transform fan in fanBlades)
            {
                if (fan != null)
                {
                    // Rotate both fans around Y axis
                    fan.Rotate(
                        Vector3.up,
                        rotationSpeed * Time.deltaTime
                    );
                }
            }
        }
    }

    public void SetTemperature(float temperature)
    {
        Debug.Log("Fan Temperature: " + temperature + " °C");

        if (temperature > temperatureThreshold)
        {
            TurnFansOn();
        }
        else
        {
            TurnFansOff();
        }
    }

    private void TurnFansOn()
    {
        if (!fanOn)
        {
            fanOn = true;

            Debug.Log("🌀 Fan 1: ON");
            Debug.Log("🌀 Fan 2: ON");
        }
    }

    private void TurnFansOff()
    {
        if (fanOn)
        {
            fanOn = false;

            Debug.Log("🌀 Fan 1: OFF");
            Debug.Log("🌀 Fan 2: OFF");
        }
    }
}