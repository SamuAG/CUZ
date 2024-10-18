using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : Bullet
{
    protected override void ApplyDamage(GameObject target)
    {
        // TODO: Lógica para aplicar daño a los objetos impactados
        if(target.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            //target.GetComponent<Health>().TakeDamage(damage);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
