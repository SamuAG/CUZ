using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private GameObject zombiePrefab;

    private List<Transform> spawnPoints = new List<Transform>();
    private int round = 0;
    private int zombiesRemaining = 0;
    private float healthIncrement = 20f;

    void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }

        gM.OnUpdateRounds += StartNewRound;
        StartNewRound(gM.Rounds);
    }

    private void StartNewRound(int currentRound)
    {
        round = currentRound;
        zombiesRemaining = Mathf.Min(24 + (round - 1) * 6, 24 * 4); 
        StartCoroutine(SpawnZombies());
    }

    private IEnumerator SpawnZombies()
    {
        for (int i = 0; i < zombiesRemaining; i++)
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
        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        Zombie zombieScript = zombie.GetComponent<Zombie>();
        zombieScript.Health = (100f + (round - 1) * healthIncrement);
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

    // Limpiamos la suscripción para que no haya problemas
    private void OnDestroy()
    {
        gM.OnUpdateRounds -= StartNewRound;
    }
}
