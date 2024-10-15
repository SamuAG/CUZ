using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] InputManagerSO input;
    [SerializeField] Transform cam;
    [SerializeField] float range = 5f;
    [SerializeField] LayerMask layerMask;
    private void OnEnable()
    {
        input.OnReloadStarted += TryInteract;
    }

    private void OnDisable()
    {
        input.OnReloadStarted -= TryInteract;
    }

    private void TryInteract()
    {
        // Raycast para detectar objetos interactuables desde la cámara
        RaycastHit hit;
        Debug.Log("aaaa");
        if (Physics.Raycast(cam.position, cam.forward, out hit, range, layerMask))
        {
            Debug.Log(hit.collider.gameObject.name);
            IInteract target;
            if (hit.collider.gameObject.TryGetComponent<IInteract>(out target))
            {
                Debug.Log("aaaa");
                target.interact();
            }
        }
    }
}
