using UnityEngine;

public class SmartPlantController : MonoBehaviour
{
    [Header("Plant")]
    public GameObject plantVisual;

    [Header("Watering System")]
    public GameObject waterVisual;

    [Header("Soil Moisture Settings")]
    public float moistureThreshold = 30f;

    private bool watering = false;

    private void Start()
    {
        SetWateringState(false);
    }

    public void SetSoilMoisture(float soilMoisture)
    {
        Debug.Log(
            "🌱 Soil Moisture: "
            + soilMoisture
            + " %"
        );

        if (soilMoisture < moistureThreshold)
        {
            StartWatering();
        }
        else
        {
            StopWatering();
        }
    }

    private void StartWatering()
    {
        if (!watering)
        {
            watering = true;

            Debug.Log(
                "💧 AUTOMATIC WATERING ON"
            );

            if (waterVisual != null)
            {
                waterVisual.SetActive(true);
            }
        }
    }

    private void StopWatering()
    {
        if (watering)
        {
            watering = false;

            Debug.Log(
                "🌱 SOIL MOISTURE SUFFICIENT - WATERING OFF"
            );

            if (waterVisual != null)
            {
                waterVisual.SetActive(false);
            }
        }
    }

    private void SetWateringState(bool state)
    {
        watering = state;

        if (waterVisual != null)
        {
            waterVisual.SetActive(state);
        }
    }
}