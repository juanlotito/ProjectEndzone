using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnPoints; 
    [SerializeField] private FemaleZombieAController femaleAZombiePrefab;
    [SerializeField] private MaleZombieAController maleAZombiePrefab;
    [SerializeField] private LevelData levelData;
    private float spawnTimer = 0f; 

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= levelData.spawnTime)
        {
            SpawnZombies();
            spawnTimer = 0f;
        }
    }

    private void SpawnZombies()
    {
        var currentZombies = GameManager.instance.currentZombies;

        if (currentZombies >= levelData.maxZombies)
        {
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPointChosen = spawnPoints[spawnIndex].transform;

        int randomZombieType = Random.Range(0, 2); // 0 para ZombieMale, 1 para ZombieFemale (POR AHORA; TODO: MAS ZOMBIES)

        ZombieController zombieInstance;

        if (randomZombieType == 0)
        {
            zombieInstance = Instantiate(maleAZombiePrefab, spawnPointChosen.position, spawnPointChosen.rotation);
        }
        else
        {
            zombieInstance = Instantiate(femaleAZombiePrefab, spawnPointChosen.position, spawnPointChosen.rotation);
        }

        GameManager.instance.AddZombie();
    }
}
