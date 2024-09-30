using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private readonly float rotationSpeed = 1f;

    public void RotateCamera(float xAxisRotation)
    {
        transform.Rotate(Vector3.down, -xAxisRotation * rotationSpeed);
    }
}