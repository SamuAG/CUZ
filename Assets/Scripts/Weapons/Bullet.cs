using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 50f;
    private float damage = 10f;
    private float lifeTime = 5f;

    public float BulletDamage { get => damage; set => damage = value; }

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
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Debug.Log("ZombieHit");
            collision.gameObject.GetComponent<Zombie>().ApplyDamage(damage);
        }
    }
}
