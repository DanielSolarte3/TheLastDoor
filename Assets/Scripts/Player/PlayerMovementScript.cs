using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 3.5f;

    [Header("Cámara")]
    public Transform cameraHolder;
    public float mouseSensitivity = 100;
    public float verticalRotationLimit = 80f;

    [SerializeField] private GameObject flashLight;
    private bool isAlive = true;
    private Rigidbody rb;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Oculta y bloquea el cursor
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isAlive) return;
        HandleCamera();
        HandleMovement();
        HandleFlashlight();
    }

    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rota el cuerpo del jugador (horizontal)
        transform.Rotate(Vector3.up * mouseX);

        // Rota la cámara (vertical)
        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalRotationLimit, verticalRotationLimit);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        vertical = -vertical;
        horizontal = -horizontal;
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        Vector3 velocity = moveDirection.normalized * moveSpeed;

        Vector3 newPosition = rb.position + velocity * Time.deltaTime;
        rb.MovePosition(newPosition);
    }
    void HandleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashLight.SetActive(!flashLight.activeSelf);
        }
    }
    public void Die()
    {
        isAlive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GetComponent<Rigidbody>().isKinematic = true;
        // Añadir animación de muerte si es necesario
    }
}