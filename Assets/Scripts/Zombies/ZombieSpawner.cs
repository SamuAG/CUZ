using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private GameObject zombiePrefab;

    private List<Transform> spawnPoints = new List<Transform>();
    private int round = 0;
    private int zombiesToSpawn = 0;
    private int zombiesRemaining = 0;
    private float healthIncrement = 20f;

    private List<GameObject> zombiePool = new List<GameObject>();
    private int initialPoolSize = 50; 

    void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }

        // Creamos los zombies de la pool
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombie.SetActive(false);
            zombiePool.Add(zombie);
        }

        gM.OnUpdateRounds += StartNewRound;
        StartNewRound(gM.Rounds);
    }

    private void StartNewRound(int currentRound)
    {
        round = currentRound;
        zombiesToSpawn = Mathf.Min(24 + (round - 1) * 6, 24 * 4);
        zombiesRemaining = zombiesToSpawn;
        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SpawnZombie()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector3 randomOffset = new Vector3(
            Random.Range(-2f, 2f),
            0,
            Random.Range(-2f, 2f)
        );

        Vector3 spawnPosition = spawnPoint.position + randomOffset;

        //Nos aseguremos de que el zombie este siempre spawne en una zona valida si no es viable se elimina el zombie
        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit navMeshHit, 1.0f, NavMesh.AllAreas))
        {
            spawnPosition = navMeshHit.position;
        }
        else
        {
            DecreaseZombieCount();
            return; 
        }

        GameObject zombie = GetZombieFromPool();
        if (zombie != null)
        {
            zombie.transform.position = spawnPosition;
            zombie.transform.rotation = Quaternion.identity;
            zombie.SetActive(true);

            Zombie zombieScript = zombie.GetComponent<Zombie>();
            zombieScript.Health = (100f + (round - 1) * healthIncrement);
            zombieScript.ResetZombie(); // Reiniciamos el estado del zombie
        }
    }

    private GameObject GetZombieFromPool()
    {
        foreach (var zombieInPool in zombiePool)
        {
            if (!zombieInPool.activeInHierarchy)
            {
                return zombieInPool;
            }
        }

        // Opcionalmente, expandimos el pool
        GameObject newZombie = Instantiate(zombiePrefab);
        newZombie.SetActive(false);
        zombiePool.Add(newZombie);
        return newZombie;
    }

    public void DecreaseZombieCount()
    {
        zombiesRemaining--;
        Debug.Log("Zombi muerto. Zombies restantes: " + zombiesRemaining);

        if (zombiesRemaining <= 0)
        {
            gM.AddRound(1); // Si no quedan zombies, avanzamos de ronda
        }
    }

    private void OnDestroy()
    {
        gM.OnUpdateRounds -= StartNewRound;
    }
}
