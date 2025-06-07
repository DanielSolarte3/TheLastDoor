using UnityEngine;

public class SystenInteractive : MonoBehaviour
{
    // Tipo de interacción
    public enum InteractionType { Rotate, Move }
    public InteractionType interactionType = InteractionType.Rotate;

    [Header("General Settings")]
    public bool objectOpen = false;
    public float smooth = 3.0f;

    [Header("Rotation Settings")]
    public float objectOpenAngle = 0.0f;
    public float objectCloseAngle = 90f;

    [Header("Movement Settings")]
    public Vector3 openPositionOffset = new Vector3(0, 0, 0.5f);
    private Vector3 closedPosition;

    [Header("Audio")]
    public AudioClip openObjectAudio;
    public AudioClip closeObjectAudio;

    void Start()
    {
        // Guardar posición cerrada como referencia para el modo "Move"
        closedPosition = transform.localPosition;
    }

    public void ChangeObjectState()
    {
        objectOpen = !objectOpen;

        // Reproducir audio de forma directa al cambiar el estado
        AudioClip clipToPlay = objectOpen ? openObjectAudio : closeObjectAudio;
        if (clipToPlay != null)
        {
            AudioSource.PlayClipAtPoint(clipToPlay, transform.position, 1);
        }
    }

    void Update()
    {
        if (interactionType == InteractionType.Rotate)
        {
            Quaternion targetRotation = objectOpen ?
                Quaternion.Euler(0, objectOpenAngle, 0) :
                Quaternion.Euler(0, objectCloseAngle, 0);

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        else if (interactionType == InteractionType.Move)
        {
            Vector3 targetPosition = objectOpen ?
                closedPosition + openPositionOffset :
                closedPosition;

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, smooth * Time.deltaTime);
        }
    }
}
