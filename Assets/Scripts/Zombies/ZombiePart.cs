using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePart : MonoBehaviour
{
    [SerializeField] private BodyPartType bodyPartType;
    [SerializeField] private Zombie parentZombie;
    [SerializeField] GameManagerSO gM;
    [SerializeField] private enum BodyPartType { Head, Body, Limb}

    public float DamageMultiplier
    {
        get
        {
            switch (bodyPartType)
            {
                case BodyPartType.Head:
                    return 1.5f;
                case BodyPartType.Body:
                    return 1.0f;
                case BodyPartType.Limb:
                    return 0.5f;
                default:
                    return 1.0f;
            }
        }
    }

    void Awake()
    {
        parentZombie = GetComponentInParent<Zombie>();
    }

    public void Hit(float baseDamage)
    {
        if (parentZombie != null)
        {
            float finalDamage = baseDamage * DamageMultiplier;
            parentZombie.ApplyDamage(finalDamage);
        }
    }
}
