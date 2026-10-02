using UnityEngine;

public class MotionController : MonoBehaviour
{
    [Header("Person Visual")]
    public GameObject personVisual;

    [Header("Motion Settings")]
    public bool showPersonWhenMotionDetected = true;

    private bool motionDetected = false;

    public void SetMotion(bool detected)
    {
        motionDetected = detected;

        if (motionDetected)
        {
            PersonDetected();
        }
        else
        {
            PersonNotDetected();
        }
    }

    private void PersonDetected()
    {
        Debug.Log("👤 PERSON DETECTED");

        if (personVisual != null)
        {
            personVisual.SetActive(showPersonWhenMotionDetected);
        }
    }

    private void PersonNotDetected()
    {
        Debug.Log("👤 NO PERSON DETECTED");

        if (personVisual != null)
        {
            personVisual.SetActive(!showPersonWhenMotionDetected);
        }
    }
}