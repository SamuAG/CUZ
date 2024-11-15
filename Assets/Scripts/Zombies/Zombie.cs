using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, Damageable
{
    private NavMeshAgent agent;
    private PlayerBasics targetPlayer;
    private Animator anim;
    private float damage = 25f;
    private float health;
    private bool isDead = false;
    private AudioSource audioSource;
    private const float soundProbability = 0.2f;
    private Rigidbody[] bones;
    private Collider[] bonesColliders;
    private float defaultSpeed;

    [SerializeField] private GameManagerSO gameManager;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private GameObject maxAmmoPrefab;
    [SerializeField] private GameObject instaKillPrefab;

    [SerializeField] private AudioClip[] zombieSound;
    [SerializeField] private AudioClip[] zombieAttack;

    public float Health { get => health; set => health = value; }
    public float DefaultSpeed { get => defaultSpeed; set => defaultSpeed = value; }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponentInParent<AudioSource>();
        bones = GetComponentsInChildren<Rigidbody>();
        bonesColliders = GetComponentsInChildren<Collider>();

        defaultSpeed = agent.speed;
        foreach (Rigidbody rigidbody in bones)
        {
            rigidbody.isKinematic = true;
        }
    }

    void OnEnable()
    {
        ResetZombie();
        gameManager.OnStopZombies += StopFollow;
    }

    void OnDisable()
    {
        StopAllCoroutines();
        gameManager.OnStopZombies -= StopFollow;
    }

    void Update()
    {
        if (isDead || !agent.enabled || !targetPlayer) return;

        agent.SetDestination(targetPlayer.transform.position);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && health > 0)
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

    // Llamado desde la animación
    private void PlayerNearAfterAttack()
    {
        agent.isStopped = false;
        anim.SetBool("Attacking", false);
    }

    // Llamado desde la animación
    private void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius);
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

        if (health <= 0 || gameManager.Player.InstaKillEnabled)
        {
            ZombieDie();
        }
    }

    public void SetSpeed(float speed)
    {
        if (agent != null)
        {
            agent.speed = speed;
        }
    }

    private void ZombieDie()
    {
        isDead = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.enabled = false;
        anim.SetTrigger("IsDead");

        foreach (var collider in bonesColliders)
        {
            collider.enabled = false;
        }

        // PowerUp
        if (Random.value <= 0.05f)
        {
            GameObject powerUp;
            if (Random.value < 0.5f)
            {
                powerUp = maxAmmoPrefab;
                Instantiate(powerUp, transform.position + Vector3.up * 1.2f, Quaternion.identity);
            }
            else
            {
                powerUp = instaKillPrefab;
                Instantiate(powerUp, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            }
        }

        ZombieSpawner spawner = FindObjectOfType<ZombieSpawner>();
        if (spawner != null)
        {
            spawner.DecreaseZombieCount();
        }
        StartCoroutine(DeactivateZombie());

        gameManager.AddPoints(80);
    }


    private IEnumerator DeactivateZombie()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRadius);
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
        agent.speed = defaultSpeed;
        foreach (var collider in bonesColliders)
        {
            collider.enabled = true;
        }

        foreach (var rb in bones)
        {
            rb.isKinematic = true;
        }

        // Nos aseguramos de que el zombi este en el NavMesh
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

    private void StopFollow()
    {
        agent.isStopped = true;   
        targetPlayer = null;      
        anim.SetBool("Attacking", false);
        anim.SetBool("Walking", false);   
    }
}
