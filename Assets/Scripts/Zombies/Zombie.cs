using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, Damageable
{
    private NavMeshAgent agent;
    private PlayerBasics targetPlayer;
    private Animator anim;
    private float damage = 10f;
    private float health;
    private bool isDead = false;
    private AudioSource audioSource;
    private const float soundProbability = 0.2f;

    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float AttackRadius;
    [SerializeField] private float attackDistance = 2f;

    [SerializeField] private AudioClip[] zombieSound;
    [SerializeField] private AudioClip[] zombieAttack;

    public float Health { get => health; set => health = value; }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    void OnEnable()
    {
        ResetZombie();
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        if (isDead) return;

        if (agent.enabled)
        {
            agent.SetDestination(targetPlayer.transform.position);
        }

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

    private void PlayerNearAfterAttack()
    {
        agent.isStopped = false;
        anim.SetBool("Attacking", false);
    }

    private void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(AttackPoint.position, AttackRadius);
        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out Damageable damageable) && collider.CompareTag("Player"))
            {
                int randomIndex = Random.Range(0, zombieAttack.Length);
                audioSource.PlayOneShot(zombieAttack[randomIndex]);
                damageable.ApplyDamage(damage);
            }
        }
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
        gameManager.AddPoints(10);

        if (health <= 0)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            isDead = true;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.enabled = false;
            anim.SetTrigger("IsDead");
            ZombieSpawner spawner = FindObjectOfType<ZombieSpawner>();
            if (spawner != null)
            {
                spawner.DecreaseZombieCount();
            }
            StartCoroutine(DeactivateZombie());

            gameManager.AddPoints(80);
        }
    }

    private IEnumerator DeactivateZombie()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(AttackPoint.position, AttackRadius);
    }

    private IEnumerator PlaySound()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(3f);

            if (Random.value < soundProbability)
            {
                int randomIndex = Random.Range(0, zombieSound.Length);
                audioSource.PlayOneShot(zombieSound[randomIndex]);
            }
        }
    }

    public void ResetZombie()
    {
        isDead = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        //Nos aseguramos que el zombie est� en el NavMesh
        if (!agent.isOnNavMesh)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit navMeshHit, 1.0f, NavMesh.AllAreas))
            {
                transform.position = navMeshHit.position;
            }
        }

        agent.enabled = true;
        agent.isStopped = false;
        anim.ResetTrigger("IsDead");
        anim.SetBool("Attacking", false);
        targetPlayer = gameManager.Player;
        StartCoroutine(PlaySound());
    }
}
