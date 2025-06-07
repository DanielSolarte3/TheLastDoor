using UnityEngine;

public class CameraView : MonoBehaviour
{
    [Header("Camara")]
    public float sensibilidad = 100f;
    public float rotacionX = 0f;
    public Transform player;

    [SerializeField] private GameObject flashLight;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Oculta y bloquea el cursor
        Cursor.visible = false; // Asegura que el cursor no sea visible
    }

    void Update()
    {
        HandleCamera();
        HandleFlashlight();
    }

    void HandleCamera()
    {
        float MouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        rotacionX -= MouseY;
        rotacionX = Mathf.Clamp(rotacionX, -90f, 90f); // Limita la rotación vertical para evitar voltear la cámara

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        player.Rotate(Vector3.up* MouseX);
    }

    void HandleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashLight.SetActive(!flashLight.activeSelf);
        }
    }
}
