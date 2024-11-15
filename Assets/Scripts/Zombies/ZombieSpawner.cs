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
    private float baseSpawnInterval = 1.5f;

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
        float spawnInterval = Mathf.Max(0.5f, baseSpawnInterval - (round - 1) * 0.2f);

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
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
            if (round >= 3 && Random.value <= 0.05f)
            {
                zombieScript.SetSpeed(6f);
            }
            else
            {
                zombieScript.SetSpeed(zombieScript.DefaultSpeed);
            }
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

        if (zombiesRemaining <= 0)
        {
            StartCoroutine(PlayNextRound());
        }
    }

    private IEnumerator PlayNextRound()
    {
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlaySFX("NewRound");
        yield return new WaitForSeconds(3f);
        gM.AddRound(1);
        yield return new WaitForSeconds(1f);
        AudioManager.Instance.PlaySFX("NextRoundVoice");
    }

    private void OnDestroy()
    {
        gM.OnUpdateRounds -= StartNewRound;
    }
}
