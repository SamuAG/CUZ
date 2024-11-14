using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] InputManagerSO input;
    [SerializeField] Transform cam;
    [SerializeField] GameObject interactUI;
    [SerializeField] float range = 5f;
    [SerializeField] LayerMask layerMask;

    private IInteract target = null;

    private void OnEnable()
    {
        input.OnInteractStarted += TryInteract;
    }

    private void OnDisable()
    {
        input.OnInteractStarted -= TryInteract;
    }

    private void Update()
    {
        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, range, layerMask))
        {
            target = hit.collider.gameObject.GetComponent<IInteract>();
            if (target != null)
                interactUI.SetActive(true);
            else
                interactUI.SetActive(false);
        }
        else
        {
            interactUI.SetActive(false);
            target = null;
        }
    }

    private void TryInteract()
    {
        if(interactUI.activeSelf)
            target.interact();
    }
}
