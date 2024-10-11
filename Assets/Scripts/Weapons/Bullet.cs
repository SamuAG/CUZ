using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Lógica para aplicar daño a los objetos impactados
        Debug.Log($"Impacto con {collision.gameObject.name}");
        Destroy(gameObject);
    }
}
