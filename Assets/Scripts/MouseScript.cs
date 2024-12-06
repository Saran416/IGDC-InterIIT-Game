using UnityEngine;
using UnityEngine.UI;

public class MouseScript : MonoBehaviour
{
    [Range(100f, 1000f)] public float sens;
    public Slider sensitivity;
    public Transform playerBody;

    private float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        sens = sensitivity.value;

        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime; 

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 70f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
