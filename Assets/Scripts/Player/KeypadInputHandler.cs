using UnityEngine;

public class KeypadInputHandler : MonoBehaviour
{
    [SerializeField] private Camera keypadCamera;
    [SerializeField] private LayerMask buttonLayer;
    [SerializeField] private float maxDistance = 5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = keypadCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, buttonLayer))
            {
                var button = hit.collider.GetComponent<NavKeypad.KeypadButton>();
                if (button != null)
                {
                    button.PressButton();
                }
            }
        }
    }
}
