using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, Damageable
{

    private NavMeshAgent agent;
    private PlayerBasics targetPlayer;
    private Animator anim;
    private float damage = 1f;
    private float health = 100f;

    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float AttackRadius;

    void Start()
    {
        targetPlayer = gameManager.Player;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        agent.SetDestination(targetPlayer.transform.position);
        if (agent.remainingDistance <= agent.stoppingDistance && health > 0)
        {
            Vector3 targetLocation = (targetPlayer.transform.position - transform.position).normalized;
            targetLocation.y = 0;
            Quaternion rotationToTarget = Quaternion.LookRotation(targetLocation);
            transform.rotation = rotationToTarget;
            agent.isStopped = true;
            anim.SetBool("Attacking", true);
        }
    }

    //Referenciado en la animacion
    private void PlayerNearAfterAttack()
    {
        agent.isStopped = false;
        anim.SetBool("Attacking", false);
    }

    //Referenciado en la animacion
    private void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(AttackPoint.position, AttackRadius);
        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out Damageable damageable) && collider.CompareTag("Player"))
            {
                damageable.ApplyDamage(damage);
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        //Hacer animacion de muerte
        if (health < 0) Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(AttackPoint.position, AttackRadius);
    }
}
