using System;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, Damageable
{
    private NavMeshAgent agent;
    private PlayerBasics targetPlayer;
    private Animator anim;
    private float damage = 1f;
    private float health;

    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float AttackRadius;
    [SerializeField] private float attackDistance = 2f;

    public float Health { get => health; set => health = value; }

    void Start()
    {
        targetPlayer = gameManager.Player;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.SetDestination(targetPlayer.transform.position);
    }

    void Update()
    {
        agent.SetDestination(targetPlayer.transform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        if (distanceToPlayer <= attackDistance && health > 0)
        {
            Vector3 targetLocation = (targetPlayer.transform.position - transform.position).normalized;
            targetLocation.y = 0;
            Quaternion rotationToTarget = Quaternion.LookRotation(targetLocation);
            transform.rotation = rotationToTarget;
            agent.isStopped = true;
            anim.SetBool("Attacking", true);
        }
        else
        {
            agent.isStopped = false;
            anim.SetBool("Attacking", false);
        }
    }

    // Referenciado en la animación
    private void PlayerNearAfterAttack()
    {
        agent.isStopped = false;
        anim.SetBool("Attacking", false);
    }

    // Referenciado en la animación
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
        if (health <= 0)
        {
            ZombieDie();
        }
    }

    private void ZombieDie()
    {
        ZombieSpawner spawner = FindObjectOfType<ZombieSpawner>();
        if (spawner != null)
        {
            spawner.DecreaseZombieCount();
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(AttackPoint.position, AttackRadius);
    }
}
