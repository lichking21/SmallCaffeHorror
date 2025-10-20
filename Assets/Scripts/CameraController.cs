using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensivity = 100f;
    private float xRot;
    private float yRot;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        RotateCam();
    }
    
    private void RotateCam()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensivity * Time.smoothDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensivity * Time.smoothDeltaTime;

        xRot -= mouseY;
        yRot += mouseX;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
    }
}