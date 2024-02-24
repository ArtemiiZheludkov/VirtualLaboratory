using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float rotationSpeed = 2.0f;

    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * rotationSpeed, 0);
    }
}