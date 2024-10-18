using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifeTime = 5f;

    // TODO: Implementar lógica para aplicar daño a los objetos impactados

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Lógica para aplicar daño a los objetos impactados
        Debug.Log($"Impacto con {collision.gameObject.name}");
        ApplyDamage(collision.gameObject);
    }

    protected abstract void ApplyDamage(GameObject target);
}
