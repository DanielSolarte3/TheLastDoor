using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controlador;
    [Header("Movimiento")]
    public float velocidad = 3f;
    public float gravedad = -9.81f;

    public Transform enElPiso;
    public float distanciaDelPiso;
    public LayerMask mascaraDelPiso;

    Vector3 velocidadAbajo;
    bool estaEnElPiso;

    void Start()
    {
        
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        estaEnElPiso = Physics.CheckSphere(enElPiso.position, distanciaDelPiso, mascaraDelPiso);

        if (estaEnElPiso && velocidadAbajo.y < 0)
        {
            velocidadAbajo.y = -2f; // Reiniciar la velocidad vertical al tocar el suelo
        }

        float x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f;

        Vector3 mover = transform.right * x + transform.forward * z;
        controlador.Move(mover * velocidad * Time.deltaTime);

        velocidadAbajo.y += gravedad * Time.deltaTime; // Aplicar gravedad

        controlador.Move(velocidadAbajo * Time.deltaTime);
    }
}
