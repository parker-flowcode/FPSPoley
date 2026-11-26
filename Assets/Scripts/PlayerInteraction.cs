using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float range = 3f;
    public LayerMask interactableLayer;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

        if (Physics.Raycast(ray, out hit, range, interactableLayer))
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                // Si el objeto (o su padre) tiene un script de entrega/baúl, lo activamos
                DepositBox box = hit.collider.GetComponentInParent<DepositBox>();
                if (box != null)
                {
                    box.DepositCoins();
                }
            }
        }
    }
}