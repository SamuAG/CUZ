using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, Damageable
{

    private NavMeshAgent agent;
    private PlayerBasics targetPlayer;
    private Animator anim;
    private float health = 100f;
    [SerializeField] private GameManagerSO gameManager;

    void Start()
    {
        targetPlayer = gameManager.Player;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        agent.SetDestination(targetPlayer.transform.position);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 targetLocation = (targetPlayer.transform.position - transform.position).normalized;
            targetLocation.y = 0;
            Quaternion rotationToTarget = Quaternion.LookRotation(targetLocation);
            transform.rotation = rotationToTarget;
            agent.isStopped = true;
            anim.SetBool("Attacking", true);
        }
    }

    private void PlayerNearAfterAttack()
    {
        agent.isStopped = false;
        anim.SetBool("Attacking", false);
    }

    public void ApplyDamage(float damage)
    {
        health-= damage;
        //Hacer animacion de muerte
        if (health < 0) Destroy(gameObject);
    }
}
