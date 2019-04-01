using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    public bool free = false;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        if (free)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");

            float newRotY = mouseX * mouseSensitivity * Time.deltaTime;
            float newRotX = mouseY * mouseSensitivity * Time.deltaTime;

            // mise a jour uniquement s'il y modification de la rotation de la camera
            if (rotY + newRotY != rotY || rotX + newRotX != rotX)
            {
                rotY += newRotY;
                rotX += newRotX;
                rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

                Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
                transform.rotation = localRotation;

                // mise a jour aliments plateau pour correction probleme slice 
                AtelierManager.Instance().UpdateAlimentPlateau();
            }
        }
    }
    public void setCameraFree(bool value)
    {
        free = value;
        AtelierManager.Instance().CursorTarget.enabled = value;
        if (!value)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
