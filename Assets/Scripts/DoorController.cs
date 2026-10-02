using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door")]
    public Transform door;

    [Header("Door Settings")]
    public float openDistance = 50f;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool shouldOpen = false;

    private void Start()
    {
        if (door == null)
        {
            door = transform;
        }

        // Store the original closed rotation
        closedRotation = door.localRotation;

        // Calculate the open rotation
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        if (door == null)
            return;

        Quaternion targetRotation;

        if (shouldOpen)
        {
            targetRotation = openRotation;
        }
        else
        {
            targetRotation = closedRotation;
        }

        // Smoothly rotate the door
        door.localRotation = Quaternion.Slerp(
            door.localRotation,
            targetRotation,
            openSpeed * Time.deltaTime
        );
    }

    public void SetDistance(float distance)
    {
        Debug.Log("Door Distance: " + distance + " cm");

        if (distance < openDistance)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (!shouldOpen)
        {
            shouldOpen = true;
            Debug.Log("🚪 Door: OPEN");
        }
    }

    public void CloseDoor()
    {
        if (shouldOpen)
        {
            shouldOpen = false;
            Debug.Log("🚪 Door: CLOSED");
        }
    }
}