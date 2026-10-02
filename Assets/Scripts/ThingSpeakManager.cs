using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class ThingSpeakData
{
    public string created_at;
    public int entry_id;

    public string field1; // Temperature
    public string field2; // Humidity
    public string field3; // Distance
    public string field4; // Light
    public string field5; // Motion
    public string field6; // Gas
    public string field7; // Soil Moisture
}

public class ThingSpeakManager : MonoBehaviour
{
    [Header("ThingSpeak")]
    public string channelID = "3477783";
    public string readAPIKey = "4BHBPMEUT2TPLCB4";

    [Header("Settings")]
    public float updateInterval = 10f;

    [Tooltip("Data older than this many seconds will be considered OFFLINE.")]
    public float dataTimeout = 60f;

    [Header("Digital Twin Controllers")]
    public LightController lightController;
    public FanController fanController;
    public MotionController motionController;
    public GasAlarmController gasAlarmController;
    public DoorController doorController;
    public SmartPlantController smartPlantController;

    [Header("UI Dashboard")]
    public SmartHomeUIController uiController;

    private string apiURL;

    private void Start()
    {
        apiURL = "https://api.thingspeak.com/channels/"
                 + channelID
                 + "/feeds/last.json";

        if (!string.IsNullOrEmpty(readAPIKey))
        {
            apiURL += "?api_key=" + readAPIKey;
        }

        // Start as OFFLINE until fresh data is received.
        if (uiController != null)
        {
            uiController.SetConnectionStatus(false);
        }

        StartCoroutine(UpdateThingSpeakData());
    }

    private IEnumerator UpdateThingSpeakData()
    {
        while (true)
        {
            yield return StartCoroutine(GetLatestData());

            yield return new WaitForSeconds(updateInterval);
        }
    }

