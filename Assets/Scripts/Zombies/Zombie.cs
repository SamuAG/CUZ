using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{

    private NavMeshAgent agent;
    private PlayerBasics targetPlayer;
    private Animator anim;
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
            anim.SetBool("Attacking", true);
        }
    }
}
