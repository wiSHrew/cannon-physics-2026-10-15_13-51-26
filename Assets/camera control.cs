
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float mouseSensitivity = 200f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        transform.Rotate(
            0f,
            mouseX * mouseSensitivity * Time.deltaTime,
            0f
        );
    }
}
