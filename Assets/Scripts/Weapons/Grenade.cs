using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float impulseForce;
    [SerializeField] private float lifeTime;
    [SerializeField] private float explotionRadius;
    [SerializeField] private float explotionForce;
    [SerializeField] private LayerMask whatIsDamagable;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 forceDirection = (transform.forward + transform.up).normalized;
        GetComponent<Rigidbody>().AddForce(forceDirection * impulseForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Collider[] collidersDetected = Physics.OverlapSphere(transform.position, explotionRadius);

            foreach(Collider coll in collidersDetected)
            {
                if (coll.CompareTag("Zombie"))
                {
                    coll.GetComponent<Rigidbody>().isKinematic = false;
                    coll.GetComponent<Rigidbody>().AddExplosionForce(explotionForce, transform.position, explotionRadius, 1.5f, ForceMode.Impulse);
                }
            }
            Destroy(gameObject);
        }
    }
}
