using UnityEngine;

public class FocusOnKeypadSwitchCam : MonoBehaviour
{
    public Transform player;
    public GameObject playerObject;             // El GameObject completo del jugador
    public GameObject keypadCamera;             // Cámara frente al Keypad (inicialmente desactivada)
    public GameObject keypadLight;              // Luz en el Keypad (opcional)
    public float triggerDistance = 2.5f;

    private bool isFocused = false;

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (!isFocused && distance <= triggerDistance && Input.GetKeyDown(KeyCode.E))
        {
            // Desactivar jugador y su cámara
            playerObject.SetActive(false);

            // Activar la cámara y luz del keypad
            keypadCamera.SetActive(true);
            if (keypadLight != null)
                keypadLight.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            isFocused = true;
        }

        if (isFocused && Input.GetKeyDown(KeyCode.Escape))
        {
            // Reactivar el jugador
            playerObject.SetActive(true);

            // Desactivar la cámara y luz del keypad
            keypadCamera.SetActive(false);
            if (keypadLight != null)
                keypadLight.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isFocused = false;
        }
    }
}
