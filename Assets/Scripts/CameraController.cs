using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Positions")]
    public Transform cameraPosition1;
    public Transform cameraPosition2;
    public Transform cameraPosition3;
    public Transform cameraPosition4;

    [Header("Main Camera")]
    public Transform mainCamera;

    private void Start()
    {
        SetCameraPosition(cameraPosition1);
    }

    public void Camera1()
    {
        SetCameraPosition(cameraPosition1);
    }

    public void Camera2()
    {
        SetCameraPosition(cameraPosition2);
    }

    public void Camera3()
    {
        SetCameraPosition(cameraPosition3);
    }

    public void Camera4()
    {
        SetCameraPosition(cameraPosition4);
    }

    private void SetCameraPosition(Transform target)
    {
        if (target == null || mainCamera == null)
            return;

        mainCamera.position = target.position;
        mainCamera.rotation = target.rotation;
    }
}