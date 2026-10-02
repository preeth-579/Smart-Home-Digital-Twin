using UnityEngine;

public class GasAlarmController : MonoBehaviour
{
    [Header("Gas Warning Visual")]
    public GameObject warningVisual;

    [Header("Gas Warning Audio")]
    public AudioSource alarmAudio;

    [Header("Gas Settings")]
    public float gasThreshold = 60f;

    private bool alarmActive = false;

    private void Start()
    {
        SetAlarmState(false);
    }

    public void SetGasLevel(float gasLevel)
    {
        Debug.Log("Gas Level: " + gasLevel + " %");

        if (gasLevel > gasThreshold)
        {
            TurnAlarmOn();
        }
        else
        {
            TurnAlarmOff();
        }
    }

    private void TurnAlarmOn()
    {
        if (!alarmActive)
        {
            alarmActive = true;

            Debug.Log("🚨 GAS WARNING! HIGH GAS LEVEL!");

            // Show warning visual
            if (warningVisual != null)
            {
                warningVisual.SetActive(true);
            }

            // Play warning audio
            if (alarmAudio != null)
            {
                if (!alarmAudio.isPlaying)
                {
                    alarmAudio.Play();
                }
            }
        }
    }

    private void TurnAlarmOff()
    {
        if (alarmActive)
        {
            alarmActive = false;

            Debug.Log("✅ Gas Level SAFE");

            // Hide warning visual
            if (warningVisual != null)
            {
                warningVisual.SetActive(false);
            }

            // Stop warning audio
            if (alarmAudio != null)
            {
                alarmAudio.Stop();
            }
        }
    }

    private void SetAlarmState(bool state)
    {
        alarmActive = state;

        if (warningVisual != null)
        {
            warningVisual.SetActive(state);
        }

        if (alarmAudio != null)
        {
            if (state)
            {
                if (!alarmAudio.isPlaying)
                {
                    alarmAudio.Play();
                }
            }
            else
            {
                alarmAudio.Stop();
            }
        }
    }
}