    private IEnumerator GetLatestData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiURL))
        {
            yield return request.SendWebRequest();

            // ==========================================
            // REQUEST FAILED
            // ==========================================

            if (request.result != UnityWebRequest.Result.Success)
            {
                SetConnectionStatus(false);

                Debug.LogError(
                    "ThingSpeak Error: "
                    + request.error
                );

                yield break;
            }

            // ==========================================
            // REQUEST SUCCESSFUL
            // ==========================================

            string json = request.downloadHandler.text;

            Debug.Log("ThingSpeak Response:");
            Debug.Log(json);

            ThingSpeakData data =
                JsonUtility.FromJson<ThingSpeakData>(json);

            // ==========================================
            // INVALID DATA
            // ==========================================

            if (data == null)
            {
                SetConnectionStatus(false);

                Debug.LogError(
                    "Failed to parse ThingSpeak data."
                );

                yield break;
            }

            // ==========================================
            // CHECK DATA FRESHNESS
            // ==========================================

            bool dataIsFresh = IsDataFresh(data.created_at);

            if (dataIsFresh)
            {
                SetConnectionStatus(true);

                Debug.Log(
                    "ThingSpeak: FRESH DATA - ONLINE"
                );

                ProcessData(data);
            }
            else
            {
                SetConnectionStatus(false);

                Debug.LogWarning(
                    "ThingSpeak: DATA IS OLD - OFFLINE"
                );

                Debug.LogWarning(
                    "Last ThingSpeak update: "
                    + data.created_at
                );
            }
        }
    }

    // ==========================================
    // CHECK WHETHER THINGSPEAK DATA IS FRESH
    // ==========================================

    private bool IsDataFresh(string createdAt)
    {
        if (string.IsNullOrEmpty(createdAt))
        {
            Debug.LogWarning(
                "ThingSpeak created_at is empty."
            );

            return false;
        }

        DateTimeOffset timestamp;

        bool parsed = DateTimeOffset.TryParse(
            createdAt,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal |
            DateTimeStyles.AdjustToUniversal,
            out timestamp
        );

        if (!parsed)
        {
            Debug.LogWarning(
                "Could not parse ThingSpeak timestamp: "
                + createdAt
            );

            return false;
        }

        double ageInSeconds =
            (DateTimeOffset.UtcNow - timestamp).TotalSeconds;

        Debug.Log(
            "ThingSpeak data age: "
            + ageInSeconds.ToString("F1")
            + " seconds"
        );

        // Protect against unusual future timestamps.
        if (ageInSeconds < 0)
        {
            return true;
        }

        return ageInSeconds <= dataTimeout;
    }

    // ==========================================
    // CONNECTION STATUS
    // ==========================================

    private void SetConnectionStatus(bool online)
    {
        if (uiController != null)
        {
            uiController.SetConnectionStatus(online);
        }
    }

    // ==========================================
    // PROCESS SENSOR DATA
    // ==========================================

    private void ProcessData(ThingSpeakData data)
    {
        // ==========================================
        // FIELD 1 - TEMPERATURE
        // ==========================================

        if (!string.IsNullOrEmpty(data.field1))
        {
            float temperature;

            if (float.TryParse(
                data.field1,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out temperature))
            {
                Debug.Log(
                    "Temperature: "
                    + temperature
                    + " °C"
                );

                if (fanController != null)
                {
                    fanController.SetTemperature(
                        temperature
                    );
                }

                if (uiController != null)
                {
                    uiController.SetTemperature(
                        temperature
                    );
                }
            }
        }

        // ==========================================
        // FIELD 2 - HUMIDITY
        // ==========================================

        if (!string.IsNullOrEmpty(data.field2))
        {
            float humidity;

            if (float.TryParse(
                data.field2,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out humidity))
            {
                Debug.Log(
                    "Humidity: "
                    + humidity
                    + " %"
                );

                if (uiController != null)
                {
                    uiController.SetHumidity(
                        humidity
                    );
                }
            }
        }

        // ==========================================
        // FIELD 3 - ULTRASONIC DISTANCE
        // ==========================================

        if (!string.IsNullOrEmpty(data.field3))
        {
            float distance;

            if (float.TryParse(
                data.field3,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out distance))
            {
                Debug.Log(
                    "Distance: "
                    + distance.ToString("F2")
                    + " cm"
                );

                if (doorController != null)
                {
                    doorController.SetDistance(
                        distance
                    );
                }

                if (uiController != null)
                {
                    uiController.SetDistance(
                        distance
                    );
                }
            }
        }

        // ==========================================
        // FIELD 4 - LDR / LIGHT
        // ==========================================

        if (!string.IsNullOrEmpty(data.field4))
        {
            float lightValue;

            if (float.TryParse(
                data.field4,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out lightValue))
            {
                Debug.Log(
                    "LDR Value: "
                    + lightValue
                    + " %"
                );

                if (lightController != null)
                {
                    lightController.SetLightLevel(
                        lightValue
                    );
                }

                if (uiController != null)
                {
                    uiController.SetLight(
                        lightValue
                    );
                }
            }
        }

        // ==========================================
        // FIELD 5 - PIR MOTION
        // ==========================================

        if (!string.IsNullOrEmpty(data.field5))
        {
            float motionValue;

            if (float.TryParse(
                data.field5,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out motionValue))
            {
                bool motionDetected =
                    motionValue >= 1f;

                Debug.Log(
                    "Motion: "
                    + (motionDetected
                        ? "DETECTED"
                        : "NO MOTION")
                );

                if (motionController != null)
                {
                    motionController.SetMotion(
                        motionDetected
                    );
                }

                if (uiController != null)
                {
                    uiController.SetMotion(
                        motionDetected
                    );
                }
            }
        }

        // ==========================================
        // FIELD 6 - GAS SENSOR
        // ==========================================

        if (!string.IsNullOrEmpty(data.field6))
        {
            float gasLevel;

            if (float.TryParse(
                data.field6,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out gasLevel))
            {
                Debug.Log(
                    "Gas Level: "
                    + gasLevel
                    + " %"
                );

                if (gasAlarmController != null)
                {
                    gasAlarmController.SetGasLevel(
                        gasLevel
                    );
                }

                if (uiController != null)
                {
                    uiController.SetGas(
                        gasLevel
                    );
                }
            }
        }

        // ==========================================
        // FIELD 7 - SOIL MOISTURE
        // ==========================================

        if (!string.IsNullOrEmpty(data.field7))
        {
            float soilMoisture;

            if (float.TryParse(
                data.field7,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out soilMoisture))
            {
                Debug.Log(
                    "Soil Moisture: "
                    + soilMoisture
                    + " %"
                );

                if (smartPlantController != null)
                {
                    smartPlantController.SetSoilMoisture(
                        soilMoisture
                    );
                }

                if (uiController != null)
                {
                    uiController.SetSoilMoisture(
                        soilMoisture
                    );
                }
            }
        }
    }
}