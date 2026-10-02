using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmartHomeUIController : MonoBehaviour
{
    [Header("Temperature")]
    public TMP_Text temperatureText;
    public Slider temperatureGauge;

    [Header("Humidity")]
    public TMP_Text humidityText;
    public Slider humidityGauge;

    [Header("Light")]
    public TMP_Text lightText;
    public Slider lightGauge;

    [Header("Soil Moisture")]
    public TMP_Text soilText;
    public Slider soilGauge;

    [Header("ThingSpeak Connection")]
    public TMP_Text connectionStatusText;

    [Header("Device Status")]
    public TMP_Text fanStatusText;
    public TMP_Text lightStatusText;
    public TMP_Text doorStatusText;
    public TMP_Text wateringStatusText;
    public TMP_Text motionStatusText;
    public TMP_Text gasStatusText;


    // ==========================================
    // THINGSPEAK CONNECTION
    // ==========================================

    public void SetConnectionStatus(bool online)
    {
        if (connectionStatusText != null)
        {
            if (online)
            {
                connectionStatusText.text =
                    "CONNECTION: ONLINE";
            }
            else
            {
                connectionStatusText.text =
                    "CONNECTION: OFFLINE";
            }
        }
    }


    // ==========================================
    // TEMPERATURE
    // ==========================================

    public void SetTemperature(float temperature)
    {
        if (temperatureText != null)
        {
            temperatureText.text =
                "Temperature: "
                + temperature.ToString("F1")
                + " °C";
        }

        if (temperatureGauge != null)
        {
            temperatureGauge.value = temperature;
        }

        if (fanStatusText != null)
        {
            if (temperature > 20f)
            {
                fanStatusText.text = "FAN: ON";
            }
            else
            {
                fanStatusText.text = "FAN: OFF";
            }
        }
    }


    // ==========================================
    // HUMIDITY
    // ==========================================

    public void SetHumidity(float humidity)
    {
        if (humidityText != null)
        {
            humidityText.text =
                "Humidity: "
                + humidity.ToString("F1")
                + " %";
        }

        if (humidityGauge != null)
        {
            humidityGauge.value = humidity;
        }
    }


    // ==========================================
    // LIGHT
    // ==========================================

    public void SetLight(float light)
    {
        if (lightText != null)
        {
            lightText.text =
                "Light: "
                + light.ToString("F1")
                + " %";
        }

        if (lightGauge != null)
        {
            lightGauge.value = light;
        }

        if (lightStatusText != null)
        {
            if (light < 30f)
            {
                lightStatusText.text = "LIGHT: ON";
            }
            else
            {
                lightStatusText.text = "LIGHT: OFF";
            }
        }
    }


    // ==========================================
    // SOIL MOISTURE
    // ==========================================

    public void SetSoilMoisture(float soilMoisture)
    {
        if (soilText != null)
        {
            soilText.text =
                "Soil: "
                + soilMoisture.ToString("F1")
                + " %";
        }

        if (soilGauge != null)
        {
            soilGauge.value = soilMoisture;
        }

        if (wateringStatusText != null)
        {
            if (soilMoisture < 30f)
            {
                wateringStatusText.text = "WATERING: ON";
            }
            else
            {
                wateringStatusText.text = "WATERING: OFF";
            }
        }
    }


    // ==========================================
    // DOOR / DISTANCE
    // ==========================================

    public void SetDistance(float distance)
    {
        if (doorStatusText != null)
        {
            if (distance < 50f)
            {
                doorStatusText.text = "DOOR: OPEN";
            }
            else
            {
                doorStatusText.text = "DOOR: CLOSED";
            }
        }
    }


    // ==========================================
    // MOTION
    // ==========================================

    public void SetMotion(bool detected)
    {
        if (motionStatusText != null)
        {
            if (detected)
            {
                motionStatusText.text = "MOTION: DETECTED";
            }
            else
            {
                motionStatusText.text = "MOTION: NONE";
            }
        }
    }


    // ==========================================
    // GAS
    // ==========================================

    public void SetGas(float gas)
    {
        if (gasStatusText != null)
        {
            if (gas > 60f)
            {
                gasStatusText.text = "GAS: WARNING";
            }
            else
            {
                gasStatusText.text = "GAS: SAFE";
            }
        }
    }
}